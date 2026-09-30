using System;
using UnityEngine;

public class MotherShipController : MonoBehaviour
{
    public float hp;
    private float maxHp = 1f;

    public event Action MotherShipDestroyed;
    void Start()
    {
        hp = maxHp;
    }

    public void Damage(float damage)
    {
        hp = Mathf.Max(0f, hp - damage);
        if (hp == 0f) MotherShipDestroyed?.Invoke();
    }
}
