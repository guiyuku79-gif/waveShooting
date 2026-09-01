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

    [SerializeField] private float originalMoveInterval = 0.1f;

    private float moveInterval;
    private float deltaTimeCount;
    [SerializeField] public int division = 100;//100個の点で表現

    public List<float> PlayerWavePowers = new List<float>();
    public List<int> PlayerWaveIds = new List<int>();

    public List<float> EnemyWavePowers = new List<float>();
    public List<int> EnemyWaveIds = new List<int>();

    //次に出す波のIDを決める
    private int playerWaveId;
    private int enemyWaveId;

    private List<float> NextEnemyWaveList = new List<float>();

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

        playerWaveId = 1;
        enemyWaveId = 1;

        //仮
        for (int i = 0; i < 20; i++)
        {
            NextEnemyWaveList.Add(i * 0.1f);
        }
    }

    void Update()
    {
        deltaTimeCount += Time.deltaTime;
        if (deltaTimeCount >= moveInterval)
        {
            deltaTimeCount = 0;
            MoveWave();
        }

        moveInterval = IntervalChange();

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            MakeSinWave();
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


            PlayerWaveIds[0] = playerWaveId;
            PlayerWavePowers[0] = mouseWorldPos.y;
            EnemyWavePowers[division - 1] = 0f;
            EnemyWaveIds[division - 1] = 0;

        }
        else
        {
            PlayerWavePowers[0] = 0f;
            PlayerWaveIds[0] = 0;

        }

        if (NextEnemyWaveList.Count == 0)
        {
            EnemyWavePowers[division - 1] = 0f;
            EnemyWaveIds[division - 1] = 0;
        }
        else
        {
            EnemyWavePowers[division - 1] = NextEnemyWaveList[0];
            EnemyWaveIds[division - 1] = enemyWaveId;
            NextEnemyWaveList.RemoveAt(0);
        }

        WaveIDCheck();
    }

    private void WaveIDCheck()
    {
        if (PlayerWaveIds[0] == 0 && PlayerWaveIds[1] != 0)
        {
            playerWaveId++;
            Debug.Log(playerWaveId);
        }
        if (EnemyWaveIds[division - 1] == 0 && EnemyWaveIds[division - 2] != 0)
        {
            enemyWaveId++;
        }
    }

    //波が重なっているほど時間をゆっくりにする
    private float IntervalChange()
    {
        int onCount = 0;
        int offCount = 0;

        for (int i = 0; i < division; i++)
        {
            if (PlayerWaveIds[i] != 0 && EnemyWaveIds[i] != 0)
            {
                onCount++;
            }
            else if (PlayerWaveIds[i] != 0 || EnemyWaveIds[i] != 0)
            {
                offCount++;
            }
        }



        if (onCount + offCount == 0) return originalMoveInterval;


        //int型同士の計算はintになってしまうので気を付ける
        return originalMoveInterval * (1 + (float)onCount / (onCount + offCount));

    }

    private void MakeSinWave()
    {
        for (int i = 0; i < 30; i++)
        {
            NextEnemyWaveList.Add(Mathf.Sin(Mathf.PI * i / 15));
        }
    }
}
