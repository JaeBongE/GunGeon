using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    private bool isTouchTerminal;
    BoxCollider2D coll;
    [SerializeField] GameObject TerminalUI;
    [SerializeField] bool check = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())
        {
            TerminalUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())
        {
            TerminalUI.SetActive(false);
        }
    }

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        TerminalUI.SetActive(false);
    }

    private void Update()
    {
        checkPlayer();
    }

    private void checkPlayer()
    {
        if (isTouchTerminal) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D playerColl = Physics2D.OverlapBox(coll.bounds.center, coll.bounds.size, 0, LayerMask.GetMask("PlayerHitBox"));
            if (playerColl != null)
            {
                Stage2Manager.Instance.CheckTerminal(true);
                Player scPlayer = playerColl.attachedRigidbody.GetComponent<Player>();
                scPlayer.Heal();
                Stage2Manager.Instance.OpenChoiceUI();

                isTouchTerminal = true;
            }
        }
    }
}
