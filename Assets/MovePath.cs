using UnityEngine;
using Maths;
public class MovePath : MonoBehaviour
{

    private float width;
    private float height;
    public GameController gameController;
    [SerializeField] Vector2 pos;
    [SerializeField] Vector2 touchpos;
    [SerializeField] float pathAccuracyTime = 0.1f;
    private float timer = 0f;

    bool touchStarted = false;
    bool clickStarted = false;



    private void Awake()
    {
        width = (float)Screen.width;
        height = (float)Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.phase == GameController.Phase.Fail
        || gameController.phase == GameController.Phase.Victory)
        {
            GetComponent<Renderer>().enabled = false;
            return;
        }
        if (gameController.phase == GameController.Phase.Play
        && gameController.playPhase == GameController.PlayPhase.DrawRoute)
        {
            GetComponent<Renderer>().enabled = true;
            if (Input.touchCount > 0)
            {
                timer += Time.deltaTime;

                Touch touch = Input.GetTouch(0);
                touchStarted = true;
                // Move the route if the screen has the finger moving.
                if (touch.phase == TouchPhase.Moved)
                {
                    touchpos = touch.position;
                    pos.x = Mathfuncs.Remap(0f, width, 3f, -2.7f, touchpos.x);
                    pos.y = Mathfuncs.Remap(0f, height, 12.7f, -2f, touchpos.y);

                    Vector3 position = new Vector3(pos.x, 1.0f, pos.y);
                    // Position the route.
                    transform.position = position;
                }
                if (timer > pathAccuracyTime)
                {
                    timer = 0f;
                    gameController.path.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
                }
            }

            if (Input.GetMouseButton(0))
            {
                timer += Time.deltaTime;

                Vector2 mousePos = Input.mousePosition;
                clickStarted = true;
                // Move the route
                pos.x = Mathfuncs.Remap(0f, width, 3f, -2.7f, mousePos.x);
                pos.y = Mathfuncs.Remap(0f, height, 12.7f, -2f, mousePos.y);

                Vector3 position = new Vector3(pos.x, 1.0f, pos.y);

                // Position the route.
                transform.position = position;

                if (timer > pathAccuracyTime)
                {
                    timer = 0f;
                    Debug.Log("addpath");
                    gameController.path.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
                }
            }

            if ((touchStarted && Input.touchCount == 0) || (clickStarted && !Input.GetMouseButton(0)))
            {
                Debug.Log("START!");
                Debug.Log(gameController.path.Count);
                touchStarted = false;
                clickStarted = false;
                gameController.playPhase = GameController.PlayPhase.FollowRoute;
                gameController.moveTimer = Time.time;
            }
        }
    }
}
