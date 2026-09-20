using UnityEngine;

public class WaveParticleGenerator : MonoBehaviour
{
    [SerializeField] GameObject ParticlePrefab;
    [SerializeField] LineSample lineSample;

    void Start()
    {
        for (int i = 0; i < Constants.Division; i++)
        {
            GameObject gameobject = Instantiate(ParticlePrefab);
            gameobject.GetComponent<WaveParticleController>().Init(i, "player", lineSample.playerMedium, lineSample.enemyMedium);
            GameObject gameobject2 = Instantiate(ParticlePrefab);
            gameobject2.GetComponent<WaveParticleController>().Init(i, "enemy", lineSample.playerMedium, lineSample.enemyMedium);

        }
    }

}
