using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Button btnMainMenu;
    [SerializeField] Button btnContinue;

    private void Start()
    {
        goMain();
        goContinue();
    }

    private void goMain()
    {
        btnMainMenu.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            LoadingSceneController.Instance.LoadScene("MainManu");
            //Destroy(gameObject);
        });
    }

    private void goContinue()
    {
        btnContinue.onClick.AddListener(() =>
        {
            string loadScene = PlayerPrefs.GetString("Continue");
            Time.timeScale = 1f;
            LoadingSceneController.Instance.ContinueScene(loadScene);
            //Destroy(gameObject);
        });
    }
}
