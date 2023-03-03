using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    public GameObject player;
    public List<Vector2> posts;
    public float moveCooldown;
    public float strength;
    public float playerDetectionRange;
    public float fenceDetectionRange;
    public float playerWeight;

    private float lastTimeMoved = 0;
    private Rigidbody2D rigi;
    
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= playerDetectionRange)
        {
            if (Time.timeSinceLevelLoad - lastTimeMoved >= moveCooldown)
            {
                //Debug.Log("Move");
                Vector2 direction = (player.transform.position - transform.position) * playerWeight;
                foreach (Vector2 vec in posts)
                {
                    if (Vector2.Distance(vec, transform.position) <= fenceDetectionRange)
                    {
                        direction += vec;
                    }
                }
                direction.Normalize();
                rigi.AddForce(-direction * strength);

                lastTimeMoved = Time.timeSinceLevelLoad;
            }
        }
    }
}
