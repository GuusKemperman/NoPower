using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using System.Collections.Generic;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject FadeToBlack;
    


    public void OnClick()
    {
        StartCoroutine(FadeIn());
        StartCoroutine(FadeOutSound());

    }


    IEnumerator FadeIn()
    {
        CanvasGroup fadeToBlackCanvas = FadeToBlack.GetComponent<CanvasGroup>();


        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            fadeToBlackCanvas.alpha = Mathf.Clamp01(elapsed / 0.5f);

            yield return null;
        }

        fadeToBlackCanvas.alpha = 1; // Ensure fully visible at end

        SceneManager.LoadScene(1);
    }


    IEnumerator FadeOutSound()
    {

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            foreach (AudioSource i in allAudioSources){
                i.volume = Mathf.Clamp01(1 - (elapsed / 0.5f));

            }

            yield return null;
        }

        SceneManager.LoadScene(1);
    }



}
