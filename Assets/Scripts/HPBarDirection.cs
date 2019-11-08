using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarDirection : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyCanvasをMain Cameraに向かせる
        canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
