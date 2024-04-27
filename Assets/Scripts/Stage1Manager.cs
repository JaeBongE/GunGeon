using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    [Header("���� ����� ��")]
    [SerializeField] GameObject wall;
    [SerializeField] float maxY;
    private bool isGetGun = false;

    [Header("���� óġ�Ǿ��� ��")]
    [SerializeField] GameObject wall2;
    [SerializeField] float maxX;
    [SerializeField] GameObject checkEnemy;
    [SerializeField] private bool isDeathEnemy = false;

    void Update()
    {
        checkGetGun();
        checkDeathEnemy();
        if (Input.GetKeyDown(KeyCode.F))
        {
            LoadingSceneController.Instance.LoadScene("Stage2");
        }
    }

    private void checkGetGun()
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

    private void checkDeathEnemy()
    {
        if (checkEnemy == null)
        {
            isDeathEnemy = true;
        }

        if (isDeathEnemy == true)
        {
            if (wall2 == null) return;

            float wallX = wall2.transform.position.x;

            if (wallX > maxX)
            {
                wallX = maxX;
                //Destroy(wall);
            }

            //���� ���� �̵��ϸ鼭 ���� ������
            wallX += Time.deltaTime;
            wall2.transform.position = new Vector3(wallX, wall2.transform.position.y, wall2.transform.position.z);
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
