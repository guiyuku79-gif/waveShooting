using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private Image fuelImage;

    [SerializeField] private TextMeshProUGUI pointText;

    private float velocityY;

    //波の残量についての変数
    public float fuelRate;
    public bool canMove;

    private float fuelConsumeSpeed = 0.1f;
    private float fuelChargeSpeed = 0.2f;

    public float point;

    //点滅処理用
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField, Min(0.01f)] private float blinkCycle = 1f;
    [SerializeField, Range(0f, 1f)] private float minAlpha = 0.2f;

    private float blinkTimer;

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
        UpdateBlink();

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
            fuelImage.fillAmount = fuelRate;
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
            fuelRate = Mathf.Clamp01(fuelRate);

            if (fuelRate <= 0f) canMove = false;
        }
        else
        {
            fuelRate += fuelChargeSpeed * Time.deltaTime;
            fuelImage.fillAmount = fuelRate;
            if (fuelRate >= 1f)
            {
                canMove = true;
                fuelRate = 1f;
            }
        }

        fuelImage.fillAmount = fuelRate;

    }

    private void UpdateBlink()
    {
        playerSprite.enabled = true;
        Color color = playerSprite.color;

        if (canMove)
        {
            blinkTimer = 0f;
            color.a = 1f;
        }
        else
        {
            blinkTimer = (blinkTimer + Time.deltaTime) % blinkCycle;

            float phase = blinkTimer / blinkCycle;
            float opacity = (Mathf.Cos(phase * Mathf.PI * 2f) + 1f) / 2f;

            color.a = Mathf.Lerp(minAlpha, 1f, opacity);
        }

        playerSprite.color = color;
    }
}
