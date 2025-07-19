using UnityEngine;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class enemy_behaviour : MonoBehaviour
{
    public Transform player;
    public GameObject playerOb;
    UnityEngine.AI.NavMeshAgent agent;
    public GameObject enemyMeshObject;
    public float AttackRadius = 10;


    [SerializeField] float damageToPlayer = 3;

    private enum State
    {
        Walking,
        Attacking
    }

    public float AttackInterval = 100;
    private float AttackTimer = 0;
    private State currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < AttackRadius)
        {
            currentState = State.Attacking;
        }
        else
        {
            currentState = State.Walking;
         
        }

        //update Attack timer
        AttackTimer = Mathf.Max(0, AttackTimer - Time.deltaTime);


        if (player != null)
        {
            switch (currentState)
        {
            case State.Walking:
               
                    agent.SetDestination(player.position);
                break;

            case State.Attacking:
                Attack();
                break;
        }
        }

    }

    void Attack()
    {
        agent.SetDestination(transform.position + ((player.position- transform.position) * 0.1f));
        if(AttackTimer <= 0.01)
        {
            enemyMeshObject.GetComponent<Animator>().SetTrigger("Attack");
            AttackTimer = AttackInterval;
            player.GetComponent<PlayerHealth>().ChangeHealth(-3);
            Debug.Log("Attacked");
        }
    }


}
