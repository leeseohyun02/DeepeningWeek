using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public GameObject circlePrefab;
    public Transform circleGroup;
    public List<Circle> circlesPool;

    public GameObject effectPrefab;
    public Transform effectGroup;
    public List<ParticleSystem> effectPool;

    [Range(1,30)]
    public int poolSize;
    public int poolCursor;
    public Circle lastCircle;

    public AudioSource bgm;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;

    public enum Sfx
    {
        LevelUp,
        Next,
        Attach,
        Button,
        Over
    };

    int sfxCursor; //현재 가르키는 음원


    public int score;
    public int maxLevel;
    public bool isOver;

    private void Awake()
    {   //프레임 설정
        Application.targetFrameRate = 60;

        circlesPool = new List<Circle>();
        effectPool = new List<ParticleSystem>();
        for(int index = 0; index < poolSize; index++)
        {
            MakeCircle();
        }
    }
    private void Start()
    {
        bgm.Play();
        NextCircle();
      
    }

    Circle MakeCircle() //오브젝트 풀링
    {
        //이펙트 생성
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        instantEffectObj.name = "Effect" + effectPool.Count;
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        effectPool.Add(instantEffect);

        //오브젝트 생성
        GameObject instantCircleObj = Instantiate(circlePrefab, circleGroup);
        instantCircleObj.name = "Circle" + circlesPool.Count;
        Circle instantCircle = instantCircleObj.GetComponent<Circle>();
        instantCircle.gameManager = this;
        //오브젝트 생성시 바로 이펙트 변수를 생성했던 것으로 초기화
        instantCircle.effect = instantEffect;
        circlesPool.Add(instantCircle);

        return instantCircle; // 자신이 만든 오브젝트 반환
    }

    Circle GetCircle()
    {
        for(int index = 0; index < circlesPool.Count; index++)
        {
            poolCursor = (poolCursor +1) % circlesPool.Count;
            if (!circlesPool[poolCursor].gameObject.activeSelf) //비활성화일 때
            {
                return circlesPool[poolCursor];
            }
        }
        return MakeCircle(); //모든 오브젝트가 활성화 되어있으면 새로 생성
    }

    private void NextCircle()
    {
        if (isOver) // 게임이 오버되어도 다시 생성되지 않게 (waitNext 코루틴이 돌지 않게끔)
        {
            return;
        }
        lastCircle = GetCircle();
        lastCircle.level = Random.Range(0, maxLevel);
        lastCircle.gameObject.SetActive(true);
        SfxPlay(Sfx.Next);

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

    public void GameOver()
    {
       if(isOver) // 한번만 실행 되도록
       {
            return;
       }
        isOver = true;

        StartCoroutine(GameOverRoutine());  
    }

    IEnumerator GameOverRoutine() 
    {
        Circle[] circles = FindObjectsOfType<Circle>(); // Circle 스크립트를 포함하는 오브젝트들을 모두 찾아옴

        for (int index = 0; index < circles.Length; index++)
        {
            circles[index]._rigidbody.simulated = false; //게임오버일 때 움직이지 않도록(합쳐지지 않게)
            yield return new WaitForSeconds(0.1f);
        }


        //하나씩 접근해서 지우기
        for (int index = 0; index < circles.Length; index++)
        {
            circles[index].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        SfxPlay(Sfx.Over);
    }

    public void SfxPlay(Sfx type) // 효과음 재생
    {
        switch (type)
        {
            case Sfx.LevelUp:
                sfxPlayer[sfxCursor].clip = sfxClip[Random.Range(0, 3)];
                break;

            case Sfx.Next:
                sfxPlayer[sfxCursor].clip = sfxClip[3];
                break;

            case Sfx.Attach:
                sfxPlayer[sfxCursor].clip = sfxClip[4];
                break;

            case Sfx.Button:
                sfxPlayer[sfxCursor].clip = sfxClip[5];
                break;

            case Sfx.Over:
                sfxPlayer[sfxCursor].clip = sfxClip[6];
                break;
        }

        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length; // 배열의 길이를 넘지 않도록 0,1,2 반복
    }
}
