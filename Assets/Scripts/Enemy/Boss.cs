using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] GameObject UI;

    public override void Start()
    {
        base.Start();
        
    }

    public override bool GetDamage(float _damage)
    {
        BossUI scUI = UI.GetComponent<BossUI>();
        scUI.SetBossHp(curHp,maxHp);
        return base.GetDamage(_damage);
        
    }
}
