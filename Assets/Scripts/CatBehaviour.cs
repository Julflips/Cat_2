using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public float sittingProp;
    public int sittingDuration;
    public int zoomieLength;
    public int zoomieStregth;
    public int type;

    private float lastTimeMoved = 0;
    private Rigidbody2D rigi;
    private List<Vector2> posts = new List<Vector2>();
    private List<Vector2> foods = new List<Vector2>();
    private bool getFood = false;
    private Vector2 food;
    private float eventCheck = 0.6f;
    private float lastTimeEvented = 0;
    private bool zoomin = false;
    private int zoomed = 0;
    private bool sitting = false;
    private float startedSitting;

    private bool walking;
    private float startedWalking;
    private Vector2 direction = new Vector2();
    

    private Animator animator;
    private SpriteRenderer sr;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Change Sprite according to type
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        animator.SetBool("sitting", sitting);
        animator.SetBool("zoomin", zoomin);
        if (Mathf.Abs(rigi.velocity.y) > Mathf.Abs(rigi.velocity.x))
        {
            animator.SetFloat("vertical_speed", rigi.velocity.y*10000);
            animator.SetFloat("horizontal_speed", 0);
            if (rigi.velocity.x * 10000 < 0.01)
            {
                sr.flipX = false;
            } else
            {
                sr.flipX = true;
            }
        }
        else
        {   
            animator.SetFloat("horizontal_speed", rigi.velocity.x*10000);
            animator.SetFloat("vertical_speed", 0);
            if (rigi.velocity.x * 10000 < 0.01)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
    }

    void Update()
    {
        if (rigi.bodyType == RigidbodyType2D.Static)
        {
            return;
        }
        //Debug.Log("Zooming: " + zoomin + "   Sitting: " + sitting);
        if (sitting && Time.timeSinceLevelLoad - startedSitting >= sittingDuration)
        {
            sitting = false;
            rigi.bodyType = RigidbodyType2D.Dynamic;
        }
        if (Time.timeSinceLevelLoad - lastTimeEvented >= eventCheck)
        {
            if (!sitting)
            {
                if (zoomin)
                {
                    Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    rigi.AddForce(direction * zoomieStregth);
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
                    else
                    {
                        if (Random.value <= zoomieProp)
                        {
                            rigi.velocity = new Vector2();
                            rigi.bodyType = RigidbodyType2D.Static;
                            startedSitting = Time.timeSinceLevelLoad;
                            sitting = true;
                        }
                    }
                }
            }

            lastTimeEvented = Time.timeSinceLevelLoad;
        }
        if (Time.timeSinceLevelLoad - lastTimeMoved >= moveCooldown && !zoomin && !sitting)
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
                //walk to food
                direction = food - (Vector2)transform.position;
                direction.Normalize();
                //rigi.AddForce(direction * strength);
            }
            else
            {
                if (Vector2.Distance(player.transform.position, transform.position) <= playerDetectionRange)
                {
                    //walk away from player
                    direction = (player.transform.position - transform.position) * playerWeight;
                    foreach (Vector2 vec in posts)
                    {
                        if (Vector2.Distance(vec, transform.position) <= fenceDetectionRange)
                        {
                            direction += vec;
                        }
                    }

                    direction = -direction;
                    direction.Normalize();
                    //rigi.AddForce(-direction * strength);
                }
                else
                {
                    //random walk
                    direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    //rigi.AddForce(direction * strength/2);
                }
            }
            
            lastTimeMoved = Time.timeSinceLevelLoad;
            
        }
        else
        {
            if (!(direction.x == 0 && direction.y == 0))
            {
                rigi.AddForce(direction * (strength * Time.deltaTime));
            }
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
