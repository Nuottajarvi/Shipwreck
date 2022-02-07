using UnityEngine;
using Maths;
public class MovePath : MonoBehaviour
{

    private float width;
    private float height;
    public GameController gameController;
    [SerializeField] Vector2 pos;
    [SerializeField] Vector2 touchpos;

    bool touchStarted = false;

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
                Touch touch = Input.GetTouch(0);
                touchStarted = true;
                // Move the cube if the screen has the finger moving.
                if (touch.phase == TouchPhase.Moved)
                {
                    touchpos = touch.position;
                    pos.x = Mathfuncs.Remap(0f, width, 3f, -2.7f, touchpos.x);
                    pos.y = Mathfuncs.Remap(0f, height, 12.7f, -2f, touchpos.y);

                    Vector3 position = new Vector3(pos.x, 1.0f, pos.y);

                    // Position the cube.
                    transform.position = position;
                }
            }
            if (touchStarted && Input.touchCount == 0)
            {
                Debug.Log("START!");
                touchStarted = false;
                gameController.playPhase = GameController.PlayPhase.FollowRoute;
                gameController.moveTimer = Time.time;
            }
        }
    }
}
