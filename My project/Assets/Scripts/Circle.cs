using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public GameManager gameManager;
    public ParticleSystem effect;

    public int level;
    private bool isDrag;
    private bool isMerge; //�������ִ� ����

    private Rigidbody2D _rigidbody;
    private CircleCollider2D circleCollider;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float deadTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        anim.SetInteger("Level", level);
    }
    // Update is called once per frame
    void Update()
    {
        if (isDrag)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float leftBorder = -5f + transform.localScale.x / 2f;
            float rightBorder = 5f - transform.localScale.x / 2f;

            if (mousePos.x < leftBorder)
            {
                mousePos.x = leftBorder;
            }
            else if (mousePos.x > rightBorder)
            {
                mousePos.x = rightBorder;
            }
            mousePos.y = 8;
            mousePos.z = 0;
            transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f);
        }
        
    }

    public void Drag()
    {
        isDrag = true;
    }

    public void Drop()
    {
        isDrag = false;
        _rigidbody.simulated = true; // �ν����Ϳ��� false ������ simulated�� true��
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Circle")
        {
            Circle other = collision.gameObject.GetComponent<Circle>(); // ������Ʈ ���� ��������

            if(level == other.level && !isMerge && !other.isMerge && level<7)
            {
                float myX = transform.position.x;
                float myY = transform.position.y;
                float otherX = other.transform.position.x; //other�� ��ġ�� �����;���
                float otherY = other.transform.position.y;
                
                //���� ������Ʈ�� �Ʒ��ְų�, ���� ��ġ�� ���� �� ���� ������Ʈ�� �����ʿ� ���� ���
                if(myY < otherY || (myY == otherY && myX > otherX))
                {
                    other.Hide(transform.position); //�� �������� ������ �����̸鼭 ���������ϹǷ� �� ��ġ�� �˾ƾ���

                    LevelUp();
                }
            }
        }
    }

    private void Hide(Vector2 targetPos)
    {
        isMerge = true;

        _rigidbody.simulated = false;
        circleCollider.enabled = false;

        StartCoroutine(HideRoutine(targetPos)); // ������ ȣ��
    }

    IEnumerator HideRoutine(Vector2 targetPos)
    {
        int frameCount = 0;

        while(frameCount < 20) // �����Ӵ�
        {
            frameCount++;
            //�� �ڽ� ��ġ, ��ǥ ����, �̵� ���� (�ε巯�� ������)
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            yield return null;
        }
        gameManager.score += (int)Mathf.Pow(2, level); //������ �������� ���� �߰�

        isMerge = false;
        gameObject.SetActive(false); //�������� ������Ʈ ��Ȱ��ȭ
       
    }

    private void LevelUp()
    {
        isMerge = true;

        _rigidbody.velocity = Vector2.zero; //�̵��ӵ� ���� (�����ӵ�)
        _rigidbody.angularVelocity = 0; //ȸ���ӵ��� ����

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1);
        EffectPlay();

        yield return new WaitForSeconds(0.3f);

        level++;

        //�ΰ��� ���ڰ��߿��� �ִ밪�� ��ȯ
        gameManager.maxLevel = Mathf.Max(level, gameManager.maxLevel);

        isMerge = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Finish") // ��輱�� ����� ��
        {
            deadTime += Time.deltaTime;

            if(deadTime > 2)
            {
                spriteRenderer.color = Color.red;
            }
            if(deadTime > 5)
            {
                gameManager.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Finish") // ��輱���κ��� ������ ��
        {
            deadTime = 0;
            spriteRenderer.color = Color.white;
        }
    }

    private void EffectPlay()
    {
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale; // �������� ����Ʈ ũ�⵵ �޶����
        effect.Play();
    }
}
