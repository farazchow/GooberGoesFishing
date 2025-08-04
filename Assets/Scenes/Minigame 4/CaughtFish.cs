using UnityEngine;
using UnityEngine.InputSystem;

public class CaughtFish : MonoBehaviour
{
    InputAction leftMove;
    InputAction rightMove;

    public int numberOfLanes = 4;
    public float screenWidth;
    private int currentLane;
    private float movementPerLane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftMove = InputSystem.actions.FindAction("Left Move");
        rightMove = InputSystem.actions.FindAction("Right Move");
        movementPerLane = screenWidth / numberOfLanes;
        if (numberOfLanes % 2 == 0)
        {
            Vector3 newPosition = gameObject.transform.position;
            newPosition.x -= movementPerLane/2;
            gameObject.transform.position = newPosition;
            currentLane = (numberOfLanes / 2) - 1;
        }
        else
        {
            currentLane = (numberOfLanes - 1) / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (leftMove.WasPressedThisFrame() && currentLane > 0)
        {
            currentLane -= 1;
            Vector3 newPosition = gameObject.transform.position;
            newPosition.x -= movementPerLane;
            gameObject.transform.position = newPosition;
        }
        else if (rightMove.WasPressedThisFrame() && currentLane < numberOfLanes - 1)
        {
            currentLane += 1;
            Vector3 newPosition = gameObject.transform.position;
            newPosition.x += movementPerLane;
            gameObject.transform.position = newPosition;
        }
    }
}
