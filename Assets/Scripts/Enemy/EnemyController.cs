using UnityEngine;

public class EnemyController : MonoBehaviour
{

    LineSample lineSample;
    float hp;

    public void Init(LineSample lineSample, int lane)
    {
        this.lineSample = lineSample;
        hp = 10;
        transform.position = new Vector3(Constants.Width / 2, Constants.laneYs[lane], 0);

        this.lineSample.WaveMoved += OnWaveMoved;
    }
    private void OnWaveMoved()
    {
        if (lineSample.playerMedium.waveIds[^1] != 0)
        {
            hp -= Mathf.Abs(lineSample.playerMedium.wavePowers[^1]);
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (lineSample != null)
            lineSample.WaveMoved -= OnWaveMoved;
    }
}
