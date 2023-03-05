using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public GameObject catManager;
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Fibsh");
        if (other.gameObject.layer.ToString() == "9")
        {
            catManager.GetComponent<CatManager>().eat();
            catManager.GetComponent<CatManager>().foods.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
