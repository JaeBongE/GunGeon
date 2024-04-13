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
        if (isGetGun == true)//���� ����ٸ�
        {
            if (wall == null) return;

            float wallY = wall.transform.position.y;

            if (wallY > maxY)
            {
                wallY = maxY;
                //Destroy(wall);
            }

            //���� ���� �̵��ϸ鼭 ���� ������
            wallY += Time.deltaTime;
            wall.transform.position = new Vector3(wall.transform.position.x, wallY, wall.transform.position.z);
        }
    }

    /// <summary>
    /// ��������1���� ���� ������� Ȯ�� �� ����
    /// </summary>
    /// <param name="_isGetGun"></param>
    public void isContinue(bool _isGetGun)
    {
        isGetGun = _isGetGun;
    }
}
