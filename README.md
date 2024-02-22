# 수박 게임 - 개인 프로젝트
 
### 📆 제작기간   

2024.2.21 ~ 2024.2.22
[유튜브 '골드메탈' 님의 강의로 제작]

----

### ✅ 기능

1. 오브젝트 드래그앤 드랍
2. 오브젝트 레벨
3. 오브젝트 흡수 및 레벨업
4. 경계선 이벤트
5. 오브젝트 풀링
6. 점수 시스템

----

### ✅ 기능 설명


오브젝트 드래그앤 드랍 - 마우스로 오브젝트를 드래그해서 원하는 위치에 떨어뜨릴 수 있다.

![image](https://github.com/leeseohyun02/DeepeningWeek/assets/78461967/173405fe-9e28-4daa-8f1b-1a394cf0dda3)
![image](https://github.com/leeseohyun02/DeepeningWeek/assets/78461967/04b4fb08-e88f-4c92-8654-bf2eb0882fe1)

오브젝트 레벨 - 0 ~ 7단계의 레벨로 구성되어 있으며 랜덤으로 나온다.

![image](https://github.com/leeseohyun02/DeepeningWeek/assets/78461967/c823a0fb-f076-449c-aabd-0bf47bcd4c98)


오브젝트 흡수 및 레벨업 - 같은 오브젝트와 충돌하면 흡수되어 레벨업이 된다.

![image](https://github.com/leeseohyun02/DeepeningWeek/assets/78461967/4dd83338-06e0-4726-a182-dac4849b26f7)

경계선 이벤트 - 경계선에 일정시간동안 닿으면 오브젝트의 색이 변하고 사라지게 된다.

![image](https://github.com/leeseohyun02/DeepeningWeek/assets/78461967/d72cc49a-960c-403e-9aff-afaddcf195e2)
![image](https://github.com/leeseohyun02/DeepeningWeek/assets/78461967/31e38ff9-9f1e-4be0-9806-4b46015acdfe)  

오브젝트 풀링 - 정해진 오브젝트의 갯수만큼 사용하되 모두 다 사용중일 경우에만 새로 하나 생성한다.
![image](https://github.com/leeseohyun02/DeepeningWeek/assets/78461967/11686597-711d-4e4b-8e0f-298def7cc071)
![image](https://github.com/leeseohyun02/DeepeningWeek/assets/78461967/7ec0687b-170a-4c53-b785-a3766d813f6f)  

점수 시스템 - 현재 점수, 최고 점수를 PlayerPrefs로 저장하여 불러온다.  




----

### ❗트러블 슈팅

문제 : 1. 같은 오브젝트끼리 흡수를 하지 못하는 현상


시도 및 해결 :  my의 변수에는 자신의 위치를 넣고, other에는 other의 위치를 넣어줘야 했음 

```
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
```

문제 : 2. 게임 종료시에도 오브젝트가 생성되는 현상   

시도 및 해결 : 오브젝트를 생성하는 코루틴이 계속해서 돌고 있었기 때문에 게임 오버시 if(isOver) return을 사용하여 코루틴이 돌지 않겠끔 에외처리 해줌

```
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
```








   

 
