using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 20;

    [SerializeField]
    float timeAliveRemaining = 30f;

    public int Damage = 1;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;

        timeAliveRemaining -= Time.deltaTime;

        if (timeAliveRemaining < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enemy_behaviour enemyBehaviour = other.gameObject.GetComponent<enemy_behaviour>();
        if (!enemyBehaviour) return;
        enemyBehaviour.TakeDamage(Damage);
        Destroy(gameObject);
    }
}
