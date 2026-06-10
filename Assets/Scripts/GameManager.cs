using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IsometricCamera isometricCamera;
    [SerializeField] private FogOfWarSystem fogOfWarSystem;
    [SerializeField] private WeatherSystem weatherSystem;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject unitPrefab;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        // 플레이어 유닛 생성
        if (playerSpawnPoint != null && unitPrefab != null)
        {
            GameObject player = Instantiate(unitPrefab, playerSpawnPoint.position, Quaternion.identity);
            player.name = "Player";
            
            // 카메라 타겟 설정
            if (isometricCamera != null)
            {
                isometricCamera.SetTarget(player.transform);
            }
        }

        Debug.Log("Game Initialized!");
        Debug.Log("Left Click: Select Unit");
        Debug.Log("Right Click: Move Selected Unit");
    }

    void Update()
    {
        UpdateGameState();
    }

    void UpdateGameState()
    {
        // 게임 상태 업데이트 로직
        if (weatherSystem != null)
        {
            WeatherSystem.WeatherState weather = weatherSystem.GetCurrentWeather();
            float rainIntensity = weatherSystem.GetRainIntensity();
            
            // 날씨에 따른 게임 로직 추가 가능
        }
    }
}
