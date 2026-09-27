using UnityEngine;

public class EnemyWaveSet : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    void Start()
    {
        GameObject gameObject = Instantiate(enemy);
        gameObject.GetComponent<EnemyController>().Init(LaneGenerator.Instance.lanes[1].GetComponent<LineSample>());
    }
}