using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private Image FuelImage;

    private float velocityY;

    //波の残量についての変数
    public float FuelRate { get; private set; }

    private float fuelConsumeSpeed = 0.02f;
    private float fuelChargeSpeed = 0.4f;

    void Start()
    {
        transform.position = new Vector3(-Constants.Width / 2, 0, 0);

        FuelRate = 1.0f;
    }

    void Update()
    {
        Move();
        FuelChange();
    }

    private void Move()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        float currentLane = 0f;
        bool isInLane = false;
        foreach (float laneY in Constants.laneYs)
        {
            if (laneY - Constants.LaneHeight / 2 <= transform.position.y &&
                transform.position.y <= laneY + Constants.LaneHeight / 2)
            {
                currentLane = laneY;
                isInLane = true;
            }
        }


        float targetY = mouseWorld.y;
        if (Mouse.current.leftButton.isPressed && isInLane)
        {
            targetY = Mathf.Clamp(
                targetY,
                currentLane - Constants.LaneHeight / 2,
                currentLane + Constants.LaneHeight / 2
            );
        }

        targetY = Mathf.Clamp(
            targetY,
            -Constants.ScreenHeight / 2,
            Constants.ScreenHeight / 2
        );


        float newY = Mathf.SmoothDamp(
            transform.position.y,
            targetY,
            ref velocityY,
            smoothTime,
            maxSpeed
        );

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    private void FuelChange()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            FuelRate -= fuelConsumeSpeed * Time.deltaTime;
            if (FuelRate <= 0f) FuelRate = 0f;
        }
        else
        {
            FuelRate += fuelChargeSpeed * Time.deltaTime;
            if (FuelRate >= 1f) FuelRate = 1f;
        }
        FuelImage.fillAmount = FuelRate;
    }
}
