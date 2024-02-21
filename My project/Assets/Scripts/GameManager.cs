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
