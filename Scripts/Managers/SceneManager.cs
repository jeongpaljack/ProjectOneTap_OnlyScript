using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public GameObject Popup;
    public Text PopupText; // 근데 이걸 씬메니저에서 하는게 맞나? 팝업메니저를 만드는게 좋아보이는데

    // 씬 전환: 다음 씬으로 이동 (빌드 인덱스 기준)

    // 특정 씬으로 이동 (인덱스 혹은 이름)


    public void LoadScene(int index)
    {
        GameManager.instance.mapIndex = index; //맵 인덱스 갱신
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }
    public void ShowClearPopup(string message = "Clear!")
    {
        Popup.SetActive(true);
        PopupText.text = message;
    }
}
