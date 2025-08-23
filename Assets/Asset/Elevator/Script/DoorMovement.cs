using System.Collections;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    private Coroutine movementCoroutine;
    private Vector3 initialPosition; 
    private Vector3 openPosition;

    void Awake()
    {
        initialPosition = transform.localPosition;
        openPosition = new Vector3(initialPosition.x + 1, initialPosition.y, initialPosition.z);
    }

    public void DoorOpen(float time)
    {
        if (time <= 0f)
        {
            StopMovement();
            transform.localPosition = openPosition;
            return;
        }

        float speed = 1.0f / time;
        StartMovement(openPosition, speed);
    }

    public void DoorClose(float time)
    {
        if (time <= 0f)
        {
            StopMovement();
            transform.localPosition = initialPosition;
            return;
        }

        float speed = 1.0f / time;
        StartMovement(initialPosition, speed);
    }

    private void StartMovement(Vector3 target, float speed)
    {
        StopMovement();
        ElevatorManager.isDoorMoving = true;
        movementCoroutine = StartCoroutine(MoveDoorTo(target, speed));
    }

    private void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            ElevatorManager.isDoorMoving = false;
            movementCoroutine = null;
        }
    }

    private IEnumerator MoveDoorTo(Vector3 target, float speed)
    {
        while (transform.localPosition != target)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
            yield return null;
        }

        ElevatorManager.isDoorMoving = false;
        movementCoroutine = null;
    }
}