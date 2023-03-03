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

    private GameObject sketch;

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
        postFilter.SetLayerMask(LayerMask.GetMask("FencePost"));
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
        if (Input.GetButtonDown("Abort"))
        {
            AbortFence();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(input * speed);
    }

    public void AbortFence()
    {
        if (connected)
        {
            connected = null;
            Destroy(sketch);
        }
    }

    private void PlaceFence()
    {
        if (!connected)
        {
            Collider2D[] result = new Collider2D[1];
            if (col.OverlapCollider(postFilter, result) > 0)
            {
                Debug.Log("new connection to existing");
                connected = result[0].gameObject;
            }
            else
            {
                Debug.Log("new connection to new");
                connected = Instantiate(fencePostPre, transform.position, Quaternion.Euler(Vector3.zero));
            }
            sketch = Instantiate(fenceSketchPre);
            sketch.GetComponent<Fence>().followPlayer = gameObject;
            sketch.GetComponent<Fence>().followOldPost = connected;
        }
        else
        {
            Destroy(sketch);
            sketch = null;
            Vector3 a = connected.transform.position;
            GameObject fence = Instantiate(fencePre);
            
            Collider2D[] result = new Collider2D[1];
            if (col.OverlapCollider(postFilter, result) > 0)
            {
                Debug.Log("continue connection to existing");
                connected = null;
                fence.GetComponent<Fence>().Stretch(a, result[0].transform.position);
            }
            else
            {
                Debug.Log("contine connection to new");
                connected = Instantiate(fencePostPre, transform.position, Quaternion.Euler(Vector3.zero));
                fence.GetComponent<Fence>().Stretch(a, connected.transform.position);
                sketch = Instantiate(fenceSketchPre);
                sketch.GetComponent<Fence>().followPlayer = gameObject;
                sketch.GetComponent<Fence>().followOldPost = connected;
            }
        }
    }


}
