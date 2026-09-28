using UnityEngine;

public class ClearManager : MonoBehaviour
{
    private int enemyCount;

    private void Awake()
    {
        enemyCount = GetComponent<EnemyWaveSet>().CountSpawn();
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemy.Defeated += OnEnemyDefeated;
    }

    private void OnEnemyDefeated()
    {
        enemyCount--;

        Debug.Log($"残りの敵: {enemyCount}");

        if (enemyCount == 0)
        {
            Debug.Log("ゲームクリア！");
            // クリア画面の表示など
        }
    }
}