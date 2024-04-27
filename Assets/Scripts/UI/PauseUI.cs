using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameoverUI;
    [SerializeField] GameObject oneMoreUI;

    private void Awake()
    {
        pauseUI.SetActive(false);
        gameoverUI.SetActive(false);
    }

    public (GameObject pauseUI, GameObject gameoverUI, GameObject oneMoreUI) GetPauseUI()
    {
        return (pauseUI, gameoverUI, oneMoreUI);
    }

}
