using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random; // Required for NavMeshAgent

public enum State
{
    Walking,
    Attacking,
    Dead
}

public class enemy_behaviour : MonoBehaviour
{
    public Transform player;
    public GameObject playerOb;
    public GameObject hitfx;
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject enemyMeshObject;
    public float attackDelay = 0.2f;
    public float AttackRadius = 10;
    public static event Action<enemy_behaviour> EnemyDied;
    
    [SerializeField]
    AudioSource attackVoiceLineAudioSource;

    [SerializeField]
    List<AudioClip> attackAudioClips = new List<AudioClip>();

    [SerializeField]
    AudioSource attackImpactAudioSource;

    [SerializeField]
    List<AudioClip> attackImpactAudioClips = new List<AudioClip>();

    public int Damage = 1;
    public int Health = 1;
    
    [SerializeField] float damageToPlayer = 3;

    public float AttackInterval = 100;
    private float AttackTimer = 0;
    private State currentState;

    public State CurrentState => currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    private void OnDestroy()
    {
        EnemyDied?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < AttackRadius && currentState != State.Dead)
        {
            currentState = State.Attacking;
        }
        else if(distanceToPlayer > AttackRadius && currentState != State.Dead)
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
            case State.Dead:
                Destroy(gameObject,1.0f);
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
            StartCoroutine(DealDamage());

        }
    }


    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(attackDelay);
        if (currentState == State.Attacking)
        {
            player.GetComponent<PlayerHealth>().ChangeHealth(-1 * Damage);
            attackVoiceLineAudioSource.clip = attackAudioClips[Random.Range(0, attackAudioClips.Count)];
            attackVoiceLineAudioSource.Play();

            attackImpactAudioSource.clip = attackImpactAudioClips[Random.Range(0, attackImpactAudioClips.Count)];
            attackImpactAudioSource.Play();
            
            Vector3 hitlocation = transform.position + ((player.position - transform.position) * 0.5f);
            Instantiate(hitfx, hitlocation, transform.rotation);
            Debug.Log("Attacked");
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        foreach (SkinnedMeshRenderer meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            Material material = meshRenderer.material;

            material.SetColor("_FlashColor", Color.white);
            material.SetFloat("_FlashAmount", 0);

            attackImpactAudioSource.clip = attackImpactAudioClips[Random.Range(0, attackImpactAudioClips.Count)];
            attackImpactAudioSource.Play();
            
            Sequence flashSeq = DOTween.Sequence();
            flashSeq.Append(material.DOFloat(1f, "_FlashAmount", 0.12f));
            flashSeq.Append(material.DOFloat(0f, "_FlashAmount", 0.12f));
            flashSeq.OnComplete(() =>
            {
                if (Health <= 0)
                {
                    currentState = State.Dead;
                    agent.enabled = false;
                    enemyMeshObject.GetComponent<Animator>().SetTrigger("Die");
                    enemyMeshObject.GetComponent<SphereCollider>().enabled = false;
                    Destroy(gameObject,1.5f);
                }
            });

        flashSeq.Play();
        }
    }
}



