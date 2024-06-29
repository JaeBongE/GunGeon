using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsExit : MonoBehaviour
{
    [SerializeField] Button btnExit;


    private void Start()
    {
        btnExit.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

}
