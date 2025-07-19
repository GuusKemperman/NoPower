using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurretFalling : MonoBehaviour
{
    float vel = 20;

    float acc = 50;

    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField]
    AudioSource impactSource;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 0)
        {
            vel += acc * Time.deltaTime;
            transform.Translate(new Vector3(0, -vel * Time.deltaTime, 0));
        }

        if (transform.position.y <= 0)
        {
            transform.position = (new Vector3(transform.position.x, 0, transform.position.z));
            gameObject.GetComponent<TurretShooting>().active = true;

            impactSource.clip = audioClips[Random.Range(0, audioClips.Count)];
            impactSource.Play();
           
            Destroy(this);
        }
    }
}
