using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class LineSample : MonoBehaviour
{
    GameObject Player;

    [SerializeField] GameObject waveLendererObject;

    private float originalMoveInterval;

    private float moveInterval;
    private float deltaTimeCount;


    private Dictionary<int, GameObject> playerWaves = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> enemyWaves = new Dictionary<int, GameObject>();


    [NonSerialized] public PlayerMedium playerMedium;
    [NonSerialized] public EnemyMedium enemyMedium;

    public event Action WaveMoved;


    public void Init(float laneY, GameObject player)
    {
        this.Player = player;
        playerMedium = new PlayerMedium(new Color32(255, 0, 0, 122), laneY);
        enemyMedium = new EnemyMedium(new Color32(0, 255, 0, 122), laneY);

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

            List<int> newIds = playerMedium.ReNewSeparetedWaveId();
            MakeNewWave(newIds, playerMedium, enemyMedium, playerWaves);
            List<int> newIds2 = enemyMedium.ReNewSeparetedWaveId(reverse: true);
            MakeNewWave(newIds2, enemyMedium, playerMedium, enemyWaves);

            PlayerDamage();

            WaveMoved?.Invoke();
        }


    }

    void OnGUI()
    {

        string debugText = "";

        foreach (var (id, ratio) in playerMedium.priviousWavePowers)
        {
            debugText += $"ID: {id}, Name: {ratio}\n";
        }
        GUI.Label(
            new Rect(10, 10, 300, 100),
            debugText
        );
    }

    private void MoveWave()
    {
        List<int> newIds;
        if (playerMedium.laneY - Constants.LaneHeight / 2 <= Player.transform.position.y
            && playerMedium.laneY + Constants.LaneHeight / 2 >= Player.transform.position.y
            && Player.GetComponent<PlayerController>().fuelRate >= 0.01f
            && Player.GetComponent<PlayerController>().canMove)
        {
            newIds = playerMedium.WaveMove(Mouse.current.leftButton.isPressed, Player.transform.position.y - playerMedium.laneY);
            MakeNewWave(newIds, playerMedium, enemyMedium, playerWaves);
        }
        else
        {
            newIds = playerMedium.WaveMove(false, 0);
            MakeNewWave(newIds, playerMedium, enemyMedium, playerWaves);
        }


        newIds = enemyMedium.WaveMove();
        MakeNewWave(newIds, enemyMedium, playerMedium, enemyWaves);

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
        Player.GetComponent<PlayerController>().point += playerMedium.WavePowerCheck(enemyMedium);
        Player.GetComponent<PlayerController>().point += enemyMedium.WavePowerCheck(playerMedium);

        playerMedium.DestroyTooSmallWave();
        enemyMedium.DestroyTooSmallWave();
    }

    private void MakeNewWave(List<int> ids, Medium medium, Medium oppositeMedium, Dictionary<int, GameObject> waves)
    {
        foreach (int id in ids)
        {
            if (waves.ContainsKey(id)) return;
            GameObject gameObject = Instantiate(waveLendererObject);

            gameObject.transform.SetParent(transform);
            gameObject.GetComponent<OneWaveLenderer>().Init(medium, oppositeMedium, id);
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
                //出している最中の波は消さない
                if (medium is PlayerMedium && pair.Key == medium.nextWaveId) continue;
                if (medium is EnemyMedium && medium.nextEnemyWaveList.Count != 0 && pair.Key == medium.nextEnemyWaveList[0].id) continue;

                Destroy(pair.Value);
                keysToRemove.Add(pair.Key);

            }
        }

        foreach (var key in keysToRemove)
        {
            waves.Remove(key);
        }
    }

    private void PlayerDamage()
    {
        if (playerMedium.laneY - Constants.LaneHeight / 2 <= Player.transform.position.y &&
            Player.transform.position.y <= playerMedium.laneY + Constants.LaneHeight / 2)
        {
            if (enemyMedium.waveIds[0] != 0)
            {
                Player.GetComponent<PlayerController>().Damage(enemyMedium.wavePowers[0] * Constants.Width / Constants.Division);
            }
        }
    }
}
