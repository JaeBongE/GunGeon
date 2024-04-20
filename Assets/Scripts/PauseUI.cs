using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameoverUI;

    private void Awake()
    {
        pauseUI.SetActive(false);
        gameoverUI.SetActive(false);
    }

    public GameObject setPauseUI()
    {
        return pauseUI;
    }

    public GameObject setGameoverUI()
    {
        return gameoverUI;
    }
}
