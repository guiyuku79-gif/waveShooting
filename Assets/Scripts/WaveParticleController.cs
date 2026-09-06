using UnityEngine;

public class WaveParticleController : MonoBehaviour
{
    [SerializeField] public float particleInterval = 0.1f;
    [SerializeField] SpriteRenderer spriteRenderer;

    private int id;
    private string charge;

    private Medium playerMedium;
    private Medium enemyMedium;
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

        playerMedium = LineSample.Instance.PlayerMedium;
        enemyMedium = LineSample.Instance.EnemyMedium;
        transform.position = new Vector3((id - LineSample.Instance.division / 2) * particleInterval, 0, 0);
    }

    void Update()
    {
        if (charge == "player")
        {
            if (playerMedium.waveIds[id] == 0)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;

                if (enemyMedium.waveIds[id] == 0) transform.position = new Vector3(transform.position.x, playerMedium.wavePowers[id], 0);
                else transform.position = new Vector3(transform.position.x, playerMedium.wavePowers[id] + enemyMedium.wavePowers[id], 0);
            }
        }
        else
        {
            if (enemyMedium.waveIds[id] == 0)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;

                if (playerMedium.waveIds[id] == 0) transform.position = new Vector3(transform.position.x, enemyMedium.wavePowers[id], 0);
                else transform.position = new Vector3(transform.position.x, playerMedium.wavePowers[id] + enemyMedium.wavePowers[id], 0);
            }
        }

    }



}
