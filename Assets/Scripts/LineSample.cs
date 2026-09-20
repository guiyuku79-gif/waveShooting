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

    private Dictionary<int, GameObject> playerWaves = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> enemyWaves = new Dictionary<int, GameObject>();


    [NonSerialized] public PlayerMedium playerMedium;
    [NonSerialized] public EnemyMedium enemyMedium;


    void Start()
    {
        playerMedium = new PlayerMedium();
        enemyMedium = new EnemyMedium();

        deltaTimeCount = 0f;

        originalMoveInterval = 1 / (Constants.Division / Constants.Width * Constants.WaveSpeed);


    }

    void Update()
    {
        deltaTimeCount += Time.deltaTime;
        if (deltaTimeCount >= moveInterval)
        {
            deltaTimeCount = 0;
            MoveWave();

            moveInterval = IntervalChange();

            WavePowerCheck();

            DestoryOutOfWave(playerMedium, playerWaves);
            DestoryOutOfWave(enemyMedium, enemyWaves);

            playerMedium.ReNewSeparetedWaveId();
            enemyMedium.ReNewSeparetedWaveId();
        }

        //仮
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            enemyMedium.MakeSinWave(4, 2, 1.5f);
        }
    }

    private void MoveWave()
    {
        List<int> newIds = playerMedium.WaveMove(Mouse.current.leftButton.isPressed, Player.transform.position.y);
        MakeNewWave(newIds, playerMedium, playerWaves);

        newIds = enemyMedium.WaveMove();
        MakeNewWave(newIds, enemyMedium, enemyWaves);

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

    private void MakeNewWave(List<int> ids, Medium medium, Dictionary<int, GameObject> waves)
    {
        foreach (int id in ids)
        {
            GameObject gameObject = Instantiate(waveLendererObject);

            gameObject.transform.SetParent(transform);
            gameObject.GetComponent<OneWaveLenderer>().Init(medium, id);

            waves.Add(id, gameObject);
        }
    }

    private void DestoryOutOfWave(Medium medium, Dictionary<int, GameObject> waves)
    {
        List<int> keysToRemove = new List<int>();
        foreach (KeyValuePair<int, GameObject> pair in waves)
        {
            if (medium.waveIds.IndexOf(pair.Key) == -1)
            {
                Destroy(pair.Value);
                keysToRemove.Add(pair.Key);

            }
        }

        foreach (var key in keysToRemove)
        {
            waves.Remove(key);
        }
    }

}
