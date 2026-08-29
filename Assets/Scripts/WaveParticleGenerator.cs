using UnityEngine;

public class WaveParticleGenerator : MonoBehaviour
{
    [SerializeField] GameObject ParticlePrefab;
    void Start()
    {
        for (int i = 0; i < LineSample.Instance.division; i++)
        {
            GameObject gameobject = Instantiate(ParticlePrefab);
            gameobject.GetComponent<WaveParticleController>().Init(i);

        }
    }

}
