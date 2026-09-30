using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStageSelectButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
