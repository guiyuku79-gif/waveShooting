using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    [SerializeField] float width = 10;
    [SerializeField] float height = 4;
    [SerializeField] float gridSize = 1;

    void Start()
    {
        //横線
        for (int i = 0; i < (int)(height / gridSize) + 1; i++)
        {
            GameObject lineObject = new GameObject($"Line_{i}");

            lineObject.transform.parent = transform;

            LineRenderer line = lineObject.AddComponent<LineRenderer>();

            // 頂点数の設定
            line.positionCount = 2;
            line.SetPosition(0, new Vector2(-width / 2, height / 2 - i * gridSize));
            line.SetPosition(1, new Vector2(width / 2, height / 2 - i * gridSize));

            line.startWidth = 0.05f;
            line.endWidth = 0.05f;

            line.startColor = Color.white;
            line.endColor = Color.white;

        }

        //縦線
        for (int i = 0; i < (int)(width / gridSize) + 1; i++)
        {
            GameObject lineObject = new GameObject($"Line_{i}");

            lineObject.transform.parent = transform;

            LineRenderer line = lineObject.AddComponent<LineRenderer>();

            // 頂点数の設定
            line.positionCount = 2;
            line.SetPosition(0, new Vector2(-width / 2 + i * gridSize, height / 2));
            line.SetPosition(1, new Vector2(-width / 2 + i * gridSize, -height / 2));

            line.startWidth = 0.05f;
            line.endWidth = 0.05f;

        }
    }
}
