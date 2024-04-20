using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Button btnExit;
    [SerializeField] Button btnCancel;
    [SerializeField] GameObject oneMore;

    private void Awake()
    {
        oneMore.SetActive(false);
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        cancel();
        exit();
    }

    private void exit()
    {
        btnExit.onClick.AddListener(() =>
        {
            oneMore.SetActive(true);
        });
    }

    private void cancel()
    {
        btnCancel.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            gameManager.checkPauseUI(false);
            gameObject.SetActive(false);
        });
    }
}
