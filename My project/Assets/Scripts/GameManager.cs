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


    public int maxLevel;

    private void Awake()
    {   //프레임 설정
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        NextCircle();
    }

    Circle GetCircle()
    {
        //이펙트 생성
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();

        //오브젝트 생성
        GameObject instantCircleObj = Instantiate(circlePrefab, circleGroup);
        Circle instantCircle = instantCircleObj.GetComponent<Circle>();

        //오브젝트 생성시 바로 이펙트 변수를 생성했던 것으로 초기화
        instantCircle.effect = instantEffect;

        return instantCircle;
    }

    private void NextCircle()
    {
        Circle newCircle = GetCircle();
        lastCircle = newCircle;
        lastCircle.gameManager = this; //초기화

        lastCircle.level = Random.Range(0, maxLevel);
        lastCircle.gameObject.SetActive(true);

        StartCoroutine(WaitNext());
    }

    //코루틴
    IEnumerator WaitNext()
    {
        while (lastCircle != null) // while에서 yield를 쓰지 않으면 무한루프
        {
            yield return null;
        }
        // 오브젝트가 없으면 2.5초 뒤에 생성되도록
        yield return new WaitForSeconds(2.5f);

        NextCircle();
    }

    public void TouchDown()
    {
        if(lastCircle == null) // null이면 반환
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
        lastCircle = null; // 떨어뜨리면 null
    }
}
