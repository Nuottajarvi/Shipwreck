using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRoute : MonoBehaviour
{
    public GameController gameController;
    void Start()
    {
        
    }

    void Update()
    {
        if(gameController.phase == GameController.Phase.Play)
        {
            //Draw Route
        }
    }
}
