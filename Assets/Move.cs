using UnityEngine;

public class Move : MonoBehaviour
{
    bool sinking = false;

    public GameController gameController;
    public GameObject captain;

    public GameObject route;
    [SerializeField] int step = 0;
    [SerializeField] float moveTime = 0.2f; // seconds

    public Vector3 stepStartPos;

    private void Awake() {
        stepStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

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
                Vector3 nextPos = gameController.path[step];
                if (nextPos == null)
                {
                    Debug.Log("lista tyhjä");
                    return;
                }
                float time =  (Time.time - gameController.moveTimer) / moveTime;
                float x = Mathf.Lerp(stepStartPos.x, nextPos.x, time);
                float z = Mathf.Lerp(stepStartPos.z, nextPos.z, time);
                transform.position = new Vector3(x, 1.0f, z);
                if (time >= 1.0f)
                {
                    if (gameController.path[step + 1] != null)
                    {
                        step += 1;
                        gameController.moveTimer = Time.time;
                        stepStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        transform.rotation = Quaternion.LookRotation(gameController.path[step] - gameController.path[step + 1]);
                    } else {
                        gameController.playPhase = GameController.PlayPhase.Inactive;
                        // route done
                    }
                } 
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
