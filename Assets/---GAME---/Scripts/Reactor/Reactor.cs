using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


public class Reactor : BaseInteractable
{

    [Inject] private PowerManager powerManager = null;

    [SerializeField]
    public float maxAmountOfPower = 100.0f;

    public float currentAmountOfPower;

    [SerializeField]
    float rechargeTime = 5.0f;

    [SerializeField]
    List<Light> reactorLights = new List<Light>();

    private List<float> intensities = new List<float>();
    
    [SerializeField]
    AudioSource hummingSource;

    [SerializeField]
    AudioSource interactSource;

    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] private List<GameObject> toDisableOnDeplete = new List<GameObject>();

    public override void Interact() 
    {
        if (CanDeplete())
        {
            OnDeplete();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reactorLights = GetComponentsInChildren<Light>().ToList();
        
        foreach (Light light in reactorLights)
        {
            intensities.Add(light.intensity);
        }

        foreach (GameObject go in toDisableOnDeplete)
        {
            go.SetActive(true);
        }
        
        currentAmountOfPower = maxAmountOfPower;
        powerManager = FindAnyObjectByType<PowerManager>();
        Assert.IsNotNull(powerManager);
    }

    public bool CanDeplete()
    {
        return currentAmountOfPower == maxAmountOfPower;
    }

    [ContextMenu("OnDeplete")]
    public void OnDeplete()
    {
        Assert.IsTrue(CanDeplete());

        interactSource.Play();

        powerManager.ChangePower((int)currentAmountOfPower);

        currentAmountOfPower = 0;
        StartCoroutine(Recharge());

        foreach (Light light in reactorLights)
        {
            light.intensity = 0.0f;
        }
        
        foreach (GameObject go in toDisableOnDeplete)
        {
            go.SetActive(false);
        }

        interactSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        interactSource.Play();
    }

    IEnumerator Recharge()
    {
        float amountPerSecond = maxAmountOfPower / rechargeTime;

        while (currentAmountOfPower < maxAmountOfPower)
        {
            currentAmountOfPower += amountPerSecond * Time.deltaTime;
            hummingSource.volume = currentAmountOfPower / maxAmountOfPower;

            yield return new WaitForEndOfFrame();
        }

        foreach (GameObject go in toDisableOnDeplete)
        {
            go.SetActive(true);
        }
        
        hummingSource.volume = 1;

        for (int i = 0; i < reactorLights.Count; i++)
        {
            reactorLights[i].intensity = intensities[i];
        }

        currentAmountOfPower = maxAmountOfPower;
        rechargeTime *= 2;
    }
}
