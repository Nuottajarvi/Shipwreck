using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject map;
    public GameObject captain;
    public GameObject ship;
    public GameObject speechBubble;

    public enum Phase
    {
        ShowCaptain,
        ShowMap,
        CaptainWave,
        Play,
        Fail,
        Victory
    }

    private bool phaseStarted = false;
    private float timer = 0;
    private bool mouseClicked = false;
    private Animator captainAnimator;
    private Animator shipAnimator;
    private Animator mapAnimator;


    public Phase phase = Phase.ShowCaptain;

    void Start()
    {
        captainAnimator = captain.GetComponent<Animator>();
        mapAnimator = map.GetComponent<Animator>();
        shipAnimator = ship.GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseClicked = true;
        }

        if (phase == Phase.ShowCaptain)
        {
            if (timer > 2)
            {
                speechBubble.SetActive(true);
            }
            if (timer > 5)
            {
                speechBubble.SetActive(false);
            }
            if (!phaseStarted)
            {
                captainAnimator.SetTrigger("MoveIn");
                phaseStarted = true;
                timer = 0;
            } else if(mouseClicked || timer > 8)
            {
                mouseClicked = false;
                phaseStarted = false;
                speechBubble.SetActive(false);
                phase = Phase.ShowMap;
            }
        }

        if (phase == Phase.ShowMap)
        {
            if (!phaseStarted)
            {
                mapAnimator.SetTrigger("MapIn");
                phaseStarted = true;
                timer = 0;
            }
            else if ((mouseClicked && timer > 1) || timer > 8)
            {
                mapAnimator.SetTrigger("MapOut");
                mouseClicked = false;
                phaseStarted = false;
                phase = Phase.CaptainWave;
            }
        }

        if (phase == Phase.CaptainWave)
        {
            if (!phaseStarted)
            {
                captainAnimator.SetTrigger("Wave");
                phaseStarted = true;
                timer = 0;
            }
            else if (timer > 2)
            {
                phaseStarted = false;
                captainAnimator.SetTrigger("MoveOut");
                phase = Phase.Play;
            }
        }

        timer += Time.deltaTime;
    }

    public void Lose()
    {
        phase = Phase.Fail;
        shipAnimator.SetTrigger("Sink");
        captainAnimator.SetTrigger("MoveIn");
        captainAnimator.SetTrigger("Drown");
    }

    public void Win()
    {
        phase = Phase.Victory;
        captainAnimator.SetTrigger("MoveIn");
    }
}
