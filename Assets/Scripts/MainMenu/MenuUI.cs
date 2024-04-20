using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("메인메뉴")]
    [SerializeField] GameObject PressAnyBtn;
    TMP_Text anyBtnText;
    [SerializeField] private float BlinkTime = 3f;
    private bool isBlink = false;
    [SerializeField] GameObject MenuBtns;
    private bool isPressAnyKey = false;

    [Header("버튼")]
    [SerializeField] Button btnStart;
    [SerializeField] Button btnContinue;
    [SerializeField] Button btnExit;
    [SerializeField] GameObject oneMore;

    private void Awake()
    {
        PressAnyBtn.SetActive(true);
        MenuBtns.SetActive(false);
        oneMore.SetActive(false);
    }

    private void Start()
    {
        anyBtnText = PressAnyBtn.GetComponent<TMP_Text>();

        pressMainButtons();
    }

    void Update()
    {
        blinkText();

        pressAnyButtons();
        
    }

    private void blinkText()
    {
        if (isBlink == true)
        {
            anyBtnText.color += new Color(0, 0, 0, 1) * Time.deltaTime / BlinkTime;
            if (anyBtnText.color.a > 1f)
            {
                isBlink = false;
            }
        }

        if (isBlink == false)//알파값을 빼서 투명화
        {
            anyBtnText.color -= new Color(0, 0, 0, 1) * Time.deltaTime / BlinkTime;
            if (anyBtnText.color.a < 0f)
            {
                isBlink = true;
            }
        }
    }

    private void pressAnyButtons()
    {
        if (isPressAnyKey == true) return;

        if (Input.anyKeyDown)
        {
            PressAnyBtn.SetActive(false);
            MenuBtns.SetActive(true);
            isPressAnyKey = true;
        }
    }

    private void pressMainButtons()
    {
        btnStart.onClick.AddListener(() => 
        {
            SceneManager.LoadSceneAsync(1);
        });
        
        btnContinue.onClick.AddListener(() => 
        {

        });

        btnExit.onClick.AddListener(() =>
        {
            oneMore.SetActive(true);
        });
    }

    public void checkExit()
    {

    }
}
