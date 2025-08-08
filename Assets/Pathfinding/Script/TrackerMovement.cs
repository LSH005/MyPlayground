using UnityEngine;

public class TrackerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;

    //void Update()
    //{
    //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    mousePosition.z = 0f;
    //    Vector3 direction = mousePosition - transform.position;

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //    transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    //}
}