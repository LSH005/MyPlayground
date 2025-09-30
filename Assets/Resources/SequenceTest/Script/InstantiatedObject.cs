using UnityEngine;

public class InstantiatedObject : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Created object - Awake");
    }

    void Start()
    {
        Debug.Log("Created object - Start");
    }

    public void AwesomeFunction()
    {
        Debug.Log("Created object - Function");
    }
}
