using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject fader;
    private Animation faderAnimator;

    bool startGame = false;
    bool fadedIn = false;

    float timer = 0;

    void Start()
    {
        faderAnimator = fader.GetComponent<Animation>();
    }

    void Update()
    {
        if(startGame && timer >= 0.5)
        {
            SceneManager.LoadScene(1);
        }

        if (!fadedIn && timer >= 0.5)
        {
            faderAnimator.Play("FadeIn");
            fadedIn = true;
        }

        timer += Time.deltaTime;

    }

    public void Play()
    {
        faderAnimator.Play("FadeOut");
        startGame = true;
        timer = 0;
    }
}
