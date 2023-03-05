using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class Menu : MonoBehaviour
{
    public GameObject slider;
    public AudioSource meow;
    public AudioSource music;
    public float volume = 0.5f;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void onStart()
    {
        SceneManager.LoadScene("UwU");
    }

    public void onSliderChange()
    {
        volume = slider.GetComponent<Slider>().value;
        meow.volume = volume;
        music.volume = volume;
        meow.Play();
    }
}
