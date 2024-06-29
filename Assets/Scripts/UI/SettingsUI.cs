using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] GameObject SettingUI;

    private void Start()
    {
        SettingUI.SetActive(false);
    }

    public void OpenSettings()
    {
        SettingUI.SetActive(true);
    }
}
