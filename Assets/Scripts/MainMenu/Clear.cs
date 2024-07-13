using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;
    [SerializeField] GameObject MenuUI;

    private float master;
    private float bgm; 
    private float sfx;


    void Start()
    {
        SelectBtnYes();
        SelectBtnNo();
    }

    private void SelectBtnYes()
    {
        btnYes.onClick.AddListener(() =>
        {
            if (PlayerPrefs.HasKey("setMaster"))
            {
                master = PlayerPrefs.GetFloat("setMaster");
            }
            if (PlayerPrefs.HasKey("setBGM"))
            {
                bgm = PlayerPrefs.GetFloat("setBGM");
            }
            if (PlayerPrefs.HasKey("setSFX"))
            {
                sfx = PlayerPrefs.GetFloat("setSFX");
            }

            PlayerPrefs.DeleteAll();

            if (master != 0)
            {
                PlayerPrefs.SetFloat("setMaster", master);
            }
            if (bgm != 0)
            {
                PlayerPrefs.SetFloat("setBGM", bgm);
            }
            if (sfx != 0)
            {
                PlayerPrefs.SetFloat("setSFX", sfx);
            }

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
