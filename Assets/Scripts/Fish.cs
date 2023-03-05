using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public GameObject catManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.ToString() == "9")
        {
            catManager.GetComponent<CatManager>().delFood();
            Destroy(gameObject);
        }
    }
}
