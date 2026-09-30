using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEngine;

public enum WaveSetActionType
{
    Spawn,
    Wait,
}

[System.Serializable]
public class WaveSetAction //敵を生成する、待機するなどの行動1つ
{
    public WaveSetActionType type;
    public int enemyId;
    public float waitTime;

    public int laneYId;

    public WaveSetAction(
        WaveSetActionType type,
        int enemyId = 0,
        float waitTime = 0f,
        int laneYId = 1)
    {
        this.type = type;
        this.enemyId = enemyId;
        this.waitTime = waitTime;
        this.laneYId = laneYId;
    }
}

public class EnemyWaveSet : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    [SerializeField] List<EnemyData> data1; //敵の種類


    List<WaveSetAction> waveSetActions = new List<WaveSetAction> { };

    void Awake()
    {
        waveSetActions = EnemyWaveData.EnemyWaveDataList.FirstOrDefault(x => x.id == StageSelection.SelectedStageId).waveSetActions;
    }

    IEnumerator Start()
    {
        foreach (var action in waveSetActions)
        {
            switch (action.type)
            {
                case WaveSetActionType.Spawn:
                    {
                        GameObject obj = Instantiate(enemy);

                        EnemyController controller = obj.GetComponent<EnemyController>();

                        GetComponent<GameStateManager>().RegisterEnemy(controller);

                        LineSample lane = LaneGenerator.Instance
                            .lanes[action.laneYId].GetComponent<LineSample>();

                        controller.Init(lane, data1[action.enemyId]);

                        break;
                    }

                case WaveSetActionType.Wait:
                    yield return new WaitForSeconds(action.waitTime);
                    break;
            }
        }
    }

    public int CountSpawn()
    {
        int sum = 0;
        foreach (var action in waveSetActions)
        {
            if (action.type == WaveSetActionType.Spawn) sum++;
        }
        return sum;
    }
}