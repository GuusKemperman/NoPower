using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
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
    Light reactorLight = null;

    float intensityLight = 0;

    [SerializeField]
    AudioSource hummingSource;

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
        intensityLight = reactorLight.intensity;
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

        powerManager.ChangePower((int)currentAmountOfPower);

        currentAmountOfPower = 0;
        StartCoroutine(Recharge());

        reactorLight.intensity = 0.0f;
    }

    IEnumerator Recharge()
    {
        float amountPerSecond = maxAmountOfPower / rechargeTime;

        while (currentAmountOfPower < maxAmountOfPower)
        {
            currentAmountOfPower += amountPerSecond * Time.deltaTime;
            hummingSource.volume = currentAmountOfPower / maxAmountOfPower;

            Debug.Log(currentAmountOfPower);
            yield return new WaitForEndOfFrame();
        }

        hummingSource.volume = 1;
        reactorLight.intensity = intensityLight;
        currentAmountOfPower = maxAmountOfPower;
        rechargeTime *= 2;
    }
}
