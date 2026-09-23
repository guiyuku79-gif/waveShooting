using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] GameObject linePrefab;

    [SerializeField] float width = 12;
    [SerializeField] float height = 4;
    [SerializeField] float gridSize = 1;

    void Start()
    {
        MakeGrid(4.5f);
        MakeGrid(0f);
        MakeGrid(-4.5f);
    }

    void MakeGrid(float laneY)
    {
        //横線
        for (int i = 0; i < (int)(height / gridSize) + 1; i++)
        {
            DrawOneGridLine(new Vector2(-width / 2, height / 2 - i * gridSize + laneY),
                            new Vector2(width / 2, height / 2 - i * gridSize + laneY));
        }

        DrawOneGridLine(new Vector2(-width / 2 - gridSize, laneY),
                new Vector2(width / 2 + gridSize, laneY));

        //縦線
        for (int i = 0; i < (int)(width / gridSize) + 1; i++)
        {
            DrawOneGridLine(new Vector2(-width / 2 + i * gridSize, height / 2 + laneY),
                            new Vector2(-width / 2 + i * gridSize, -height / 2 + laneY));
        }
    }

    void DrawOneGridLine(Vector2 startPos, Vector2 endPos)
    {
        GameObject lineObject = Instantiate(linePrefab);

        lineObject.transform.parent = transform;

        LineRenderer line = lineObject.GetComponent<LineRenderer>();

        // 頂点数の設定
        line.positionCount = 2;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);

        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        line.startColor = new Color32(255, 255, 255, 100);
        line.endColor = new Color32(255, 255, 255, 100);
    }
}
