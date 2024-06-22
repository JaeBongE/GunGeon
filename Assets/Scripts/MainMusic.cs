using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    AudioSource audio;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {

    }
}
