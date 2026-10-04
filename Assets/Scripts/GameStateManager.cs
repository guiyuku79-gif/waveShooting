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
    [SerializeField] GameObject GameClearText;

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
        GameClearText.SetActive(false);


    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemy.Defeated += OnEnemyDefeated;
    }
    private void OnEnemyDefeated()
    {
        enemyCount--;

        Debug.Log($"残りの敵: {enemyCount}");

        if (enemyCount == 0 && GameState == GameStateName.Play)
        {
            Debug.Log("ゲームクリア！");
            GameState = GameStateName.GameClear;
            GameClearText.SetActive(true);

            //仮
            StageAcheivement.progress[StageSelection.SelectedStageId] = 3;
        }
    }


    void GameOver()
    {
        if (GameState == GameStateName.Play)
        {
            GameState = GameStateName.GameOver;
            GameOverText.SetActive(true);
        }

    }

    private void OnDestroy()
    {
        motherShipController.MotherShipDestroyed -= GameOver;
    }
}
