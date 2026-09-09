using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class LineSample : MonoBehaviour
{
    [SerializeField] GameObject Player;
    public LineRenderer line;

    [SerializeField] private float originalMoveInterval = 0.1f;

    private float moveInterval;
    private float deltaTimeCount;
    [SerializeField] public int division = 100;//100個の点で表現

    [NonSerialized] public Medium PlayerMedium;
    [NonSerialized] public Medium EnemyMedium;


    private List<float> NextEnemyWaveList = new List<float>();

    //シングルトン
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
        PlayerMedium = new Medium(division);
        EnemyMedium = new Medium(division);


        deltaTimeCount = 0f;

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

        WavePowerCheck();

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            MakeSinWave();
        }
    }

    private void MoveWave()
    {

        if (Mouse.current.leftButton.isPressed)
        {
            PlayerMedium.RightWaveMove(Player.transform.position.y);
        }
        else
        {
            PlayerMedium.RightWaveMove(0);
        }

        if (NextEnemyWaveList.Count == 0)
        {
            EnemyMedium.LeftWaveMove(0);
        }
        else
        {
            EnemyMedium.LeftWaveMove(NextEnemyWaveList[0]);
            NextEnemyWaveList.RemoveAt(0);
        }

        WaveIDCheck();
    }

    private void WaveIDCheck()
    {
        if (PlayerMedium.waveIds[0] == 0 && PlayerMedium.waveIds[1] != 0)
        {
            PlayerMedium.nextWaveId++;
            Debug.Log(PlayerMedium.nextWaveId);
        }
        if (EnemyMedium.waveIds[division - 1] == 0 && EnemyMedium.waveIds[division - 2] != 0)
        {
            EnemyMedium.nextWaveId++;
        }
    }

    //波が重なっているほど時間をゆっくりにする
    private float IntervalChange()
    {
        int onCount = 0;
        int offCount = 0;

        for (int i = 0; i < division; i++)
        {
            if (PlayerMedium.waveIds[i] != 0 && EnemyMedium.waveIds[i] != 0)
            {
                onCount++;
            }
            else if (PlayerMedium.waveIds[i] != 0 || EnemyMedium.waveIds[i] != 0)
            {
                offCount++;
            }
        }



        if (onCount + offCount == 0) return originalMoveInterval;


        //int型同士の計算はintになってしまうので気を付ける
        return originalMoveInterval * (1 + (float)onCount * 2 / (onCount + offCount));

    }

    private void MakeSinWave()
    {
        for (int i = 0; i < 30; i++)
        {
            NextEnemyWaveList.Add(Mathf.Sin(Mathf.PI * i / 15) * 2);
        }
    }

    private void WavePowerCheck()
    {
        PlayerMedium.WavePowerCheck(EnemyMedium);
        EnemyMedium.WavePowerCheck(PlayerMedium);

        PlayerMedium.DestroyTooSmallWave();
        EnemyMedium.DestroyTooSmallWave();
    }

}
