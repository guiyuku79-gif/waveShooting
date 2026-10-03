using Unity.VisualScripting;
using UnityEngine;

public class BackStarController : MonoBehaviour
{
    private float size;
    [SerializeField] float speedPerSize;
    public void Init(float size)
    {
        this.size = size;
        transform.localScale = new Vector3(size, size, size);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x - size * speedPerSize, transform.position.y, 0);

        if (transform.position.x <= -14f)
        {
            Destroy(gameObject);
        }
    }
}
