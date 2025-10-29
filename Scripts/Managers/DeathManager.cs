using GameManagers.Singleton; 
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathManager : Singleton<DeathManager>
{
    public GameSceneManager sceneManager;
    public Transform spawnPoint;

    void Start()
    {
        sceneManager = FindFirstObjectByType<GameSceneManager>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 바뀔 때마다 씬 전용 매니저 참조 갱신
        sceneManager = FindFirstObjectByType<GameSceneManager>();
    }

    public void Initialize() // 현재 씬을 로드 (UnityEngine.SceneManagement.SceneManager 사용)
    {
        sceneManager.LoadScene(GameManager.instance.mapIndex);
    }
}
