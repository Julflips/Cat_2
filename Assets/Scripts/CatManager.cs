using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatManager : MonoBehaviour
{
    public int areaX = 10;
    public int areaY = 5;
    public int numberOfCats;
    public List<GameObject> prefabCats;
    public GameObject player;
    public List<GameObject> cats;
    public TextMeshProUGUI timer;
    public GameObject fenceManager;
    public List<FencePost> posts;
    public GameObject endScreen;
    public TextMeshProUGUI points;
    public List<GameObject> foods;

    private float timeValue = 0;
    private bool freezeTime = false;


    void Start()
    {
        posts = fenceManager.GetComponent<FenceManager>().vertices;
        foods = player.GetComponent<PlayerMovement>().foods;
        //Spawn cats around 0 in a areaX times areaY zone
        for (int i = 0; i < numberOfCats; i++)
        {
            int randomcat = Random.Range(0, prefabCats.Count);
            GameObject tempCat = Instantiate(prefabCats[randomcat], getRandomPos(areaX, areaY), Quaternion.identity);
            tempCat.GetComponent<CatBehaviour>().player = player;
            tempCat.GetComponent<CatBehaviour>().posts = posts;
            tempCat.GetComponent<CatBehaviour>().foods = foods;
            cats.Add(tempCat);
        }
    }

    public void newRound()
    {
        SceneManager.LoadScene("UwU");
    }

    private Vector2 getRandomPos(int x, int y)
    {
        return new Vector2(Random.Range(-x / 2, x / 2), Random.Range(-y/2, y/2));
    }

    private void Update()
    {
        timeValue += Time.deltaTime;

        float minutes = Mathf.FloorToInt(timeValue / 60);
        float seconds = Mathf.FloorToInt(timeValue % 60);

        timer.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    public void endGame()
    {
        float endtime = timeValue;
        timer.enabled = false;
        endScreen.SetActive(true);
        int p = fenceManager.GetComponent<FenceManager>().capturedCats.Count;
        if (p == 0)
        {
            points.text = "Lmao you didn't catch a single cat";
        }
        else
        {
            if (p == 1)
            {
                points.text = points.text = "You caught a cat (at least u got one <3)";
            }
            else
            {
                points.text = "You caught " + p + " cats UwU";
            }
        }
    }
}
