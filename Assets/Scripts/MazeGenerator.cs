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
    public bool[,] walkable; // 壁か通れるか


    void Start()
    {
        GenerateBacktrackPath();
        GenerateMaze();
    }

    void GenerateBacktrackPath()
    {
        path.Clear();

        bool[,] visited = new bool[width, height];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        Vector2Int current = new Vector2Int(0, 0);
        visited[current.x, current.y] = true;
        path.Add(current);
        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Peek();
            if (current == new Vector2Int(width - 1, height - 1))
            {
                break; // ゴールに着いたら探索終了！
            }

            List<Vector2Int> neighbors = new List<Vector2Int>();

            // 隣のマス（未訪問のみ）
            Vector2Int[] directions = {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;

                if (next.x >= 0 && next.x < width &&
                    next.y >= 0 && next.y < height &&
                    !visited[next.x, next.y])
                {
                    neighbors.Add(next);
                }
            }

            if (neighbors.Count > 0)
            {
                // ランダムに1マス選んで進む
                Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];

                visited[chosen.x, chosen.y] = true;
                path.Add(chosen);
                stack.Push(chosen);
            }
            else
            {
                // 行き止まりなので1つ戻る
                stack.Pop();
            }
        }
    }

    void GenerateMaze()
    {
        walkable = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int current = new Vector2Int(x, y);
                bool isPath = path.Contains(current); // 1本道の通路

                // 通路 or ランダムで壁なし
                bool placeWall = !isPath && Random.value > 0.0f;

                walkable[x, y] = !placeWall;

                Vector3 pos = new Vector3(x, y, 0);
                Instantiate(floorPrefab, pos, Quaternion.identity, transform);

                if (placeWall)
                {
                    Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                }           
            }
        }
    }
}
