using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breeding : MonoBehaviour
{
    public GameObject playMap;
    public GameObject breedingMap;

    private List<GameObject> cage1 = new List<GameObject>();
    private List<GameObject> cage2 = new List<GameObject>();
    private List<GameObject> cage3 = new List<GameObject>();
    

    public void onStartPhase()
    {
        playMap.SetActive(false);
        breedingMap.SetActive(false);
    }
}
