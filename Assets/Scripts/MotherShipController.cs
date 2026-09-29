using UnityEngine;

public class MotherShipController : MonoBehaviour
{
    public float hp;
    private float maxHp = 10f;
    void Start()
    {
        hp = maxHp;
    }

    public void Damage(float damage)
    {
        hp = Mathf.Max(0f, hp - damage);
    }
}
