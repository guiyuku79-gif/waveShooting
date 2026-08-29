using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class LineSample : MonoBehaviour
{
    public LineRenderer line;

    public int pointCount = 200;
    public float amplitude = 1f;
    public float frequency = 2f;
    public float width = 10f;

    [SerializeField] private float moveInterval = 0.1f;
    private float deltaTimeCount;
    [SerializeField] public int division = 100;//100個の点で表現

    public List<float> wavePowers = new List<float>();
    public List<int> waveIds = new List<int>();

    public static LineSample Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < division; i++)
        {
            wavePowers.Add(0f);
            waveIds.Add(0);
        }

        deltaTimeCount = 0f;
    }

    void Update()
    {
        deltaTimeCount += Time.deltaTime;
        if (deltaTimeCount >= moveInterval)
        {
            deltaTimeCount = 0;
            MoveWave();
        }
    }

    private void MoveWave()
    {
        for (int i = division - 1; i > 0; i--)
        {
            wavePowers[i] = wavePowers[i - 1];
            waveIds[i] = waveIds[i - 1];
        }



        if (Mouse.current.leftButton.isPressed)
        {
            waveIds[0] = 1;

            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f)
            );

            wavePowers[0] = mouseWorldPos.y;
        }
        else
        {
            wavePowers[0] = 0f;
            waveIds[0] = 0;
        }
    }
}
