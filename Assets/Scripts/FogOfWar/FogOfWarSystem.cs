using UnityEngine;
using System.Collections.Generic;

public class FogOfWarSystem : MonoBehaviour
{
    [SerializeField] private int mapWidth = 100;
    [SerializeField] private int mapHeight = 100;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private Material fogOfWarMaterial;
    
    private bool[,] exploredMap;
    private bool[,] visibleMap;
    private Texture2D fogTexture;
    private RenderTexture fogRenderTexture;
    private List<Unit> units = new List<Unit>();

    void Start()
    {
        InitializeFogOfWar();
    }

    void InitializeFogOfWar()
    {
        exploredMap = new bool[mapWidth, mapHeight];
        visibleMap = new bool[mapWidth, mapHeight];
        
        // 텍스처 생성 (검은색 = 안 보임, 흰색 = 보임)
        fogTexture = new Texture2D(mapWidth, mapHeight, TextureFormat.RGBA32, false);
        UpdateFogTexture();
    }

    void LateUpdate()
    {
        UpdateVisibility();
    }

    void UpdateVisibility()
    {
        // 현재 프레임의 시야 초기화
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                visibleMap[x, y] = false;
            }
        }

        // 각 유닛의 시야 범위 계산
        foreach (Unit unit in units)
        {
            RevealArea(unit.transform.position, sightRange);
        }

        UpdateFogTexture();
    }

    void RevealArea(Vector3 position, float range)
    {
        // 월드 좌표를 맵 좌표로 변환
        int centerX = Mathf.RoundToInt(position.x / cellSize);
        int centerZ = Mathf.RoundToInt(position.z / cellSize);

        int radiusInCells = Mathf.RoundToInt(range / cellSize);

        for (int x = centerX - radiusInCells; x <= centerX + radiusInCells; x++)
        {
            for (int z = centerZ - radiusInCells; z <= centerZ + radiusInCells; z++)
            {
                if (IsInBounds(x, z))
                {
                    // 원 범위 체크
                    Vector2 cellPos = new Vector2(x * cellSize, z * cellSize);
                    Vector2 unitPos = new Vector2(position.x, position.z);
                    
                    if (Vector2.Distance(cellPos, unitPos) <= range)
                    {
                        visibleMap[x, z] = true;
                        exploredMap[x, z] = true; // 탐험된 영역은 유지
                    }
                }
            }
        }
    }

    void UpdateFogTexture()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Color pixelColor;
                
                if (visibleMap[x, y])
                {
                    pixelColor = Color.white; // 현재 보이는 영역
                }
                else if (exploredMap[x, y])
                {
                    pixelColor = Color.gray; // 탐험했지만 현재 보이지 않는 영역 (어두운 안개)
                }
                else
                {
                    pixelColor = Color.black; // 아직 탐험하지 않은 영역 (검은 안개)
                }
                
                fogTexture.SetPixel(x, y, pixelColor);
            }
        }
        
        fogTexture.Apply();
        fogOfWarMaterial.mainTexture = fogTexture;
    }

    bool IsInBounds(int x, int z)
    {
        return x >= 0 && x < mapWidth && z >= 0 && z < mapHeight;
    }

    public void RegisterUnit(Unit unit)
    {
        if (!units.Contains(unit))
            units.Add(unit);
    }

    public void UnregisterUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public bool IsVisible(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / cellSize);
        int z = Mathf.RoundToInt(position.z / cellSize);
        
        if (IsInBounds(x, z))
            return visibleMap[x, z];
        
        return false;
    }

    public bool IsExplored(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / cellSize);
        int z = Mathf.RoundToInt(position.z / cellSize);
        
        if (IsInBounds(x, z))
            return exploredMap[x, z];
        
        return false;
    }
}
