using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject map;
    public GameObject captain;
    public GameObject ship;
    public GameObject speechBubble;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject fader;
    public Text speech;

    public AudioSource hello;
    public AudioSource greatjob;
    public AudioSource mayday;
    public AudioSource song;

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
    private Animation faderAnimator;
    private bool playingSound = false;


    public Phase phase = Phase.ShowCaptain;

    void Start()
    {
        captainAnimator = captain.GetComponent<Animator>();
        mapAnimator = map.GetComponent<Animator>();
        shipAnimator = ship.GetComponent<Animator>();
        faderAnimator = fader.GetComponent<Animation>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseClicked = true;
        }

        if (phase == Phase.ShowCaptain)
        {
            if (timer > 5)
            {
                speechBubble.SetActive(false);
            }
            else if (timer > 2)
            {
                speechBubble.SetActive(true);
                if (!playingSound)
                    hello.Play();
                playingSound = true;
                speech.text = "Ahoy sailor!";
            }
            
            if (!phaseStarted)
            {
                captainAnimator.SetTrigger("MoveIn");
                phaseStarted = true;
                playingSound = false;
                faderAnimator.Play("FadeIn");
                timer = 0;
            }
            else if(mouseClicked || timer > 8)
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

        if (phase == Phase.Fail)
        {
            if (!phaseStarted)
            {
                phaseStarted = true;
                playingSound = false;
                timer = 0;
            }
            else if (timer > 9)
            {
                loseScreen.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
            else if (timer > 8.5)
            {
                faderAnimator.Play("FadeOut");
            }
            else if (timer > 4)
            {
                loseScreen.SetActive(true);
            }
            else if (timer > 1)
            {
                if(!playingSound)
                    mayday.Play();
                playingSound = true;
            }
        }

        if (phase == Phase.Victory)
        {

            if (!phaseStarted)
            {
                phaseStarted = true;
                timer = 0;
                winScreen.SetActive(true);
                playingSound = false;
                song.Play();
            }

            if (timer > 6)
            {
                captainAnimator.SetTrigger("MoveOut");
            }
            else if (timer > 5)
            {
                speechBubble.SetActive(false);
                winScreen.SetActive(false);
            }
            else if (timer > 2)
            {
                speechBubble.SetActive(true);
                speech.text = "Great job!";
                if (!playingSound)
                    greatjob.Play();
                playingSound = true;
            }
        }

        timer += Time.deltaTime;
    }

    public void Lose()
    {
        phase = Phase.Fail;
        shipAnimator.SetTrigger("Sink");
        ship.GetComponent<AudioSource>().Play();
        captainAnimator.SetTrigger("MoveIn");
        captainAnimator.SetTrigger("Drown");
    }

    public void Win()
    {
        phase = Phase.Victory;
        captainAnimator.SetTrigger("MoveIn");
    }
}
