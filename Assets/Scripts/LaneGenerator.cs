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
        GameObject gameObject1 = Instantiate(lineSample);
        gameObject1.GetComponent<LineSample>().Init(4.5f, player);
        lanes.Add(gameObject1);

        GameObject gameObject2 = Instantiate(lineSample);
        gameObject2.GetComponent<LineSample>().Init(0f, player);
        lanes.Add(gameObject2);

        GameObject gameObject3 = Instantiate(lineSample);
        gameObject3.GetComponent<LineSample>().Init(-4.5f, player);
        lanes.Add(gameObject3);

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
