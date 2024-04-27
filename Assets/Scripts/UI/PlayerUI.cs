using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("ÃÑ")]
    [SerializeField] Image gunUi;
    [SerializeField] TMP_Text gunBulletUi;

    [Header("Ã¼·Â")]
    [SerializeField] Image hp3;
    [SerializeField] Image hp2;
    [SerializeField] Image hp1;

    [Header("´ë½Ã")]
    [SerializeField] Image dashImage;
    [SerializeField] TMP_Text tmpDashCool;

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
}
