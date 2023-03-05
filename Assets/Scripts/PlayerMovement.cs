using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private ContactFilter2D postFilter;
    public AudioSource high;
    public AudioSource low;
    public AudioSource music;

    private Vector2 input;

    private GameObject connected;
    private GameObject sketch;

    public GameObject targetPre;
    private GameObject throwTarget;

    public float speed;

    public GameObject fencePostPre;
    public GameObject fencePre;
    public GameObject fenceSketchPre;

    public GameObject FenceManager;
    private FenceManager fm;

    public float maxFenceLength = 3.5f;
    public int fencePostsLeft = 20;

    public int foodRemaining = 5;
    public float foodThrowStrenght = 0.1f;
    public GameObject foodPre;
    public List<GameObject> foods;
    public GameObject catManager;
    public TextMeshProUGUI strFood;
    public TextMeshProUGUI strPosts;

    private Animator animator;

    private bool throwAborted;

    public float timeToRecoverFood = 10f;
    public float timeToRecoverFence = 10f;

    private float timeLastFood;
    private float timeLastFence;


    // Start is called before the first frame update
    void Start()
    {
        music.Play();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        fm = FenceManager.GetComponent<FenceManager>();
        postFilter = new ContactFilter2D();
        postFilter.SetLayerMask(LayerMask.GetMask("FencePost"));
        connected = null;
        foods = new List<GameObject>();
        throwAborted = false;
        timeLastFence = Time.time;
        timeLastFood = Time.time;
    }

    void FixedUpdate()
    {
        rb.AddForce(input * (speed * Time.fixedDeltaTime));
    }
    
    // Update is called once per frame
    void Update()
    {
        if (catManager.GetComponent<CatManager>().gameEnded)
        {
            return;
        }
        strFood.text = "Food: " + foodRemaining;
        strPosts.text = "Posts: " + fencePostsLeft;
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
            throwAborted = true;
            AbortFence();
            Destroy(throwTarget);
            throwTarget = null;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            throwAborted = false;
        }
        if (Input.GetButton("Fire1"))
        {
            if (!throwAborted)
            {
                AbortFence();
                UpdateTarget();
            }
            
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (throwTarget && !throwAborted)
            {
                ThrowFence(throwTarget.transform.position);
                Destroy(throwTarget);
                throwTarget = null;
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            ThrowFood();
        }

        if (connected)
        {
            float distance = (sketch.transform.position - transform.position).magnitude;
            if (sketch.transform.localScale.y > 0.1f) {
                //Debug.Log(distance);
                if (distance > maxFenceLength)
                {
                    AbortFence();
                }
            }
        }

        if(timeLastFence + timeToRecoverFence < Time.time)
        {
            timeLastFence = Time.time;
            fencePostsLeft++; 
        }
        if (timeLastFood + timeToRecoverFood < Time.time)
        {
            timeLastFood = Time.time;
            foodRemaining++;
        }
    }

    private void ThrowFood()
    {
        if(foodRemaining<1)
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        mousePosition.z = 0;
        Vector3 v = (mousePosition - transform.position);
        GameObject food = Instantiate(foodPre, transform.position+(v.normalized), Quaternion.Euler(Vector3.zero));
        food.GetComponent<Fish>().catManager = catManager;
        food.GetComponent<Rigidbody2D>().AddForce(v * foodThrowStrenght);
        foods.Add(food);
        foodRemaining--;
    }

    private void UpdateTarget()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        mousePosition.z = 0;
        Vector3 v = (mousePosition - transform.position);
        float distance = Mathf.Clamp(v.magnitude, 0.2f, maxFenceLength);
        RaycastHit2D[] result = new RaycastHit2D[1];
        int hit = col.Raycast(v.normalized, result, distance, layerMask:~LayerMask.GetMask("FencePost"));
        Vector3 pos;
        if (hit == 0)
        {
            pos = transform.position + (v.normalized * distance);
        }
        else
        {
            pos = transform.position + (v.normalized * (result[0].distance - 0.2f));
        }
        if (!throwTarget)
        {
            throwTarget = Instantiate(targetPre, transform);
        }
        throwTarget.transform.position = pos;

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
                //Debug.Log("new connection to existing");
                connected = result[0].gameObject;
            }
            else
            {
                if (fencePostsLeft < 1)
                {
                    return;
                }
                playFenceSound();
                //Debug.Log("new connection to new");
                connected = Instantiate(fencePostPre, transform.position, Quaternion.Euler(Vector3.zero));
                fencePostsLeft--;
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
                    //Debug.Log("continue connection to existing");
                    fence.GetComponent<Fence>().Stretch(oldPost, result[0].gameObject);
                    oldPost.GetComponent<FencePost>().connectedFences.Add(fence);
                    result[0].GetComponent<FencePost>().connectedFences.Add(fence);
                    fm.UpdateCats();
                }
                else { 
                    //Debug.Log("not connecting to self");
                    Destroy(fence);
                    fencePostsLeft++;
                }
            }
            else
            {
                //Debug.Log("contine connection to new");
                if (fencePostsLeft < 1)
                {
                    Destroy(fence);
                    return;
                }
                connected = Instantiate(fencePostPre, transform.position, Quaternion.Euler(Vector3.zero));
                fencePostsLeft--;
                playFenceSound();
                fm.vertices.Add(connected.GetComponent<FencePost>());
                fence.GetComponent<Fence>().Stretch(oldPost, connected);
                sketch = Instantiate(fenceSketchPre);
                sketch.GetComponent<Fence>().followPlayer = gameObject;
                sketch.GetComponent<Fence>().followOldPost = connected;
                oldPost.GetComponent<FencePost>().connectedFences.Add(fence);
                connected.GetComponent<FencePost>().connectedFences.Add(fence);
                fm.UpdateCats();
            }
        }
    }

    private GameObject GetOrCreateRemoteFence(Vector3 position)
    {
        GameObject fencePost = Instantiate(fencePostPre, position, Quaternion.Euler(Vector3.zero));
        Collider2D[] result = new Collider2D[1];
        if (fencePost.GetComponent<CircleCollider2D>().OverlapCollider(postFilter, result) > 0)
        {
            Destroy(fencePost);
            return result[0].gameObject;
        }
        else
        {
            fencePostsLeft--;
            return fencePost;
        }
    }

    private void ThrowFence(Vector3 targetPos)
    {
        Collider2D[] result = new Collider2D[1];
        if (col.OverlapCollider(postFilter, result) > 0)
        {
            //Debug.Log("throwing from existing post");
            int pfStart = fencePostsLeft;
            GameObject post1 = result[0].gameObject;
            GameObject post2 = GetOrCreateRemoteFence(targetPos);
            if (fencePostsLeft < 0)
            {
                Destroy(post2);
                fencePostsLeft = pfStart;
                return;
            }
            playFenceSound();
            fm.vertices.Add(post2.GetComponent<FencePost>());
            GameObject fence = Instantiate(fencePre);
            fence.GetComponent<Fence>().Stretch(post1, post2);
            post1.GetComponent<FencePost>().connectedFences.Add(fence);
            post2.GetComponent<FencePost>().connectedFences.Add(fence);
        }
        else
        {

            //Debug.Log("throwing from new post");
            int pfStart = fencePostsLeft;
            GameObject post1 = Instantiate(fencePostPre, transform.position, Quaternion.Euler(Vector3.zero));
            fencePostsLeft--;
            GameObject post2 = GetOrCreateRemoteFence(targetPos);
            if (fencePostsLeft < 0)
            {
                Destroy(post1);
                Destroy(post2);
                fencePostsLeft = pfStart;
                return;
            }
            playFenceSound();
            fm.vertices.Add(post1.GetComponent<FencePost>());
            fm.vertices.Add(post2.GetComponent<FencePost>());
            GameObject fence = Instantiate(fencePre);
            fence.GetComponent<Fence>().Stretch(post1, post2);
            post1.GetComponent<FencePost>().connectedFences.Add(fence);
            post2.GetComponent<FencePost>().connectedFences.Add(fence);

        }
        fm.UpdateCats();

    }

    private void playFenceSound()
    {
        if (Random.Range(0, 2) == 0)
        {
            high.Play();
        }
        else
        {
            low.Play();
        }
        
    }

}
