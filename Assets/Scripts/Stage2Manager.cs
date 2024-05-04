using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : MonoBehaviour
{
    GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
        Player scPlayer = player.GetComponent<Player>();
        scPlayer.GetGun(Player.typeGun.Pistol);
    }
}
