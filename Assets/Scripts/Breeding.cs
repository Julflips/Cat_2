using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class Breeding : MonoBehaviour
{
    public GameObject playMap;
    public GameObject breedingMap;
    public TextMeshProUGUI strMoney;
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public TextMeshProUGUI strCap1;
    public TextMeshProUGUI strCap2;
    public TextMeshProUGUI strCap3;
    public GameObject add1;
    public GameObject add2;
    public GameObject add3;
    public List<int> expensiveCats;
    public GameObject phase1;
    public GameObject phase2;
    public Transform catPodest;
    public GameObject mainCamera;
    public GameObject breedCamera;
    public GameObject breedUI;
    public GameObject player;
    public List<GameObject> prefabCats;
    public Transform coolCatPodest;
    public GameObject podest;
    public TextMeshProUGUI timer;
    public int time;

    private List<GameObject> cage1 = new List<GameObject>();
    private List<GameObject> cage2 = new List<GameObject>();
    private List<GameObject> cage3 = new List<GameObject>();
    private int money;
    private List<GameObject> cats;
    private int index = -1;
    private GameObject aktCat;
    private int maxCagesize = 4;
    private int cap1;
    private int cap2;
    private int cap3;
    private List<int> catPrices = new List<int>() {1, 2, 3, 4, 5};
    private List<GameObject> realExpensiveCats = new List<GameObject>();
    private float timeValue;

    private void Start()
    {
        timeValue = time;
        podest.SetActive(false);
        expensiveCats = new List<int>{Random.Range(0,catPrices.Count), Random.Range(0,catPrices.Count), Random.Range(0,catPrices.Count)};
        int offset = 2;
        int tempOffest = 0;
        foreach (int type in expensiveCats)
        {
            GameObject tempCat = Instantiate(prefabCats[type]);
            tempCat.GetComponent<CatBehaviour>().player = player;
            tempCat.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tempCat.transform.position = coolCatPodest.position;
            tempCat.transform.position += Vector3.down * tempOffest;
            //Debug.Log("cat at: " + tempCat.transform.position);
            realExpensiveCats.Add(tempCat);
            tempOffest += offset;
            tempCat.SetActive(false);
        }
        strCap1.text = "0/" + maxCagesize;
        strCap2.text = "0/" + maxCagesize;
        strCap3.text = "0/" + maxCagesize;
    }


    public void onStartPhase()
    {
        foreach (GameObject cat in realExpensiveCats)
        {
            cat.SetActive(true);
        }
        player.SetActive(false);
        breedUI.SetActive(true);
        mainCamera.SetActive(false);
        breedCamera.SetActive(true);
        playMap.SetActive(false);
        breedingMap.SetActive(true);
        podest.SetActive(true);
        cats = GetComponent<CatManager>().cats;
        foreach (GameObject cat in cats)
        {
            cat.SetActive(false);
        }
        phase1.SetActive(true);
        phase2.SetActive(false);
        expensiveCats.Add(Random.Range(0,catPrices.Count));
        expensiveCats.RemoveAt(0);
        nextCat();
    }

    public void nextCat()
    {
        index++;
        if (index >= cats.Count)
        {
            phase1.SetActive(false);
            phase2.SetActive(true);
        }
        else
        {
            aktCat = cats[index];
            aktCat.SetActive(true);
            aktCat.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            aktCat.transform.position = catPodest.position;
        }
    }

    public void chooseCage(int cage)
    {
        //Debug.Log("Cat to cage: " + cage);
        if (cage == 1)
        {
            aktCat.transform.position = spawn1.position;
            cage1.Add(aktCat);
            cap1++;
            strCap1.text = cap1 + "/" + maxCagesize;
            if (cap1 == maxCagesize)
            {
                add1.GetComponent<Button>().interactable = false;
            }
        }

        if (cage == 2)
        {
            aktCat.transform.position = spawn2.position;
            cage2.Add(aktCat);
            cap2++;
            strCap2.text = cap2 + "/" + maxCagesize;
            if (cap2 == maxCagesize)
            {
                add2.GetComponent<Button>().interactable = false;
            }
        }
        
        if (cage == 3)
        {
            aktCat.transform.position = spawn3.position;
            cage3.Add(aktCat);
            cap3++;
            strCap3.text = cap3 + "/" + maxCagesize;
            if (cap3 == maxCagesize)
            {
                add3.GetComponent<Button>().interactable = false;
            }
        }
        
        aktCat.SetActive(true);
        aktCat.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        nextCat();
    }

    public void sellCage(int cage)
    {
        if (cage == 1)
        {
            if (cap1 == 0)
            {
                return;
            }

            cap1 = 0;
            strCap1.text = cap1 + "/" + maxCagesize;
            sellCats(cage1);
            cage1 = new List<GameObject>();
            add1.GetComponent<Button>().interactable = true;
        }
        if (cage == 2)
        {
            if (cap2 == 0)
            {
                return;
            }
            
            cap2 = 0;
            strCap2.text = cap2 + "/" + maxCagesize;
            sellCats(cage2);
            cage2 = new List<GameObject>();
            add2.GetComponent<Button>().interactable = true;
        }
        if (cage == 3)
        {
            if (cap3 == 0)
            {
                return;
            }
            
            cap3 = 0;
            strCap3.text = cap3 + "/" + maxCagesize;
            sellCats(cage3);
            cage3 = new List<GameObject>();
            add3.GetComponent<Button>().interactable = true;
        }
    }

    private void sellCats(List<GameObject> slaves)
    {
        int stonks = 0;
        int factor;
        foreach (GameObject cat in slaves)
        {
            if (cat.GetComponent<CatBehaviour>().type == expensiveCats[0])
            {
                factor = 2;
            }
            else
            {
                factor = 1;
            }
            stonks += catPrices[cat.GetComponent<CatBehaviour>().type] * factor;
        }
        foreach (GameObject cat in slaves)
        {
            Destroy(cat);
        }

        money += stonks;
        strMoney.text = "Money: " + money;
    }
    
    public void breedCage(int cage)
    {
        
    }

    public void finished()
    {
        player.SetActive(true);
        Debug.Log("Finished");
        breedUI.SetActive(false);
        mainCamera.SetActive(true);
        breedCamera.SetActive(false);
        playMap.SetActive(true);
        breedingMap.SetActive(false);
        podest.SetActive(false);
    }

    private void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
            onStartPhase();
        }

        float minutes = Mathf.FloorToInt(timeValue / 60);
        float seconds = Mathf.FloorToInt(timeValue % 60);

        timer.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }
}
