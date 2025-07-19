using System;
using System.Data;
using UnityEngine;

public class RingInReactorScript : MonoBehaviour
{
    [SerializeField]
    float minSpeed = 360.0f * 1;

    [SerializeField]
    float maxSpeed = 360.0f * 5;

    Vector3 rotationPerSecond;

    [SerializeField]
    Reactor reactorComponent = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotationPerSecond = new Vector3(UnityEngine.Random.Range(minSpeed, maxSpeed),
            UnityEngine.Random.Range(minSpeed, maxSpeed),
            UnityEngine.Random.Range(minSpeed, maxSpeed));

        for (int i = 0; i < 3; i++)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                rotationPerSecond[i] *= -1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = rotationPerSecond * Time.deltaTime * 
            (reactorComponent.currentAmountOfPower / reactorComponent.maxAmountOfPower);
     
        transform.rotation *= Quaternion.Euler(rot);    
    }
}
