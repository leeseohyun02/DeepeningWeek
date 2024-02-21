using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Circle lastCircle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
