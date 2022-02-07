using UnityEngine;

public class Move : MonoBehaviour
{
    bool sinking = false;

    float moveTime = 5f; // seconds
    public GameController gameController;
    public GameObject captain;

    public GameObject route;

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
        if (gameController.phase == GameController.Phase.Play)
        {
            if (gameController.playPhase == GameController.PlayPhase.DrawRoute)
            {

            }

            if (gameController.playPhase == GameController.PlayPhase.FollowRoute)
            {
                float x = Mathf.Lerp(transform.position.x, route.transform.position.x, (Time.time - gameController.moveTimer) / moveTime);
                float z = Mathf.Lerp(transform.position.z, route.transform.position.z, (Time.time - gameController.moveTimer) / moveTime);
                transform.position = new Vector3(x, 1.0f, z);
            }
            /*
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
            }*/
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
