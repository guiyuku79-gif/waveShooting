using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class LineSample : MonoBehaviour
{
    [SerializeField] GameObject Player;

    [SerializeField] GameObject waveLendererObject;
    public LineRenderer line;

    private float originalMoveInterval;

    private float moveInterval;
    private float deltaTimeCount;


    [NonSerialized] public PlayerMedium playerMedium;
    [NonSerialized] public EnemyMedium enemyMedium;


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
        playerMedium = new PlayerMedium();
        enemyMedium = new EnemyMedium();

        deltaTimeCount = 0f;

        originalMoveInterval = 1 / (Constants.Division / Constants.Width * Constants.WaveSpeed);

        GameObject gameObject = Instantiate(waveLendererObject);
        gameObject.GetComponent<OneWaveLenderer>().Init(playerMedium, 2);
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

        //仮
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            enemyMedium.MakeSinWave(4, 2, 1);
        }
    }

    private void MoveWave()
    {
        playerMedium.WaveMove(Mouse.current.leftButton.isPressed, Player.transform.position.y);

        enemyMedium.WaveMove();

    }


    //波が重なっているほど時間をゆっくりにする
    private float IntervalChange()
    {
        int onCount = 0;
        int offCount = 0;

        for (int i = 0; i < Constants.Division; i++)
        {
            if (playerMedium.waveIds[i] != 0 && enemyMedium.waveIds[i] != 0)
            {
                onCount++;
            }
            else if (playerMedium.waveIds[i] != 0 || enemyMedium.waveIds[i] != 0)
            {
                offCount++;
            }
        }

        if (onCount + offCount == 0) return originalMoveInterval;
        //int型同士の計算はintになってしまうので気を付ける
        return originalMoveInterval * (1 + (float)onCount * 2 / (onCount + offCount));

    }

    private void WavePowerCheck()
    {
        playerMedium.WavePowerCheck(enemyMedium);
        enemyMedium.WavePowerCheck(playerMedium);

        playerMedium.DestroyTooSmallWave();
        enemyMedium.DestroyTooSmallWave();
    }

}
