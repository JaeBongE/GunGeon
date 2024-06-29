using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;
    [SerializeField] GameObject MenuUI;

    void Start()
    {
        SelectBtnYes();
        SelectBtnNo();
    }

    private void SelectBtnYes()
    {
        btnYes.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            LoadingSceneController.Instance.LoadScene("Stage1");
            Destroy(MenuUI);
        });
    }

    private void SelectBtnNo()
    {
        btnNo.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
