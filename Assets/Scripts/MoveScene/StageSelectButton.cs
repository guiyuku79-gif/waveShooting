using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;

    public void Init(int stageId, Action<int> onSelected)
    {
        label.text = stageId.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onSelected(stageId));
    }
}