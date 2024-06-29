using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("총")]
    [SerializeField] Image gunUi;
    [SerializeField] TMP_Text gunBulletUi;

    [Header("체력")]
    [SerializeField] Image hp3;
    [SerializeField] Image hp2;
    [SerializeField] Image hp1;

    [Header("대시")]
    [SerializeField] Image dashImage;
    [SerializeField] TMP_Text tmpDashCool;

    [Header("무적")]
    [SerializeField] Button btnNodamage;
    [SerializeField] TMP_Text TextNodamage;
    private bool isNodamage = false;

    [SerializeField] Button btnSettings;

    public (Image gunUi, TMP_Text gunBulletUi) GetGunUI()
    {
        return (gunUi, gunBulletUi);
    }

    public (Image hp3, Image hp2, Image hp1) GetHpUI()
    {
        return (hp3, hp2, hp1);
    }

    public (Image dashImage, TMP_Text tmpDashCool) GetDashUI()
    {
        return (dashImage, tmpDashCool);
    }

    private void Start()
    {
        GameObject objPlayer = GameObject.Find("Player");
        Player scPlayer = objPlayer.GetComponent<Player>();

        if (PlayerPrefs.HasKey("Nodamage"))
        {
            isNodamage = true;
            TextNodamage.text = "ON";
            scPlayer.CheckNodamage(true);
            btnNodamage.image.color = Color.blue;
        }
        else
        {
            isNodamage = false;
            TextNodamage.text = "OFF";
            scPlayer.CheckNodamage(false);
            btnNodamage.image.color = Color.red;
        }

        checkNodamage();

        btnSettings.onClick.AddListener(() => 
        {
            SettingsUI scSettingUI = btnSettings.GetComponent<SettingsUI>();
            scSettingUI.OpenSettings();
        });
    }


    private void checkNodamage()
    {
        GameObject objPlayer = GameObject.Find("Player");
        Player scPlayer = objPlayer.GetComponent<Player>();

        btnNodamage.onClick.AddListener(() =>
        {
            if (isNodamage)
            {
                isNodamage = false;
                TextNodamage.text = "OFF";
                scPlayer.CheckNodamage(false);
                btnNodamage.image.color = Color.red;
            }
            else
            {
                isNodamage = true;
                TextNodamage.text = "ON";
                scPlayer.CheckNodamage(true);
                btnNodamage.image.color = Color.blue;
            }
        });
    }

    public void SaveNodamageInfo()
    {
        if (isNodamage)
        {
            PlayerPrefs.SetString("Nodamage", "true");
        }
        else
        {
            if (PlayerPrefs.HasKey("Nodamage"))
            {
                PlayerPrefs.DeleteKey("Nodamage");
            }
        }
    }
}
