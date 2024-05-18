using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] private GameObject bossHpBalckBar;
    [SerializeField] private Image bossHpBar;

    private void Start()
    {
        bossHpBalckBar.SetActive(false);
    }

    public void SetBossHp(float _curHp, float _maxHp)
    {
        bossHpBar.fillAmount = _curHp / _maxHp;
    }

    public void CheckCutScene()
    {
        bossHpBalckBar.SetActive(true);
    }
}