using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRepeatMusic : MonoBehaviour
{
    private GameObject[] music;

    void Start()
    {
        music = GameObject.FindGameObjectsWithTag("gameMusic");

        // Check if the array has at least two elements
        if (music.Length >= 2)
        {
            // Destroy the second element in the array
            Destroy(music[1]);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}