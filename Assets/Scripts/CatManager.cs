using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public int areaX = 10;
    public int areaY = 5;
    public int numberOfCats;
    public List<GameObject> prefabCats;
    public GameObject player;
    public List<GameObject> cats;
    public TextMeshProUGUI timer;
    public int time;
    public GameObject fenceManager;
    public List<FencePost> posts;
    
    private float timeValue;
    private bool freezeTime = false;


    void Start()
    {
        posts = fenceManager.GetComponent<FenceManager>().vertices;
        timeValue = time;
        //Spawn cats around 0 in a areaX times areaY zone
        for (int i = 0; i < numberOfCats; i++)
        {
            int randomcat = Random.Range(0, prefabCats.Count);
            GameObject tempCat = Instantiate(prefabCats[randomcat], getRandomPos(areaX, areaY), Quaternion.identity);
            tempCat.GetComponent<CatBehaviour>().player = player;
            tempCat.GetComponent<CatBehaviour>().posts = posts;
            cats.Add(tempCat);
        }
    }

    public void newRound()
    {
        cats = new List<GameObject>();
        for (int i = 0; i < numberOfCats; i++)
        {
            int randomcat = Random.Range(0, prefabCats.Count);
            GameObject tempCat = Instantiate(prefabCats[randomcat], getRandomPos(areaX, areaY), Quaternion.identity);
            tempCat.GetComponent<CatBehaviour>().player = player;
            cats.Add(tempCat);
        }
    }

    private Vector2 getRandomPos(int x, int y)
    {
        return new Vector2(Random.Range(-x / 2, x / 2), Random.Range(-y/2, y/2));
    }

    private void Update()
    {
        if (timeValue > 0)
        {
            if (!freezeTime)
            {
                timeValue -= Time.deltaTime;
            }
        }
        else
        {
            timeValue = time;
            freezeTime = true;
            //timer end
        }

        float minutes = Mathf.FloorToInt(timeValue / 60);
        float seconds = Mathf.FloorToInt(timeValue % 60);

        timer.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }
}
