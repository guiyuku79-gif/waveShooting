using System.Collections.Generic;
using UnityEngine;
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

}
