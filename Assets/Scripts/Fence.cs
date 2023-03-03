using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{

    public GameObject followA;
    public GameObject followB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followA && followB)
        {
            Stretch(followA.transform.position, followB.transform.position);
        }
        
    }

    public void Stretch(Vector2 a, Vector2 b)
    {
        if (a.Equals(b))
        {
            return;
        }

        transform.position = new Vector2((a.x + b.x) / 2, (a.y + b.y) / 2);
        transform.localScale = new Vector3(transform.localScale.x, (a - b).magnitude, transform.localScale.z); //ugly
        //Debug.Log(AngleTo(a, b));

        transform.rotation = Quaternion.Euler(0, 0, AngleTo(a, b));
    }


    private float AngleTo(Vector2 a, Vector2 b)
    {
        float adjacent = (b.x - a.x);
        float opposite = (b.y - a.y);
        float deg =  Mathf.Atan(opposite / adjacent) * Mathf.Rad2Deg;
        return deg + 90;
    }
}
