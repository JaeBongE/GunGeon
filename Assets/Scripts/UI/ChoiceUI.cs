using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceUI : MonoBehaviour
{
    [SerializeField] Button btnPistol;
    [SerializeField] Button btnRifle;
    [SerializeField] GameObject stage2Manager;

    void Start()
    {
        selectButton();
    }

    private void selectButton()
    {
        btnPistol.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });

        btnRifle.onClick.AddListener(() =>
        {
            GameObject player = GameObject.Find("Player");
            Player scPlayer = player.GetComponent<Player>();
            scPlayer.ChangeGun(true);

            Stage2Manager sc = stage2Manager.GetComponent<Stage2Manager>();
            sc.ChangeGun();
            Destroy(gameObject);
        });
    }
}
