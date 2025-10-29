using System;
using GameManagers.Singleton;   
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameSceneManager sceneManager;
    public DeathManager deathManager;

    public int mapIndex; //맵 인덱스
    
    void Start()
    {
        sceneManager = FindFirstObjectByType<GameSceneManager>();
        deathManager = FindFirstObjectByType<DeathManager>();
        sceneManager.LoadScene(mapIndex); //처음 씬 로드

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
        deathManager = FindFirstObjectByType<DeathManager>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //R키 누르면
        {
            deathManager.Initialize(); //현재 씬 재시작
        }
        if (Input.GetKeyDown(KeyCode.T)) //R키 누르면
        {
            mapIndex = 0;
            sceneManager.LoadScene(mapIndex);
        }
        if (Input.GetKeyDown(KeyCode.N)) //N키 누르면
        {
            mapIndex++; //맵 인덱스 증가
            sceneManager.LoadScene(mapIndex);
        }
        
    }
    
}
