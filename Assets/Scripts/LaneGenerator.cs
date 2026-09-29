using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class LaneGenerator : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] MotherShipController motherShip;
    [SerializeField] GameObject lineSample;

    public List<GameObject> lanes = new List<GameObject>();

    //シングルトン
    public static LaneGenerator Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (float laneY in Constants.laneYs)
        {
            GameObject gameObject1 = Instantiate(lineSample);
            gameObject1.GetComponent<LineSample>().Init(laneY, player, motherShip);
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
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            lanes[1].GetComponent<LineSample>().enemyMedium.MakeTriangleWave(2, 1, 0.5f);
        }
    }
}
