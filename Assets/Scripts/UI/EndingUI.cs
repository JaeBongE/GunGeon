using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    [SerializeField] Button btnMain;
    [SerializeField] GameObject BackGround;

    private void Awake()
    {
        BackGround.SetActive(false);
    }

    void Start()
    {
        goMain();
    }

    private void goMain()
    {
        btnMain.onClick.AddListener(() =>
        {
            BackGround.SetActive(true);
        });
    }
}
