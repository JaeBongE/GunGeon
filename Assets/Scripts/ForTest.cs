using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTest : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LoadingSceneController.Instance.LoadScene("MainMenu");
        }
    }

}
