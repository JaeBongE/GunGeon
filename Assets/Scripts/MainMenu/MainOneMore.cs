using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainOneMore : MonoBehaviour
{
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;

    void Start()
    {
        SelectBtnYes();
        SelectBtnNo();
    }

    private void SelectBtnYes()
    {
        btnYes.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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

