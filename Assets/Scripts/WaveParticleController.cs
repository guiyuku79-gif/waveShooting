using UnityEngine;

public class WaveParticleController : MonoBehaviour
{
    [SerializeField] public float particleInterval = 0.1f;
    [SerializeField] SpriteRenderer spriteRenderer;

    private int id;
    private string charge;
    public void Init(int id, string charge)
    {
        this.id = id;
        this.charge = charge;
        if (this.charge == "player")
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.yellow;
        }
        transform.position = new Vector3((id - LineSample.Instance.division / 2) * particleInterval, 0, 0);
    }

    void Update()
    {
        if (charge == "player")
        {
            if (LineSample.Instance.PlayerWaveIds[id] == 0)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;

                if (LineSample.Instance.EnemyWaveIds[id] == 0) transform.position = new Vector3(transform.position.x, LineSample.Instance.PlayerWavePowers[id], 0);
                else transform.position = new Vector3(transform.position.x, LineSample.Instance.PlayerWavePowers[id] + LineSample.Instance.EnemyWavePowers[id], 0);
            }
        }
        else
        {
            if (LineSample.Instance.EnemyWaveIds[id] == 0)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;

                if (LineSample.Instance.EnemyWaveIds[id] == 0) transform.position = new Vector3(transform.position.x, LineSample.Instance.EnemyWavePowers[id], 0);
                else transform.position = new Vector3(transform.position.x, LineSample.Instance.PlayerWavePowers[id] + LineSample.Instance.EnemyWavePowers[id], 0);
            }
        }

    }



}
