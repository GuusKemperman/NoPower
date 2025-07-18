using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField]
    GameObject target;


    Vector3 desiredDelta = default;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform myTransform = transform;
        Transform otherTransform = target.transform;

        desiredDelta = myTransform.position - otherTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 otherPos = target.transform.position;
        Vector3 desiredPos = otherPos + desiredDelta;

        transform.position = desiredPos;
    }
}
