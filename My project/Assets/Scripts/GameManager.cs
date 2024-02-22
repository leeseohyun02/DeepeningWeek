using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Circle lastCircle;
    public GameObject circlePrefab;
    public Transform circleGroup;

    public GameObject effectPrefab;
    public Transform effectGroup;

    public int score;
    public int maxLevel;

    public bool isOver;

    private void Awake()
    {   //������ ����
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        NextCircle();
    }

    Circle GetCircle()
    {
        //����Ʈ ����
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();

        //������Ʈ ����
        GameObject instantCircleObj = Instantiate(circlePrefab, circleGroup);
        Circle instantCircle = instantCircleObj.GetComponent<Circle>();

        //������Ʈ ������ �ٷ� ����Ʈ ������ �����ߴ� ������ �ʱ�ȭ
        instantCircle.effect = instantEffect;

        return instantCircle;
    }

    private void NextCircle()
    {
        Circle newCircle = GetCircle();
        lastCircle = newCircle;
        lastCircle.gameManager = this; //�ʱ�ȭ

        lastCircle.level = Random.Range(0, maxLevel);
        lastCircle.gameObject.SetActive(true);

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

    public void GameOver()
    {
       if(isOver) // �ѹ��� ���� �ǵ���
       {
            return;
       }
        isOver = true;
    }
}
