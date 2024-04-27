using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OneMore : MonoBehaviour
{

    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;

    void Start()
    {
        seletYes();
        seletNo();
    }


    private void seletYes()
    {
        btnYes.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            //SceneManager.LoadSceneAsync(0);
            LoadingSceneController.Instance.LoadScene("MainManu");
        });
    }

    private void seletNo()
    {
        btnNo.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
