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

    int sfxCursor; //���� ����Ű�� ����


    public int score;
    public int maxLevel;
    public bool isOver;

    private void Awake()
    {   //������ ����
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

    Circle MakeCircle() //������Ʈ Ǯ��
    {
        //����Ʈ ����
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        instantEffectObj.name = "Effect" + effectPool.Count;
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        effectPool.Add(instantEffect);

        //������Ʈ ����
        GameObject instantCircleObj = Instantiate(circlePrefab, circleGroup);
        instantCircleObj.name = "Circle" + circlesPool.Count;
        Circle instantCircle = instantCircleObj.GetComponent<Circle>();
        instantCircle.gameManager = this;
        //������Ʈ ������ �ٷ� ����Ʈ ������ �����ߴ� ������ �ʱ�ȭ
        instantCircle.effect = instantEffect;
        circlesPool.Add(instantCircle);

        return instantCircle; // �ڽ��� ���� ������Ʈ ��ȯ
    }

    Circle GetCircle()
    {
        for(int index = 0; index < circlesPool.Count; index++)
        {
            poolCursor = (poolCursor +1) % circlesPool.Count;
            if (!circlesPool[poolCursor].gameObject.activeSelf) //��Ȱ��ȭ�� ��
            {
                return circlesPool[poolCursor];
            }
        }
        return MakeCircle(); //��� ������Ʈ�� Ȱ��ȭ �Ǿ������� ���� ����
    }

    private void NextCircle()
    {
        if (isOver) // ������ �����Ǿ �ٽ� �������� �ʰ� (waitNext �ڷ�ƾ�� ���� �ʰԲ�)
        {
            return;
        }
        lastCircle = GetCircle();
        lastCircle.level = Random.Range(0, maxLevel);
        lastCircle.gameObject.SetActive(true);
        SfxPlay(Sfx.Next);

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

        StartCoroutine(GameOverRoutine());  
    }

    IEnumerator GameOverRoutine() 
    {
        Circle[] circles = FindObjectsOfType<Circle>(); // Circle ��ũ��Ʈ�� �����ϴ� ������Ʈ���� ��� ã�ƿ�

        for (int index = 0; index < circles.Length; index++)
        {
            circles[index]._rigidbody.simulated = false; //���ӿ����� �� �������� �ʵ���(�������� �ʰ�)
            yield return new WaitForSeconds(0.1f);
        }


        //�ϳ��� �����ؼ� �����
        for (int index = 0; index < circles.Length; index++)
        {
            circles[index].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        SfxPlay(Sfx.Over);
    }

    public void SfxPlay(Sfx type) // ȿ���� ���
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
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length; // �迭�� ���̸� ���� �ʵ��� 0,1,2 �ݺ�
    }
}
