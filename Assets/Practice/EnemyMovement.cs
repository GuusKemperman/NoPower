using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 7f;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().GameObject();
    } 

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }

        transform.LookAt(player.transform.position);
        
        Vector3 delta = player.transform.position - transform.position;
        transform.position += delta.normalized * Time.deltaTime * speed;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //if (collision.gameObject == player)
    //    //{
    //    //    // Deal damage
    //    //    Destroy(player);
    //    //}
    //    if (other.gameObject.CompareTag("projectile"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
