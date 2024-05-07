using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("���θ޴�")]
    [SerializeField] GameObject PressAnyBtn;
    TMP_Text anyBtnText;
    [SerializeField] private float BlinkTime = 3f;
    private bool isBlink = false;
    [SerializeField] GameObject MenuBtns;
    private bool isPressAnyKey = false;
    [SerializeField] GameObject Textdata;

    [Header("��ư")]
    [SerializeField] Button btnStart;
    [SerializeField] Button btnContinue;
    [SerializeField] Button btnExit;
    [SerializeField] GameObject oneMore;
    [SerializeField] GameObject clear;

    private void Awake()
    {
        PressAnyBtn.SetActive(true);
        MenuBtns.SetActive(false);
        oneMore.SetActive(false);
        Textdata.SetActive(false);
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

        if (isBlink == false)//���İ��� ���� ����ȭ
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
        //���� ���۽� Ű �� �����
        btnStart.onClick.AddListener(() => 
        {
            if (PlayerPrefs.HasKey("Continue") == true)
            {
                //ó������ �ٽ� �� ������ �����
                clear.SetActive(true);
            }
            else
            {
                clear.SetActive(false);
            }

            //PlayerPrefs.DeleteAll();
            //SceneManager.LoadSceneAsync(1);
            //LoadingSceneController.Instance.LoadScene("Stage1");
            //Destroy(gameObject);
            //LoadingBar.LoadScene("Stage1");
        });
        
        btnContinue.onClick.AddListener(() => 
        {
            if (PlayerPrefs.HasKey("Continue") == false)
            {
                Textdata.SetActive(true);
                Invoke("outText", 0.5f);
            }
            else
            {
                LoadingSceneController.Instance.Continue(true);
                string loadScene = PlayerPrefs.GetString("Continue");
                LoadingSceneController.Instance.LoadScene(loadScene);
                Destroy(gameObject);
            }
        });

        btnExit.onClick.AddListener(() =>
        {
            oneMore.SetActive(true);
        });
    }

    private void outText()
    {
        Textdata.SetActive(false);
    }
}
