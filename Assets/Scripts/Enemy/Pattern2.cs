using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2 : MonoBehaviour
{
    [SerializeField] List<GameObject> listMuzzle = new List<GameObject>();
    [SerializeField] List<GameObject> listExplosion = new List<GameObject>();
    AudioSource audioPattern2;

    private bool isStart = false;
    private float timer = 3f;
    private float Limittimer = 3f;

    private bool doExplosion = false;

    private void Awake()
    {
        audioPattern2 = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //Muzzle1.SetActive(false);
        //Muzzle2.SetActive(false);
        //Muzzle3.SetActive(false);
        //Muzzle4.SetActive(false);
        //Muzzle5.SetActive(false);

        for (int iNum = 0; iNum < listMuzzle.Count; iNum ++)
        {
            listMuzzle[iNum].SetActive(false);
        }
    }

    private void Update()
    {
        checkTImer();

        if (doExplosion)
        {
            for (int iNum = 0; iNum < listMuzzle.Count; iNum++)
            {
                listMuzzle[iNum].SetActive(false);
            }
            doExplosion = false;
        }
    }

    private void checkTImer()
    {
        if (isStart == true)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                if (timer < 0f)
                {
                    timer = Limittimer;

                    isStart = false;

                    for (int jNum = 0; jNum < listExplosion.Count; jNum ++)
                    {
                        listExplosion[jNum].SetActive(true);
                        audioPattern2.PlayOneShot(audioPattern2.clip);
                    }

                    //for (int iNum = 0; iNum < listMuzzle.Count; iNum++)
                    //{
                    //    listMuzzle[iNum].SetActive(false);
                    //}
                }
            }
        }

    }

    public void DoPattern2()
    {
        isStart = true;

        for (int iNum = 0; iNum < listMuzzle.Count; iNum++)
        {
            listMuzzle[iNum].SetActive(true);
        }

    }

    public void CheckExplosion(bool _doExplosion)
    {
        doExplosion = _doExplosion;
    }
}
