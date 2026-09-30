using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameStateName
    {
        Play,
        GameOver,
        GameClear
    }

    private int enemyCount;

    public GameStateName GameState { get; private set; }

    [SerializeField] MotherShipController motherShipController;
    [SerializeField] GameObject GameOverText;

    public static GameStateManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        enemyCount = GetComponent<EnemyWaveSet>().CountSpawn();
    }

    void Start()
    {
        GameState = GameStateName.Play;
        motherShipController.MotherShipDestroyed += GameOver;
        GameOverText.SetActive(false);

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


    void GameOver()
    {
        GameOverText.SetActive(true);
    }

    private void OnDestroy()
    {
        motherShipController.MotherShipDestroyed -= GameOver;
    }
}
