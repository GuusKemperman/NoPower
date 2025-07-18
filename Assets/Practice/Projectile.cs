using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 20;

    [SerializeField]
    float timeAliveRemaining = 30f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;

        timeAliveRemaining -= Time.deltaTime;

        if (timeAliveRemaining < 0)
        {
            Destroy(this);
        }
    }
}
