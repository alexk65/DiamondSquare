using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.HeightMapProviders;
using UnityEngine;
using Random = UnityEngine.Random;

public class TilemapManager : MonoBehaviour
{
    public int mapSizePower = 7;
    public float roughness = 20f;

    public List<GameObject> grassTiles;
    public List<GameObject> sandTiles;
    public List<GameObject> waterTiles;

    public List<GameObject> trees;

    private const float WaterLine = 0f;
    private const float SandLine = 0.5f;
    private const float TreesLine = 1.7f;

    private float[,] heightMap;
    /*private float[,] forestMap;*/

    private IHeightMapProvider heightMapProvider;

    private void Awake()
    {
        heightMapProvider = new DiamondSquareHeightMapProvider();
        heightMap = heightMapProvider.Generate(mapSizePower, roughness);
        /*forestMap = heightMapProvider.Generate(mapSizePower, 2f);*/

        GenerateTileMap();
        /*GenerateForests();*/
    }

    private void GenerateTileMap()
    {
        var maxSideSize = heightMap.GetLength(1);
        for (var y = 0; y < maxSideSize; y++)
        {
            for (var x = 0; x < maxSideSize; x++)
            {
                InstantiateLand(x, y);
                InstantiateTree(x, y);
            }
        }
    }

    /*private void GenerateForests()
    {
        var maxSideSize = forestMap.GetLength(1);
        for (var y = 0; y < maxSideSize; y++)
        {
            for (var x = 0; x < maxSideSize; x++)
            {
                if (heightMap[x,y] > SandLine && forestMap[x,y] > 0.6f)
                {
                    var tile = trees[Random.Range(0, trees.Count - 1)];
                    Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }*/

    private void InstantiateLand(int x, int y)
    {
        var tile = ResolveTile(heightMap[x, y]);
        Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity);
    }

    private void InstantiateTree(int x, int y)
    {
        if (heightMap[x, y] > TreesLine)
        {
            var tile = trees[Random.Range(0, trees.Count - 1)];
            Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity);
        }
    }

    private GameObject ResolveTile(float height)
    {
        List<GameObject> tiles = null;

        tiles = AttemptGetWaterTiles(tiles, height);
        tiles = AttemptGetSandTiles(tiles, height);
        tiles = AttemptGetGrassTiles(tiles, height);

        return tiles[Random.Range(0, tiles.Count - 1)];
    }

    private List<GameObject> AttemptGetWaterTiles(List<GameObject> tiles, float height)
    {
        if (tiles != null || height > WaterLine)
        {
            return tiles;
        }

        return waterTiles;
    }


    private List<GameObject> AttemptGetSandTiles(List<GameObject> tiles, float height)
    {
        if (tiles != null || height > SandLine)
        {
            return tiles;
        }

        return sandTiles;
    }


    private List<GameObject> AttemptGetGrassTiles(List<GameObject> tiles, float height)
    {
        if (tiles != null || height <= SandLine)
        {
            return tiles;
        }

        return grassTiles;
    }

    private GameObject ResolveGrayScaleTile(float height)
    {
        var tile = grassTiles.First();
        tile.GetComponent<SpriteRenderer>().color = new Color(height, height, height);

        return tile;
    }
}
