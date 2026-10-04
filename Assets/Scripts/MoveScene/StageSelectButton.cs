using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;

    [SerializeField] List<SpriteRenderer> stars;


    public void Init(int stageId, Action<int> onSelected, int stageAcheivement)
    {
        label.text = stageId.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onSelected(stageId));

        for (int i = 0; i < stageAcheivement; i++)
        {
            stars[i].color = Color.white;
        }
    }
}