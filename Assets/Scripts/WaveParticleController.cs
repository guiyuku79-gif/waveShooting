using UnityEngine;

public class WaveParticleController : MonoBehaviour
{
    [SerializeField] public float particleInterval = 0.1f;
    [SerializeField] Renderer renderer;

    private int id;
    public void Init(int id)
    {
        this.id = id;
        transform.position = new Vector3((id - LineSample.Instance.division / 2) * particleInterval, 0, 0);
    }

    void Update()
    {
        if (LineSample.Instance.waveIds[id] == 0)
        {
            renderer.enabled = false;
        }
        else
        {
            renderer.enabled = true;
            transform.position = new Vector3(transform.position.x, LineSample.Instance.wavePowers[id], 0);
        }
    }



}
