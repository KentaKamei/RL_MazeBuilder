using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEvaluator : MonoBehaviour
{
    public MazeGenerator mazeGen; // 生成された迷路を参照
    public Vector2Int start = new Vector2Int(0, 0);
    public Vector2Int goal;

    IEnumerator Start()
    {
        // MazeGenerator の初期化完了を少し待つ
        yield return new WaitForSeconds(0.1f);

        if (mazeGen == null || mazeGen.walkable == null)
        {
            Debug.LogError("mazeGen または walkable が null です！");
            yield break;
        }

        goal = new Vector2Int(mazeGen.width - 1, mazeGen.height - 1);
        int steps = EvaluateMazeBFS();
        Debug.Log($"評価結果：最短ルートのステップ数 = {steps}");
    }
    int EvaluateMazeBFS()
    {
        int width = mazeGen.width;
        int height = mazeGen.height;
        bool[,] walkable = mazeGen.walkable;

        bool[,] visited = new bool[width, height];
        int[,] distance = new int[width, height];

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited[start.x, start.y] = true;
        distance[start.x, start.y] = 0;

        Vector2Int[] directions = {
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.down
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == goal)
                return distance[current.x, current.y];

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;

                if (next.x >= 0 && next.x < width &&
                    next.y >= 0 && next.y < height &&
                    walkable[next.x, next.y] &&
                    !visited[next.x, next.y])
                {
                    visited[next.x, next.y] = true;
                    distance[next.x, next.y] = distance[current.x, current.y] + 1;
                    queue.Enqueue(next);
                }
            }
        }

        // ゴールにたどり着けなかった場合
        return -1;
    }
}
