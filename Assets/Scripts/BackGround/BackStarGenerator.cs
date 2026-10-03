using UnityEngine;

public class BackStarGenerator : MonoBehaviour
{
    [SerializeField] Vector2 screenSize;
    [SerializeField] GameObject starPrefab;

    [SerializeField] float moveInterval;

    float deltaTimeCount;
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject obj = Instantiate(starPrefab);
            obj.transform.position = new Vector3(Random.Range(-screenSize.x / 2, screenSize.x / 2),
                                                Random.Range(-screenSize.y / 2, screenSize.y / 2),
                                                0);
            obj.GetComponent<BackStarController>().Init(Random.Range(0.1f, 0.3f));
        }

    }

    void Update()
    {
        deltaTimeCount += Time.deltaTime;
        if (deltaTimeCount >= moveInterval)
        {
            GameObject obj = Instantiate(starPrefab);
            obj.transform.position = new Vector3(screenSize.x / 2,
                                                Random.Range(-screenSize.y / 2, screenSize.y / 2),
                                                0);
            obj.GetComponent<BackStarController>().Init(Random.Range(0.1f, 0.3f));
            deltaTimeCount = 0;
        }
    }
}
