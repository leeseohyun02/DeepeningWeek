using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Circle lastCircle;
    public GameObject circlePrefab;
    public Transform circleGroup;

    private void Start()
    {
        NextCircle();
    }
    Circle GetCircle()
    {
        GameObject instant = Instantiate(circlePrefab, circleGroup);
        Circle instantCircle = instant.GetComponent<Circle>();
        return instantCircle;
    }

    private void NextCircle()
    {
        Circle newCircle = GetCircle();
        lastCircle = newCircle;

        StartCoroutine(WaitNext());
    }

    //�ڷ�ƾ
    IEnumerator WaitNext()
    {
        while (lastCircle != null) // while���� yield�� ���� ������ ���ѷ���
        {
            yield return null;
        }
        // ������Ʈ�� ������ 2.5�� �ڿ� �����ǵ���
        yield return new WaitForSeconds(2.5f);

        NextCircle();
    }

    public void TouchDown()
    {
        if(lastCircle == null) // null�̸� ��ȯ
        {
            return;
        }

        lastCircle.Drag();
    }

    public void TouchUp()
    {
        if(lastCircle == null)
        {
            return;
        }
        lastCircle.Drop();
        lastCircle = null; // ����߸��� null
    }
}
