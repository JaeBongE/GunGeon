using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour
{
    [SerializeField] GameObject portalUi;
    CircleCollider2D coll;

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
                LoadingSceneController.Instance.LoadScene("EndingScene");
            }
        }
    }
}
