using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [SerializeField] float maxY;
    private bool isGetGun = false;

    private void Awake()
    {
        isGetGun = false;
    }

    void Update()
    {
        if (isGetGun == true)//총을 얻었다면
        {
            if (wall == null) return;

            float wallY = wall.transform.position.y;

            if (wallY > maxY)
            {
                wallY = maxY;
                //Destroy(wall);
            }

            //벽이 위로 이동하면서 문이 열린다
            wallY += Time.deltaTime;
            wall.transform.position = new Vector3(wall.transform.position.x, wallY, wall.transform.position.z);
        }
    }

    /// <summary>
    /// 스테이지1에서 총을 얻었는지 확인 후 진행
    /// </summary>
    /// <param name="_isGetGun"></param>
    public void isContinue(bool _isGetGun)
    {
        isGetGun = _isGetGun;
    }
}
