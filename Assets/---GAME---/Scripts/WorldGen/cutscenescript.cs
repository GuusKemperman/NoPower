using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using UnityEngine.Playables;

public class cutscenescript : MonoBehaviour
{

    [SerializeField] private GameObject player;

    [SerializeField] private List<GameObject> toDisable;

    [SerializeField] private GameObject musicManager;


    [SerializeField] private GameObject UI;

    [SerializeField] private GameObject FadeFromBlack;



    private CanvasGroup canvas;


    [SerializeField] private PlayableDirector director;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        canvas = UI.GetComponent<CanvasGroup>();

        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(FadeFromBlackF());
        

        AudioSource audioSource = musicManager.GetComponent<AudioSource>();
        float delayInSeconds = 2.0f;
        audioSource.PlayDelayed(0);

        foreach (GameObject ob in toDisable)
        {
            ob.SetActive(false);
        }

        canvas.alpha = 0;

        director.Play();
        StartCoroutine(SetPlayerActive());
    }


    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            canvas.alpha = Mathf.Clamp01(elapsed / 0.5f);

            yield return null;
        }

        canvas.alpha = 1; // Ensure fully visible at end
    }


    IEnumerator FadeFromBlackF()
    {

        CanvasGroup fadeFromBlackCanvas= FadeFromBlack.GetComponent<CanvasGroup>();



        float elapsed = 0f;
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime;
            fadeFromBlackCanvas.alpha = Mathf.Clamp01(1-(elapsed / 1.0f));

            yield return null;
        }

        fadeFromBlackCanvas.alpha = 0; // Ensure fully visible at end
    }


    IEnumerator SetPlayerActive()
    {
        yield return new WaitForSeconds(2.5f);
        player.GetComponent<PlayerMovement>().enabled = true;
        StartCoroutine(FadeIn());

        foreach (GameObject ob in toDisable)
        {
            ob.SetActive(true);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
