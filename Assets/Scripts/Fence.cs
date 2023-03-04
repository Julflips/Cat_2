using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{

    public GameObject followOldPost;
    public GameObject followPlayer;


    private LayerMask noBullshit;


    // Start is called before the first frame update
    void Start()
    {
        noBullshit = ~LayerMask.GetMask("Player", "FencePost");

    }

    // Update is called once per frame
    void Update()
    {
        if (followOldPost && followPlayer)
        {
            Stretch(followOldPost.transform.position, followPlayer.transform.position);
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

        transform.rotation = Quaternion.Euler(0, 0, AngleTo(a, b));
    }


    private float AngleTo(Vector2 a, Vector2 b)
    {
        float adjacent = (b.x - a.x);
        float opposite = (b.y - a.y);
        float deg =  Mathf.Atan(opposite / adjacent) * Mathf.Rad2Deg;
        return deg + 90;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (followOldPost && followPlayer && ((noBullshit & (1 << collision.gameObject.layer)) != 0))
        {
            if (collision.gameObject.tag.Equals("FenceSketch"))
            {
                return;
            }
            if (followOldPost.GetComponent<FencePost>().connectedFences.Contains(collision.gameObject))
            {
                return;
            }
            followPlayer.GetComponent<PlayerMovement>().AbortFence();

        }
    }
}
