using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject portalUi;
    CircleCollider2D coll;
    public enum toStage
    {
        Stage1,
        Stage2,
        Boss,
    }

    public toStage stage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())
        {
            portalUi.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())
        {
            portalUi.SetActive(false);
        }
    }

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        portalUi.SetActive(false);
    }

    void Update()
    {
        checkPlayer();
    }

    private void checkPlayer()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D playerColl = Physics2D.OverlapBox(coll.bounds.center, coll.bounds.size, 0, LayerMask.GetMask("PlayerHitBox"));
            if (playerColl != null)
            {
                GameManager.Instance.SaveInfor();
                GameObject objPlayerUI = GameObject.Find("PlayerUI");
                PlayerUI scPlayerUI = objPlayerUI.GetComponent<PlayerUI>();
                scPlayerUI.SaveNodamageInfo();
                LoadingSceneController.Instance.LoadScene(stage.ToString());

                //if (stage == toStage.Stage2)
                //{
                //    GameObject objStage1Manager = GameObject.Find("Stage1Manager");
                //    Stage1Manager sc = objStage1Manager.GetComponent<Stage1Manager>();
                //    sc.OpenChoiceUI();
                //}


                //if (GameObject.Find("ChoiceUI") == null)
                //{
                //    LoadingSceneController.Instance.LoadScene(stage.ToString());
                //}
            }
        }
    }
}
