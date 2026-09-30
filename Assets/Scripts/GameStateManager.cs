using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameStateName
    {
        Play,
        GameOver,
        GameClear
    }

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
    }

    void Start()
    {
        GameState = GameStateName.Play;
        motherShipController.MotherShipDestroyed += GameOver;
        GameOverText.SetActive(false);

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
