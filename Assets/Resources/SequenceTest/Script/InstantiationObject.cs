using UnityEngine;

public class InstantiationObject : MonoBehaviour
{
    public GameObject something;

    private void Awake()
    {
        Debug.Log("Constructor object - Awake");
    }

    void Start()
    {
        Debug.Log("Constructor object - Start");
        InstantiateSomething();
    }

    private void InstantiateSomething()
    {
        GameObject CreatedObj = Instantiate(something);
        InstantiatedObject CreatedObjScript = CreatedObj.GetComponent<InstantiatedObject>();
        CreatedObjScript.AwesomeFunction();
    }
}
