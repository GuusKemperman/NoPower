using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voicelinemanager : MonoBehaviour
{
    [SerializeField]
    public int id = 0;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    List<AudioClip> clips = new List<AudioClip>();

    [SerializeField]
    float minCooldown = 5;

    [SerializeField]
    float maxCooldown = 30;

    [SerializeField]
    float chanceToPlay = .8f;

    bool isLocked = false;

    static bool isGlobalLock = false;

    private void Start()
    {
        StartCoroutine(Cooldown());
    }

    public void TryPlay()
    {
        if (isLocked
            || isGlobalLock)
        {
            return;
        }

        if (UnityEngine.Random.Range(0f, 1f) < chanceToPlay)
        {
            source.clip = clips[UnityEngine.Random.Range(0, clips.Count)];
            source.Play();
            StartCoroutine(Cooldown());
            StartCoroutine(GlobalCooldown());
        }
    }

    IEnumerator Cooldown()
    {
        isLocked = true;
        yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
        isLocked = false;
    }

    IEnumerator GlobalCooldown()
    {
        isGlobalLock = true;
        yield return new WaitForSeconds(2.0f);
        isGlobalLock = false;
    }
}
