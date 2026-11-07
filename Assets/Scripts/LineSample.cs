using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class LineSample : MonoBehaviour
{
    GameObject player;

    [SerializeField] GameObject waveRendererPrefab;

    private float originalMoveInterval;

    private float moveInterval;
    private float deltaTimeCount;


    private WaveViewCollection playerWaveViews;
    private WaveViewCollection enemyWaveViews;

    private LaneDamageResolver damageResolver;


    [NonSerialized] public PlayerMedium playerMedium;
    [NonSerialized] public EnemyMedium enemyMedium;

    public event Action WaveMoved;

    public List<EnemyController> enemies = new();


    public void Init(float laneY, GameObject player, MotherShipController motherShip)
    {
        this.player = player;
        playerMedium = new PlayerMedium(new Color32(255, 0, 0, 122), laneY);
        enemyMedium = new EnemyMedium(new Color32(0, 255, 0, 122), laneY);

        playerWaveViews = new WaveViewCollection(
            waveRendererPrefab,
            transform,
            playerMedium,
            enemyMedium
        );

        enemyWaveViews = new WaveViewCollection(
            waveRendererPrefab,
            transform,
            enemyMedium,
            playerMedium
        );

        damageResolver = new LaneDamageResolver(
            enemyMedium,
            player.GetComponent<PlayerController>(),
            motherShip
        );

        deltaTimeCount = 0f;

        originalMoveInterval = 1 / (Constants.Division / Constants.Width * Constants.WaveSpeed);
        moveInterval = originalMoveInterval;

        transform.position = new Vector3(0, laneY, 0);
    }

    void Update()
    {
        if (!GameStateManager.Instance.IsPlaying)
        {
            return;
        }
        deltaTimeCount += Time.deltaTime;
        if (deltaTimeCount >= moveInterval)
        {
            deltaTimeCount = 0;
            MoveWave();

            //moveInterval = IntervalChange();

            WavePowerCheck();

            playerWaveViews.RemoveMissingViews();
            enemyWaveViews.RemoveMissingViews();

            List<int> newIds = playerMedium.ReassignSeparatedWaveId();
            playerWaveViews.CreateViews(newIds);
            List<int> newIds2 = enemyMedium.ReassignSeparatedWaveId(reverse: true);
            enemyWaveViews.CreateViews(newIds2);

            damageResolver.ApplyPlayerDamage();
            damageResolver.ApplyMotherShipDamage();

            if (!GameStateManager.Instance.IsPlaying)
            {
                return;
            }

            WaveMoved?.Invoke();
        }


    }

    void OnGUI()
    {

        string debugText = "";

        foreach (var (id, ratio) in playerMedium.previousWaveSigns)
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
        //プレイヤーの動き
        List<int> newIds;
        if (playerMedium.laneY - Constants.LaneHeight / 2 <= player.transform.position.y
            && playerMedium.laneY + Constants.LaneHeight / 2 >= player.transform.position.y
            && player.GetComponent<PlayerController>().CanMove)
        {
            newIds = playerMedium.WaveMove(Mouse.current.leftButton.isPressed, player.transform.position.y - playerMedium.laneY);
            playerWaveViews.CreateViews(newIds);
        }
        else
        {
            newIds = playerMedium.WaveMove(false, 0);
            playerWaveViews.CreateViews(newIds);
        }

        newIds.Clear();
        List<(int id, float displacement, int x)> newWaveSources = new();

        foreach (EnemyController enemy in enemies)
        {
            var waveData = enemy.WaveMove();
            newIds.AddRange(waveData.newIds);
            newWaveSources.Add((waveData.id, waveData.displacement, waveData.x));
        }

        enemyMedium.Move(newWaveSources);
        enemyWaveViews.CreateViews(newIds);

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
        player.GetComponent<PlayerController>().point += playerMedium.WavePowerCheck(enemyMedium);
        player.GetComponent<PlayerController>().point += enemyMedium.WavePowerCheck(playerMedium);

        playerMedium.DestroyTooSmallWave(new List<int> { Constants.PlayerDivisionX });
        List<int> exceptPositions = new();
        foreach (EnemyController enemy in enemies)
        {
            exceptPositions.Add(enemy.positionX);
        }
        enemyMedium.DestroyTooSmallWave(exceptPositions);
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }
}
