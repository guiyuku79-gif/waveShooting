using System;
using Unity.Mathematics;

public sealed class EnemyHealth
{
    private const float SignChangeDamage = 0.5f;
    private const float SignChangeThreshold = 1f;

    private readonly float damagePerSample;
    private int previousWaveSign;

    public float MaxHp { get; }
    public float CurrentHp { get; private set; }

    public float HpRatio => CurrentHp / MaxHp;
    public bool IsDead => CurrentHp <= 0f;

    public EnemyHealth(float maxHp, float damagePerSample)
    {
        MaxHp = maxHp;
        CurrentHp = maxHp;
        this.damagePerSample = damagePerSample;
    }
    public void ApplyWave(float wavePower)
    {
        if (IsDead || wavePower == 0f)
        {
            return;
        }

        float damage = damagePerSample;

        if (previousWaveSign == 0)
        {
            damage += SignChangeDamage;
            previousWaveSign = wavePower > 0f ? 1 : -1;
        }
        else if (
            wavePower > SignChangeThreshold &&
            previousWaveSign == -1)
        {
            damage += SignChangeDamage;
            previousWaveSign = 1;
        }
        else if (
            wavePower < -SignChangeThreshold &&
            previousWaveSign == 1)
        {
            damage += SignChangeDamage;
            previousWaveSign = -1;
        }

        CurrentHp = Math.Max(0f, CurrentHp - damage);
    }
}