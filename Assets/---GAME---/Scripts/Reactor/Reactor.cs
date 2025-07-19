using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


public class Reactor : BaseInteractable
{
    [SerializeField]
    public float maxAmountOfPower = 100.0f;

    public float currentAmountOfPower;

    [SerializeField]
    float rechargeTime = 5.0f;


    [SerializeField]
    Light reactorLight = null;

    float intensityLight = 0;

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
    }

    public bool CanDeplete()
    {
        return currentAmountOfPower == maxAmountOfPower;
    }

    [ContextMenu("OnDeplete")]
    public void OnDeplete()
    {
        Assert.IsTrue(CanDeplete());

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

            Debug.Log(currentAmountOfPower);
            yield return new WaitForEndOfFrame();
        }

        reactorLight.intensity = intensityLight;
        currentAmountOfPower = maxAmountOfPower;
        rechargeTime *= 2;
    }
}
