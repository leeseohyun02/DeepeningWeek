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
        GetCircle();
    }

    public void TouchDown()
    {
        lastCircle.Drag();
    }

    public void TouchUp()
    {
        lastCircle.Drop();
    }
}
