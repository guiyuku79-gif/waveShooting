using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private StageSelectButton stageButtonPrefab;
    [SerializeField] private Transform buttonParent;
    [SerializeField, Min(1)] private int stageCount = 5;

    private void Start()
    {
        for (int stageId = 1; stageId <= stageCount; stageId++)
        {
            StageSelectButton button =
                Instantiate(stageButtonPrefab, buttonParent);

            button.Init(stageId, SelectStage);
        }
    }

    private void SelectStage(int stageId)
    {
        StageSelection.SelectedStageId = stageId;
        SceneManager.LoadScene("GameScene");
    }
}