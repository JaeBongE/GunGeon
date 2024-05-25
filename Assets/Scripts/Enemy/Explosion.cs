using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Pattern2 scPattern2;
    Pattern2Muzzle scPattern2Muzzle;

    private void Start()
    {
        scPattern2 = GetComponentInParent<Pattern2>();
        scPattern2Muzzle = GetComponentInParent<Pattern2Muzzle>();
    }

    public void DoDestroy()
    {
        scPattern2.CheckExplosion(true);
        scPattern2Muzzle.CheckColor(true);
        gameObject.SetActive(false);
    }
}
