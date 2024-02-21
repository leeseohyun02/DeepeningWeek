using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float leftBorder = -5f + transform.localScale.x / 2f;
        float rightBorder = 5f - transform.localScale.x / 2f;

        if(mousePos.x < leftBorder)
        {
            mousePos.x = leftBorder;
        }
        else if(mousePos.x > rightBorder)
        {
            mousePos.x = rightBorder;
        }
        mousePos.y = 8;
        mousePos.z = 0;
        transform.position = Vector3.Lerp(transform.position,mousePos,0.2f);
    }
}
