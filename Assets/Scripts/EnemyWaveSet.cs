using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public enum WaveSetActionType
{
    Spawn,
    Wait,
}

[System.Serializable]
public class WaveSetAction
{
    public WaveSetActionType type;
    public int waveId;
    public float waitTime;

    public int laneYId;

    public WaveSetAction(
        WaveSetActionType type,
        int waveId = 0,
        float waitTime = 0f,
        int laneYId = 1)
    {
        this.type = type;
        this.waveId = waveId;
        this.waitTime = waitTime;
        this.laneYId = laneYId;
    }
}

public class EnemyWaveSet : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    [SerializeField] List<EnemyData> data1; //敵の種類


    List<WaveSetAction> waveSetActions = new List<WaveSetAction>
    {
        new WaveSetAction(type:WaveSetActionType.Spawn,waveId:0,laneYId:1),
        new WaveSetAction(type: WaveSetActionType.Wait, waitTime: 5f),
        new WaveSetAction(type:WaveSetActionType.Spawn,waveId:0,laneYId:0),
    };

    IEnumerator Start()
    {
        foreach (var action in waveSetActions)
        {
            switch (action.type)
            {
                case WaveSetActionType.Spawn:
                    {
                        GameObject obj = Instantiate(enemy);

                        LineSample lane = LaneGenerator.Instance
                            .lanes[action.laneYId].GetComponent<LineSample>();

                        obj.GetComponent<EnemyController>()
                            .Init(lane, data1[action.waveId]);

                        break;
                    }

                case WaveSetActionType.Wait:
                    yield return new WaitForSeconds(action.waitTime);
                    break;
            }
        }
    }
}