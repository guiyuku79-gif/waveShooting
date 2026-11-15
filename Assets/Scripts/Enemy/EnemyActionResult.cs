using NUnit.Framework;

public readonly struct EnemyActionResult
{
    public bool HasWave { get; }
    public bool IsNewWave { get; }
    public int WaveId { get; }
    public float Displacement { get; }
    public int PositionX { get; }

    private EnemyActionResult(bool hasWave, bool isNewWave, int waveId, float displacement, int positionX)
    {
        HasWave = hasWave;
        IsNewWave = isNewWave;
        WaveId = waveId;
        Displacement = displacement;
        PositionX = positionX;
    }

    public static EnemyActionResult NoWave(int positionX)
    {
        return new EnemyActionResult(
            hasWave: false,
            isNewWave: false,
            waveId: 0,
            displacement: 0f,
            positionX: positionX
        );
    }

    public static EnemyActionResult Emit(
        int waveId,
        float displacement,
        int positionX,
        bool isNewWave)
    {
        return new EnemyActionResult(
            hasWave: true,
            isNewWave: isNewWave,
            waveId: waveId,
            displacement: displacement,
            positionX: positionX);
    }
}