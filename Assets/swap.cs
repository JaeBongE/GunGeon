using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swap : MonoBehaviour
{

    void Start()
    {
        SpriteRenderer[] sprs = GetComponentsInChildren<SpriteRenderer>();
        int count = sprs.Length;
        foreach (SpriteRenderer spr in sprs)
        {
            GameObject go = spr.gameObject;
            Image img = go.AddComponent<Image>();
            img.sprite = spr.sprite;
            //img.SetNativeSize();
            Destroy(spr);
        }
    }
}
