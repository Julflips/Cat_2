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

    public GameObject FenceManager;
    private FenceManager fm;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        fm = FenceManager.GetComponent<FenceManager>();
        postFilter = new ContactFilter2D();
        postFilter.SetLayerMask(LayerMask.GetMask("FencePost"));
        connected = null;
    }

    void FixedUpdate()
    {
        rb.AddForce(input * (speed * Time.fixedDeltaTime));
    }
    
    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        if (input.magnitude > 1){
            input.Normalize();
        }
        
        animator.SetFloat("movement_speedus", speed/10);
        animator.SetFloat("horizontal_speed", rb.velocity.x*1000);
        animator.SetFloat("vertical_speed", rb.velocity.y*1000);

        if (Input.GetButtonDown("Place"))
        {
            PlaceFence();
        }
        if (Input.GetButtonDown("Abort"))
        {
            AbortFence();
        }
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
                fm.vertices.Add(connected.GetComponent<FencePost>());
            }
            sketch = Instantiate(fenceSketchPre);
            sketch.GetComponent<Fence>().followPlayer = gameObject;
            sketch.GetComponent<Fence>().followOldPost = connected;
        }
        else
        {
            Destroy(sketch);
            sketch = null;
            GameObject oldPost = connected;
            GameObject fence = Instantiate(fencePre);
            

            Collider2D[] result = new Collider2D[1];
            if (col.OverlapCollider(postFilter, result) > 0)
            {
                
                connected = null;

                if (result[0].gameObject != oldPost)
                {
                    Debug.Log("continue connection to existing");
                    fence.GetComponent<Fence>().Stretch(oldPost, result[0].gameObject);
                    oldPost.GetComponent<FencePost>().connectedFences.Add(fence);
                    result[0].GetComponent<FencePost>().connectedFences.Add(fence);
                }
                else { 
                    Debug.Log("not connecting to self");
                    Destroy(fence);
                }
            }
            else
            {
                Debug.Log("contine connection to new");
                connected = Instantiate(fencePostPre, transform.position, Quaternion.Euler(Vector3.zero));
                fm.vertices.Add(connected.GetComponent<FencePost>());
                fence.GetComponent<Fence>().Stretch(oldPost, connected);
                sketch = Instantiate(fenceSketchPre);
                sketch.GetComponent<Fence>().followPlayer = gameObject;
                sketch.GetComponent<Fence>().followOldPost = connected;
                oldPost.GetComponent<FencePost>().connectedFences.Add(fence);
                connected.GetComponent<FencePost>().connectedFences.Add(fence);
            }
        }
    }


}
