using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingBack : MonoBehaviour
{
    [SerializeField] Button btnMain;
    [SerializeField] Button btnQuit;

    void Start()
    {
        btnMain.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            LoadingSceneController.Instance.LoadScene("MainManu");
        });

        btnQuit.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

}
