using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossCutScene : MonoBehaviour
{
    [SerializeField] GameObject B;
    [SerializeField] GameObject O;
    [SerializeField] GameObject S;
    [SerializeField] GameObject S1;
    List<GameObject> listText = new List<GameObject>();

    CanvasGroup canvas;

    private bool isOpen = false;
    private bool isEnd = false;
    [SerializeField] private float timer = 3;
    private float maxTimer = 0;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0f;

        listText.Add(B);
        listText.Add(O);
        listText.Add(S);
        listText.Add(S1);

        B.SetActive(false);
        O.SetActive(false);
        S.SetActive(false);
        S1.SetActive(false);

    }


    private void Update()
    {
        checkTimer();

        EndCutScene();

        showText();

    }

    private void checkTimer()
    {
        if (isOpen)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < maxTimer)
                {
                    timer = maxTimer;
                    isEnd = true;
                }
            }
        }
    }

    public void CheckCutScene()
    {
        canvas.alpha = 1f;
        isOpen = true;
    }

    private void EndCutScene()
    {
        if (isEnd)
        {
            canvas.alpha -= Time.deltaTime;
            if (canvas.alpha < 0f)
            {
                canvas.alpha = 0f;
                //gameObject.SetActive(false);
            }
        }

    }

    private float textTimer = 1f;
    private float textMaxTimer = 0f;
    private bool isShowText = false;
    private int textCount = 1;

    private void showText()
    {
        if (isOpen)
        {
            listText[0].SetActive(true);

            if (textTimer > 0f)
            {
                textTimer -= Time.deltaTime;
                if (textTimer < textMaxTimer)
                {
                    textTimer = textMaxTimer;
                    if (textCount < listText.Count)
                    {
                        listText[textCount].SetActive(true);
                        textCount++;
                        textTimer = 1f;
                    }
                }
            }

            


            //for (int iNum = 0; iNum < 3; iNum++)
            //{
            //    if (isShowText)
            //    {
            //        listText[iNum + 1].SetActive(true);
            //        isShowText = false;
            //        textTimer = 1f;
            //    }
            //}

        }
    }

    //IEnumerator showText()
    //{
    //    if (isOpen)
    //    {
    //        foreach (GameObject obj in listText)
    //        {
    //            obj.SetActive(true);
    //            yield return new WaitForSeconds(1f); // 1ÃÊ ´ë±â
    //        }
    //    }
    //}
}
