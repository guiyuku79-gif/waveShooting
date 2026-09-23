using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float smoothTime = 0.1f;

    private float velocityY;

    void Start()
    {
        transform.position = new Vector3(-Constants.Width / 2, 0, 0);
    }

    void Update()
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


        float targetY;
        if (Mouse.current.leftButton.isPressed && isInLane)
        {
            if (mouseWorld.y >= currentLane + Constants.LaneHeight / 2) targetY = currentLane + Constants.LaneHeight / 2;
            else if (mouseWorld.y <= currentLane - Constants.LaneHeight / 2) targetY = currentLane - Constants.LaneHeight / 2;
            else targetY = mouseWorld.y;
        }
        else
        {
            targetY = mouseWorld.y;
        }


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
}
