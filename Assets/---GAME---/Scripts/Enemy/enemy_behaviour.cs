using UnityEngine;

public class enemy_behaviour : MonoBehaviour
{
    public Transform player; 
    UnityEngine.AI.NavMeshAgent agent;
    public GameObject enemyMeshObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Animation anim = enemyMeshObject.GetComponent<Animation>();
        anim.Play("Walking_A");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

        }
    }
}
