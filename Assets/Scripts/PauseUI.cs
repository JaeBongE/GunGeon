using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;

    private void Awake()
    {
        pauseUI.SetActive(false);
    }

    public GameObject setPauseUI()
    {
        return pauseUI;
    }
}
