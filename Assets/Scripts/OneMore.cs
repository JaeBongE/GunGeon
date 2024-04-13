using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneMore : MonoBehaviour
{
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;

    void Start()
    {
        Goyes();
        GoNo();
    }

    void Update()
    {
        
    }

    private void Goyes()
    {

    }

    private void GoNo()
    {
        btnNo.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
