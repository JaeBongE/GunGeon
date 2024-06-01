using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2Muzzle : MonoBehaviour
{
    private float BlinkTime = 0.1f;
    private bool isBlink = false;
    SpriteRenderer spr;

    private bool doExplosion = false;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (doExplosion)
        {
            spr.color = new Color(255, 0, 0, 0);
            doExplosion = false;
        }

        if (isBlink == true)
        {
            spr.color += new Color(0, 0, 0, 0.2f) * Time.deltaTime / BlinkTime;
            if (spr.color.a > 0.2f)
            {
                isBlink = false;
            }
        }

        if (isBlink == false)//알파값을 빼서 투명화
        {
            spr.color -= new Color(0, 0, 0, 0.2f) * Time.deltaTime / BlinkTime;
            if (spr.color.a < 0f)
            {
                isBlink = true;
            }
        }
    }

    public void CheckColor(bool _doExplosion)
    {
        doExplosion = _doExplosion;
    }
}
