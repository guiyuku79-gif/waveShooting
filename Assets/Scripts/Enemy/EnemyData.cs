using System.Collections.Generic;
using UnityEngine;

public enum EnemyActionType
{
    Sin,
    Triangle,
    Constant,
    Wait,
    Move
}

[System.Serializable]
public class EnemyAction
{
    public EnemyActionType type;

    public float waveLength = 1f;
    public float amplitude = 0f;
    public float waveCount = 0f;
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHp = 5f;
    public List<EnemyAction> actions = new(); //手動で追加

    public List<(Constants.EnemyAction action, float displacement)>
    CreateWaveList()
    {
        var waves = new List<(
            Constants.EnemyAction action,
            float displacement
        )>();

        float samplesPerUnit = Constants.Division / Constants.Width;

        foreach (var step in actions)
        {
            if (step.type == EnemyActionType.Wait)
            {
                for (int i = 0; i < samplesPerUnit * step.waveLength; i++)
                    waves.Add((Constants.EnemyAction.Wait, 0));

                continue;
            }

            if (step.type == EnemyActionType.Move)
            {
                for (int i = 0; i < samplesPerUnit * step.waveLength; i++)
                    waves.Add((Constants.EnemyAction.Move, step.waveCount));
                continue;
            }

            float length = step.type == EnemyActionType.Constant
                ? step.waveLength
                : step.waveLength * step.waveCount;

            // WaveStartの直後には必ず波のデータが必要
            if (step.waveLength <= 0f || length <= 0f)
                continue;

            waves.Add((Constants.EnemyAction.WaveStart, 0f));

            for (int i = 0; i < samplesPerUnit * length; i++)
            {
                float phase = i / samplesPerUnit / step.waveLength;

                float displacement = step.type switch
                {
                    EnemyActionType.Sin =>
                        Mathf.Sin(2f * Mathf.PI * phase) * step.amplitude,

                    EnemyActionType.Triangle =>
                        TriangleDef(4f * phase) * step.amplitude,

                    EnemyActionType.Constant => step.amplitude,

                    _ => 0f
                };

                waves.Add((Constants.EnemyAction.Wave, displacement));
            }
        }

        return waves;
    }

    private static float TriangleDef(float x)
    {
        x %= 4f;

        if (x <= 1f) return x;
        if (x <= 3f) return 2f - x;
        return x - 4f;
    }
}