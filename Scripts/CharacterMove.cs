using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    // ----------------------------
    // Inspector에 노출되는 설정 값들
    // ----------------------------
    // 플레이어의 최대 이동 속도
    public float maxSpeed;      // Inspector에서 튜닝하세요.
    public float jumpPower = 6.5f;

    // 컴포넌트 캐시
    Rigidbody2D rigid;          // 이 스크립트가 붙은 게임오브젝트의 Rigidbody2D
    SpriteRenderer spriteRenderer;
    public int playerIndex = 1;   // 멀티플레이어 입력 분기용 인덱스(예: 1P, 2P)
    public bool isJumping = false;    // 현재 점프 상태(공중에 떠 있는지 여부)
    



    void Awake()        //플레이어 오브젝트가 만들어졌을 때
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private string InputName(string baseName) //인풋 네임 설정 함수
    {
        if (playerIndex <= 0) return baseName;
        return $"{baseName}_{playerIndex}P"; // 예: "Vertical_1P"
    }
    private float Axis(string name) => Input.GetAxisRaw(InputName(name));
    private bool ButtonUp(string name) => Input.GetButtonUp(InputName(name));
    private bool ButtonDown(string name) => Input.GetButtonDown(InputName(name));
    private bool Button(string name) => Input.GetButton(InputName(name));

    // 위의 헬퍼들 설명:
    // - InputName: playerIndex값을 붙여서 멀티플레이어용 입력축/버튼 이름을 구성합니다.
    //   예: baseName = "Horizontal", playerIndex = 1 -> "Horizontal_1P"
    // - Axis / ButtonUp / ButtonDown / Button: 기존 구식 Input Manager API를 사용합니다.
    //   (Project Settings > Input Manager에 정의된 이름과 일치해야 정상 동작합니다.)

    //지속적인 키 입력은 FixedUpdate에서 하는 게 좋지만,
    //단발적인 키 입력은 Update에서 하는게 훨씬 더 좋다.
    //FixedUpdate는 1초에 약 50번 돌고, Update는 약 60번식 돈다
    //그래서 단발적인 키 입력은 FixedUpdate에서 하면 키가 씹힐 수도 있다.
    //이를 방지하기 위해 Update에다가 해주는 게 좋다.
    void Update()
    {
        // 단발 입력(버튼) 처리는 Update에서 합니다. FixedUpdate는 물리 프레임에서 호출되므로
        // 버튼 입력을 Update에서 읽어 물리 동작(velocity 변경 등)을 수행하도록 합니다.
        if (Button("Jump") && !isJumping)     // 점프 버튼을 누른 경우
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpPower); //x축 그대로 y축만 점프파워로 변경
            isJumping = true;
        }

        //Direction Sprite (방향 전환)
        //flipX는 Bool값이다. Input을 통해 들어오는 값이 -1이면 뒤집는다.

        if (ButtonUp("Horizontal"))//좌우 버튼을 떼는 순간에 속도를 0으로.
        {
            // 좌우 입력을 떼면 수평 속도를 0으로 만들어 관성 제거
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x * 0f, rigid.linearVelocity.y);
        }
    }

    void FixedUpdate()      //플레이어가 움직이는 물리연산이므로
    {
        //Move by Control
        float direction = Axis("Horizontal"); //방향을 인풋매니저로 받아옴
        rigid.AddForce(Vector2.right*direction,ForceMode2D.Impulse);
        //Right Max Speed
        //현재 물체의 속도가 최대 속도를 넘었다면
        //절대값x 속도를 최대속도로 한정
        // 수평 속도 제한 적용
        rigid.linearVelocity = new Vector2(Mathf.Clamp(rigid.linearVelocity.x, -maxSpeed, maxSpeed), rigid.linearVelocity.y);
        
        // 낙하 중(수직속도 < 0)일 때 바닥 체크를 해서 착지 상태로 전환
        if (rigid.linearVelocity.y < 0) // 내려오는 중
        {
            // 아래로 레이캐스트를 쏴서 Ground 레이어와 충돌하는지 확인
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
            if (rayHit.collider != null) // 바닥 감지
            {
                // 거리 체크: 너무 멀리 있으면 아직 착지 아님
                if (rayHit.distance < 1)
                    Debug.Log(rayHit.collider.name);
                    isJumping = false; // 착지했으므로 점프 상태 해제
            }
        }
    }
}