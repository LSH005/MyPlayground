using System;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject walls;

    private int nodeNumber = 0;

    private bool isActivating = false;

    void Update()
    {
        if (isActivating)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isActivating = true;

            }

            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;

                Instantiate(walls, mousePosition, Quaternion.identity);
            }
        }
    }
}