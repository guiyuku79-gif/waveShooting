using UnityEngine;

public sealed class LaneDamageResolver
{
    private readonly EnemyMedium enemyMedium;
    private readonly PlayerController player;
    private readonly MotherShipController motherShip;

    public LaneDamageResolver(
        EnemyMedium enemyMedium,
        PlayerController player,
        MotherShipController motherShip
    )
    {
        this.enemyMedium = enemyMedium;
        this.player = player;
        this.motherShip = motherShip;
    }

    public void ApplyPlayerDamage()
    {
        float playerY = player.transform.position.y;
        float halfLaneHeight = Constants.LaneHeight / 2f;

        if (playerY < enemyMedium.laneY - halfLaneHeight || playerY > enemyMedium.laneY + halfLaneHeight)
        {
            return;
        }

        int index = Constants.PlayerDivisionX;

        if (enemyMedium.waveIds[index] == 0) return;

        float damage = CalculateDamage(index);
        player.Damage(damage);
        enemyMedium.DestroyOneDivision(index);
    }

    public void ApplyMotherShipDamage()
    {
        const int index = 0;

        if (enemyMedium.waveIds[index] == 0) return;

        float damage = CalculateDamage(index);

        motherShip.Damage(damage);
        enemyMedium.DestroyOneDivision(index);
    }

    private float CalculateDamage(int index)
    {
        float wavePower = Mathf.Abs(enemyMedium.wavePowers[index]);
        float divisionWidth = Constants.Width / Constants.Division;

        return wavePower * divisionWidth;
    }
}