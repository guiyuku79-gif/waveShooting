using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class LaneGenerator : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject lineSample;

    private List<GameObject> lanes = new List<GameObject>();
    void Start()
    {
        foreach (float laneY in Constants.laneYs)
        {
            GameObject gameObject1 = Instantiate(lineSample);
            gameObject1.GetComponent<LineSample>().Init(laneY, player);
            lanes.Add(gameObject1);
        }
    }

    void Update()
    {
        //仮
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            lanes[1].GetComponent<LineSample>().enemyMedium.MakeSinWave(4, 2, 1.5f);
        }
    }
}
