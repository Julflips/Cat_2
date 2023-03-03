using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Collider2D col;

    private ContactFilter2D postFilter;

    private Vector2 input;

    private GameObject connected;

    public float speed;

    public GameObject fencePostPre;

    public GameObject fencePre;

    public GameObject fenceSketchPre;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        postFilter = new ContactFilter2D();
        postFilter.layerMask = LayerMask.GetMask("PostFence");
        connected = null;
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        if (input.magnitude > 1){
            input.Normalize();
        }

        if (Input.GetButtonDown("Place"))
        {
            PlaceFence();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(input * speed);
    }

    private void PlaceFence()
    {
        if (!connected)
        {
            
            Collider2D[] result = new Collider2D[1];
            GameObject post;
            if (col.OverlapCollider(postFilter, result) > 0)
            {
                Debug.Log("connecting to");
                post = result[0].gameObject;
            }
            else
            {
                Debug.Log("placing first");
                post = Instantiate(fencePostPre, transform.position, Quaternion.Euler(Vector3.zero));
            }
            connected = post;
            GameObject sketch = Instantiate(fenceSketchPre);
            sketch.GetComponent<Fence>().followA = gameObject;
            sketch.GetComponent<Fence>().followB = post;
        }
        else
        {

        }
    }
}
