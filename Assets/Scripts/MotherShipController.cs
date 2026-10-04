using System;
using UnityEngine;

public class MotherShipController : MonoBehaviour
{
    public float hp;
    [SerializeField] float maxHp = 1f;

    [SerializeField] float moveRange = 1f;
    [SerializeField] float moveCycle = 1f;

    private float timeCount;

    public event Action MotherShipDestroyed;
    void Start()
    {
        hp = maxHp;

        timeCount = 0;
    }

    void Update()
    {
        timeCount += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, moveRange * Mathf.Sin(timeCount / moveCycle), 0);
    }

    public void Damage(float damage)
    {
        hp = Mathf.Max(0f, hp - damage);
        if (hp == 0f) MotherShipDestroyed?.Invoke();
    }
}
