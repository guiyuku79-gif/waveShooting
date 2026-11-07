using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameStateName
    {
        Play,
        GameOver,
        GameClear,
        Preparing
    }

    private int enemyCount;

    public GameStateName GameState { get; private set; } = GameStateName.Preparing;

    public bool IsPlaying => GameState == GameStateName.Play;
    private Coroutine spawnCoroutine;

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
    }

    void Start()
    {
        GameOverText.SetActive(false);
        GameClearText.SetActive(false);

        EnemyWaveSet enemyWaveSet = GetComponent<EnemyWaveSet>();

        int stageId = StageSelection.SelectedStageId;

        if (!enemyWaveSet.Initialize(stageId))
        {
            return;
        }

        //EnemyWaveSetが準備し終わった後に敵の数を設定
        enemyCount = enemyWaveSet.CountSpawn();

        GameState = GameStateName.Play;
        motherShipController.MotherShipDestroyed += GameOver;

        spawnCoroutine = StartCoroutine(enemyWaveSet.SpawnEnemies());

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
            EndGame(GameStateName.GameClear);
        }
    }


    void GameOver()
    {
        EndGame(GameStateName.GameOver);
    }

    private void EndGame(GameStateName result)
    {
        if (!IsPlaying) return;

        GameState = result;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        GameOverText.SetActive(result == GameStateName.GameOver);
        GameClearText.SetActive(result == GameStateName.GameClear);

        if (result == GameStateName.GameClear)
        {
            StageAcheivement.progress[StageSelection.SelectedStageId] = 3;
        }
    }

    private void OnDestroy()
    {
        motherShipController.MotherShipDestroyed -= GameOver;
    }
}
