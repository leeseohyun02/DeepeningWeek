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
    }
    private void Start()
    {
        bgm.Play();
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
        if (isOver) // ������ �����Ǿ �ٽ� �������� �ʰ� (waitNext �ڷ�ƾ�� ���� �ʰԲ�)
        {
            return;
        }
        Circle newCircle = GetCircle();
        lastCircle = newCircle;
        lastCircle.gameManager = this; //�ʱ�ȭ

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
