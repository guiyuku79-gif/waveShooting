using System.Collections.Generic;

public sealed class WaveSimulation
{
    private readonly PlayerMedium playerMedium;
    private readonly EnemyMedium enemyMedium;

    public WaveSimulation(
        PlayerMedium playerMedium,
        EnemyMedium enemyMedium)
    {
        this.playerMedium = playerMedium;
        this.enemyMedium = enemyMedium;
    }

    public float ResolveInterference(List<int> enemyPositions)
    {
        float earnedPoints = playerMedium.WavePowerCheck(enemyMedium);
        earnedPoints += enemyMedium.WavePowerCheck(playerMedium);

        playerMedium.DestroyTooSmallWave(new List<int> { Constants.PlayerDivisionX });

        enemyMedium.DestroyTooSmallWave(enemyPositions);

        return earnedPoints;
    }

    public (List<int> playerIds, List<int> enemyIds) ReassignSeparatedWaves()
    {
        List<int> playerIds = playerMedium.ReassignSeparatedWaveId();

        List<int> enemyIds = enemyMedium.ReassignSeparatedWaveId(reverse: true);

        return (playerIds, enemyIds);
    }
}