using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private Image FuelImage;

    [SerializeField] private TextMeshProUGUI pointText;

    private float velocityY;

    //波の残量についての変数
    public float fuelRate;
    public bool canMove;

    private float fuelConsumeSpeed = 0.02f;
    private float fuelChargeSpeed = 0.4f;

    public float point;

    void Start()
    {
        transform.position = new Vector3(-Constants.Width / 2, 0, 0);

        fuelRate = 1.0f;
        point = 0;
        canMove = true;

    }

    void Update()
    {
        Move();
        FuelChange();

        pointText.text = $"point: {(int)(point * 10)}";
    }

    private void Move()
    {
        if (!canMove) return;

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

    public void Damage(float damage)
    {
        if (canMove)
        {
            fuelRate -= damage;
            FuelImage.fillAmount = fuelRate;
        }
    }



    private void FuelChange()
    {
        if (canMove)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                fuelRate -= fuelConsumeSpeed * Time.deltaTime;
            }
            else
            {
                fuelRate += fuelChargeSpeed * Time.deltaTime;
            }
            fuelRate =  Mathf.Clamp01(fuelRate);

            if (fuelRate <= 0f) canMove = false;
        }
        else
        {
            fuelRate += fuelChargeSpeed * Time.deltaTime;
            FuelImage.fillAmount = fuelRate;
            if (fuelRate >= 1f)
            {
                canMove = true;
                fuelRate = 1f;
            }
        }

        FuelImage.fillAmount = fuelRate;

    }
}
