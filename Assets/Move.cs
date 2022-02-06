using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool sinking = false;
    public GameController gameController;
    public GameObject captain;
    // Update is called once per frame
    void Update()
    {
        if (gameController.phase == GameController.Phase.Fail)
        {
            return;
        }
        if (gameController.phase == GameController.Phase.Victory)
        {
            transform.position += transform.forward * -Time.deltaTime * 1f;
            return;
        }

        if(Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * -Time.deltaTime * .5f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * Time.deltaTime * .5f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -Time.deltaTime * 15);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 15);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Rock")
        { 
            gameController.Lose();
        } else if (other.gameObject.tag == "Finish")
        {
            gameController.Win();
        }
    }
}
