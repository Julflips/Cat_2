using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupSounds : MonoBehaviour
{
    public List<AudioSource> au;
    
    
    private GameObject cam;
    public float volume;

    private void Start()
    {
        cam = GameObject.Find("TitleCamera");
        volume = cam.GetComponent<Menu>().volume;
        foreach (AudioSource asss in au)
        {
            asss.volume = volume;
        }
    }
}
