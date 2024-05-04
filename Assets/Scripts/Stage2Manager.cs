using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2Manager : MonoBehaviour
{
    GameObject player;
    [SerializeField] Image gunUI;
    private void Start()
    {
        player = GameObject.Find("Player");
        Player scPlayer = player.GetComponent<Player>();
        scPlayer.GetGun(Player.typeGun.Pistol);

        GameObject objGun = GameObject.Find("LeftHand").transform.GetChild(0).gameObject;
        SpriteRenderer sprGun = objGun.GetComponent<SpriteRenderer>();
        gunUI.sprite = sprGun.sprite;
        gunUI.gameObject.SetActive(true);
    }
}
