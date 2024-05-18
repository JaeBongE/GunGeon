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

    CanvasGroup canvas;

    private bool isOpen = false;
    private bool isEnd = false;
    [SerializeField] private float timer = 3;
    private float maxTimer = 0;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0f;
        //B.SetActive(false);
        //O.SetActive(false);
        //S.SetActive(false);
        //S1.SetActive(false);
    }

    private void Update()
    {
        checkTimer();

        EndCutScene();
    }

    private void checkTimer()
    {
        if (isOpen)
        {
            if (timer > 0)
            {
                timer -=Time.deltaTime;
                if (timer < maxTimer )
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
}
