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

    public List<float> PlayerWavePowers = new List<float>();
    public List<int> PlayerWaveIds = new List<int>();

    public List<float> EnemyWavePowers = new List<float>();
    public List<int> EnemyWaveIds = new List<int>();

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
            PlayerWavePowers.Add(0f);
            PlayerWaveIds.Add(0);

            EnemyWavePowers.Add(0f);
            EnemyWaveIds.Add(0);
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
            PlayerWavePowers[i] = PlayerWavePowers[i - 1];
            PlayerWaveIds[i] = PlayerWaveIds[i - 1];
        }

        for (int i = 0; i < division - 1; i++)
        {
            EnemyWavePowers[i] = EnemyWavePowers[i + 1];
            EnemyWaveIds[i] = EnemyWaveIds[i + 1];
        }





        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f)
            );

            if (Keyboard.current.qKey.isPressed)
            {
                PlayerWaveIds[0] = 1;
                PlayerWavePowers[0] = mouseWorldPos.y;
                EnemyWavePowers[division - 1] = 0f;
                EnemyWaveIds[division - 1] = 0;
            }
            else
            {
                PlayerWavePowers[0] = 0f;
                PlayerWaveIds[0] = 0;
                EnemyWavePowers[division - 1] = mouseWorldPos.y;
                EnemyWaveIds[division - 1] = 1;
            }

        }
        else
        {
            PlayerWavePowers[0] = 0f;
            PlayerWaveIds[0] = 0;

            EnemyWavePowers[division - 1] = 0f;
            EnemyWaveIds[division - 1] = 0;
        }
    }
}
