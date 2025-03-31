using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public int width = 7;
    public int height = 7;

    private HashSet<Vector2Int> path = new HashSet<Vector2Int>();

    void Start()
    {
        GenerateGuaranteedPath();
        GenerateMaze();
    }

    void GenerateGuaranteedPath()
    {
        // スタートからゴールまで1本道を確保（右 or 上に進む）
        int x = 0, y = 0;
        path.Add(new Vector2Int(x, y));

        while (x < width - 1 || y < height - 1)
        {
            if (x < width - 1 && (y == height - 1 || Random.value > 0.5f))
            {
                x++; // 右へ
            }
            else
            {
                y++; // 上へ
            }
            path.Add(new Vector2Int(x, y));
        }
    }

    void GenerateMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                Instantiate(floorPrefab, pos, Quaternion.identity, transform);

                Vector2Int current = new Vector2Int(x, y);

                // 通路として確保したマス以外にランダムで壁を置く
                if (!path.Contains(current) && Random.value > 0.5f)
                {
                    Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                }
            }
        }
    }
}
