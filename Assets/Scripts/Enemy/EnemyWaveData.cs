using System.Collections.Generic;

public class OneEnemyWaveData
{
    public int id;
    public List<WaveSetAction> waveSetActions;
    public OneEnemyWaveData(int id, List<WaveSetAction> waveSetActions)
    {
        this.id = id;
        this.waveSetActions = waveSetActions;
    }
};

public static class EnemyWaveData
{
    public static List<OneEnemyWaveData> EnemyWaveDataList = new List<OneEnemyWaveData>
    {
        new OneEnemyWaveData(1,
        new List<WaveSetAction>
        {
            new WaveSetAction(type:WaveSetActionType.Spawn,enemyId:0,laneYId:1),
            new WaveSetAction(type: WaveSetActionType.Wait, waitTime: 5f),
            new WaveSetAction(type:WaveSetActionType.Spawn,enemyId:0,laneYId:1),
        }),
        new OneEnemyWaveData(2,
        new List<WaveSetAction>
        {
            new WaveSetAction(type:WaveSetActionType.Spawn,enemyId:0,laneYId:1),
            new WaveSetAction(type:WaveSetActionType.Spawn,enemyId:1,laneYId:0),
            new WaveSetAction(type:WaveSetActionType.Spawn,enemyId:0,laneYId:2),
        }),
        new OneEnemyWaveData(3,
        new List<WaveSetAction>
        {
            new WaveSetAction(type:WaveSetActionType.Spawn,enemyId:0,laneYId:1),
            new WaveSetAction(type:WaveSetActionType.Wait,waitTime: 5f),
            new WaveSetAction(type:WaveSetActionType.Spawn,enemyId:2,laneYId:1),
        }),


    };
}
