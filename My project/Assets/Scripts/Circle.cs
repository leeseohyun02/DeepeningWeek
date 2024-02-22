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
    private bool isMerge; //합쳐져있는 상태

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
        _rigidbody.simulated = true; // 인스펙터에서 false 상태인 simulated를 true로
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Circle")
        {
            Circle other = collision.gameObject.GetComponent<Circle>(); // 오브젝트 정보 가져오기

            if(level == other.level && !isMerge && !other.isMerge && level<7)
            {
                float myX = transform.position.x;
                float myY = transform.position.y;
                float otherX = other.transform.position.x; //other의 위치를 가져와야함
                float otherY = other.transform.position.y;
                
                //현재 오브젝트가 아래있거나, 같은 위치에 있을 때 현재 오브젝트가 오른쪽에 있을 경우
                if(myY < otherY || (myY == otherY && myX > otherX))
                {
                    other.Hide(transform.position); //내 기준으로 상대방이 움직이면서 합쳐져야하므로 내 위치를 알아야함

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

        StartCoroutine(HideRoutine(targetPos)); // 움직임 호출
    }

    IEnumerator HideRoutine(Vector2 targetPos)
    {
        int frameCount = 0;

        while(frameCount < 20) // 프레임당
        {
            frameCount++;
            //내 자신 위치, 목표 지점, 이동 강도 (부드러운 움직임)
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            yield return null;
        }
        gameManager.score += (int)Mathf.Pow(2, level); //레벨별 제곱으로 점수 추가

        isMerge = false;
        gameObject.SetActive(false); //합쳐지면 오브젝트 비활성화
       
    }

    private void LevelUp()
    {
        isMerge = true;

        _rigidbody.velocity = Vector2.zero; //이동속도 제거 (물리속도)
        _rigidbody.angularVelocity = 0; //회전속도도 제거

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1);
        EffectPlay();

        yield return new WaitForSeconds(0.3f);

        level++;

        //두개의 인자값중에서 최대값을 반환
        gameManager.maxLevel = Mathf.Max(level, gameManager.maxLevel);

        isMerge = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Finish") // 경계선에 닿았을 때
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
        if(collision.tag == "Finish") // 경계선으로부터 떨어질 때
        {
            deadTime = 0;
            spriteRenderer.color = Color.white;
        }
    }

    private void EffectPlay()
    {
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale; // 레벨별로 이펙트 크기도 달라야함
        effect.Play();
    }
}
