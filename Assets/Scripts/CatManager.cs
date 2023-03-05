using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        //Spawn cats around 0 in a areaX times areaY zone
        for (int i = 0; i < numberOfCats; i++)
        {
            int randomcat = Random.Range(0, prefabCats.Count);
            GameObject tempCat = Instantiate(prefabCats[randomcat], getRandomPos(areaX, areaY), Quaternion.identity);
            tempCat.GetComponent<CatBehaviour>().player = player;
            cats.Add(tempCat);
        }
        //test();
    }

    private void test()
    {
        GetComponent<Breeding>().onStartPhase();
    }

    private Vector2 getRandomPos(int x, int y)
    {
        return new Vector2(Random.Range(-x / 2, x / 2), Random.Range(-y/2, y/2));
    }

    private void addFood(Vector2 pos)
    {
        foreach (GameObject tempCat in cats)
        {
            tempCat.GetComponent<CatBehaviour>().addFood(pos);
        }
    }
    
    private void delFood(Vector2 pos)
    {
        foreach (GameObject tempCat in cats)
        {
            tempCat.GetComponent<CatBehaviour>().delFood(pos);
        }
    }
    
    private void addPost(Vector2 pos)
    {
        foreach (GameObject tempCat in cats)
        {
            tempCat.GetComponent<CatBehaviour>().addPost(pos);
        }
    }
}
