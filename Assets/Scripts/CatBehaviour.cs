using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    public GameObject player;
    public float moveCooldown;
    public float strength;
    public float playerDetectionRange;
    public float fenceDetectionRange;
    public float playerWeight;
    public float foodDetectionRange;
    public float zoomieProp;
    public int zoomieLength;
    public int type;

    private float lastTimeMoved = 0;
    private Rigidbody2D rigi;
    private List<Vector2> posts = new List<Vector2>();
    private List<Vector2> foods = new List<Vector2>();
    private bool getFood = false;
    private Vector2 food;
    private float zoomieCheck = 0.6f;
    private float lastTimeZoomed = 0;
    private bool zoomin = false;
    private int zoomed = 0;

    public Animator animator;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        //Change Sprite according to type
    }
    
    void Update()
    {
        if (Time.timeSinceLevelLoad - lastTimeZoomed >= zoomieCheck)
        {
            if (zoomin)
            {
                Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rigi.AddForce(direction * strength * 2);
                zoomed++;
                if (zoomed >= zoomieLength)
                {
                    zoomin = false;
                    zoomed = 0;
                }
            }
            else
            {
                if (Random.value <= zoomieProp)
                {
                    zoomin = true;
                }
            }
            
            lastTimeZoomed = Time.timeSinceLevelLoad;
        }
        if (Time.timeSinceLevelLoad - lastTimeMoved >= moveCooldown && !zoomin)
        {
            getFood = false;
            foreach (Vector2 vec in foods)
            {
                if (Vector2.Distance(vec, transform.position) <= foodDetectionRange)
                {
                    getFood = true;
                    food = vec;
                }
            }

            if (getFood)
            {
                Vector2 direction = food - (Vector2)transform.position;
                direction.Normalize();
                rigi.AddForce(direction * strength);
            }
            else
            {
                if (Vector2.Distance(player.transform.position, transform.position) <= playerDetectionRange)
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
                }
                else
                {
                    //random walk
                    Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    rigi.AddForce(direction * strength/2);
                }
            }
            
            lastTimeMoved = Time.timeSinceLevelLoad;
            
            animator.SetBool("zoomin", zoomin);
            animator.SetFloat("vertical_speed", rigi.velocity.y);
            animator.SetFloat("horizontal_speed", rigi.velocity.x);
        }
    }

    public void addPost(Vector2 pos)
    {
        posts.Add(pos);
    }

    public void addFood(Vector2 pos)
    {
        foods.Add(pos);
    }

    public void delFood(Vector2 pos)
    {
        foods.Remove(pos);
    }
}
