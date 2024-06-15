using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    Vector2 StaffPos;
    [SerializeField] float speed;
    [SerializeField] GameObject Fin;
    CanvasGroup canvas;

    private void Awake()
    {
        StaffPos = transform.localPosition;
    }

    private void Start()
    {
        canvas = Fin.GetComponent<CanvasGroup>();
        canvas.alpha = 0f;
    }

    private void Update()
    {
        StaffPos.x -= speed * Time.deltaTime;
        transform.localPosition = StaffPos;

        if(gameObject.transform.localPosition.x < -1961f)
        {
            gameObject.transform.localPosition = new Vector2(-1961f, gameObject.transform.localPosition.y);
            canvas.alpha += Time.deltaTime;
            if (canvas.alpha > 1f)
            {
                canvas.alpha = 1f;
            }
        }
    }
}
