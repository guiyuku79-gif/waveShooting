using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class LineSample : MonoBehaviour
{
    GameObject player;
    private MotherShipController motherShip;

    [SerializeField] GameObject waveRendererPrefab;

    private float originalMoveInterval;

    private float moveInterval;
    private float deltaTimeCount;


    private Dictionary<int, GameObject> playerWaves = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> enemyWaves = new Dictionary<int, GameObject>();


    [NonSerialized] public PlayerMedium playerMedium;
    [NonSerialized] public EnemyMedium enemyMedium;

    public event Action WaveMoved;

    public List<EnemyController> enemies = new();


    public void Init(float laneY, GameObject player, MotherShipController motherShip)
    {
        this.motherShip = motherShip;
        this.player = player;
        playerMedium = new PlayerMedium(new Color32(255, 0, 0, 122), laneY);
        enemyMedium = new EnemyMedium(new Color32(0, 255, 0, 122), laneY);

        deltaTimeCount = 0f;

        originalMoveInterval = 1 / (Constants.Division / Constants.Width * Constants.WaveSpeed);
        moveInterval = originalMoveInterval;

        transform.position = new Vector3(0, laneY, 0);
    }

    void Update()
    {
        deltaTimeCount += Time.deltaTime;
        if (deltaTimeCount >= moveInterval)
        {
            deltaTimeCount = 0;
            MoveWave();

            //moveInterval = IntervalChange();

            WavePowerCheck();

            DestroyOutOfWave(playerMedium, playerWaves);
            DestroyOutOfWave(enemyMedium, enemyWaves);

            List<int> newIds = playerMedium.ReassignSeparatedWaveId();
            MakeNewWave(newIds, playerMedium, enemyMedium, playerWaves);
            List<int> newIds2 = enemyMedium.ReassignSeparatedWaveId(reverse: true);
            MakeNewWave(newIds2, enemyMedium, playerMedium, enemyWaves);

            PlayerDamage();
            MotherShipDamage();

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
        List<int> newIds;
        if (playerMedium.laneY - Constants.LaneHeight / 2 <= player.transform.position.y
            && playerMedium.laneY + Constants.LaneHeight / 2 >= player.transform.position.y
            && player.GetComponent<PlayerController>().canMove)
        {
            newIds = playerMedium.WaveMove(Mouse.current.leftButton.isPressed, player.transform.position.y - playerMedium.laneY);
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
        player.GetComponent<PlayerController>().point += playerMedium.WavePowerCheck(enemyMedium);
        player.GetComponent<PlayerController>().point += enemyMedium.WavePowerCheck(playerMedium);

        playerMedium.DestroyTooSmallWave(new List<int> { Constants.PlayerDivisionX });
        enemyMedium.DestroyTooSmallWave(new List<int> { });
    }

    private void MakeNewWave(List<int> ids, Medium medium, Medium oppositeMedium, Dictionary<int, GameObject> waves)
    {
        foreach (int id in ids)
        {
            if (waves.ContainsKey(id)) continue;
            GameObject gameObject = Instantiate(waveRendererPrefab);

            gameObject.transform.SetParent(transform);
            gameObject.GetComponent<OneWaveLenderer>().Init(medium, oppositeMedium, id);
            waves.Add(id, gameObject);

        }
    }

    private void DestroyOutOfWave(Medium medium, Dictionary<int, GameObject> waves)
    {
        List<int> keysToRemove = new List<int>();
        foreach (KeyValuePair<int, GameObject> pair in waves)
        {
            if (medium.waveIds.IndexOf(pair.Key) == -1)
            {
                //出している最中の波は消さない
                if (pair.Key == medium.nextWaveId) continue;

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
        if (playerMedium.laneY - Constants.LaneHeight / 2 <= player.transform.position.y &&
            player.transform.position.y <= playerMedium.laneY + Constants.LaneHeight / 2)
        {
            if (enemyMedium.waveIds[Constants.PlayerDivisionX] != 0)
            {
                player.GetComponent<PlayerController>().Damage(Mathf.Abs(enemyMedium.wavePowers[Constants.PlayerDivisionX]) * Constants.Width / Constants.Division);
                enemyMedium.DestroyOneDivision(Constants.PlayerDivisionX);
            }
        }
    }

    private void MotherShipDamage()
    {
        if (enemyMedium.waveIds[0] == 0) return;

        float damage = Mathf.Abs(enemyMedium.wavePowers[0]) * Constants.Width / Constants.Division;

        motherShip.Damage(damage);
        enemyMedium.DestroyOneDivision(0);
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }
}
