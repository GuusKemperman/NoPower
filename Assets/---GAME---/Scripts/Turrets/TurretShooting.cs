using System;
using System.Linq;
using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    [SerializeField] private float range = 10.0f;
    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private GameObject shotPoint = null;
    [SerializeField] private int damage = 1;
    
    private TurretTarget closestTarget;
    private float shotTimer = 0.0f;

    public bool active = false;

    private void Awake()
    {
        shotTimer = fireRate;
    }

    private void Update()
    {
        if (!active) return;
        closestTarget = FindClosestTarget();
        HandleAiming();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if(closestTarget == null)return;
        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0.0f)
        {
            Shoot();
            shotTimer = fireRate;
        }
    }

    private void Shoot()
    {
        GameObject spawned =Instantiate(projectile, shotPoint.transform.position, transform.rotation);
        spawned.GetComponent<Projectile>().Damage = damage;
    }

    private void HandleAiming()
    {
        if (closestTarget == null) return;
        Vector3 toTarget = closestTarget.transform.position - transform.position;
        toTarget.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(toTarget);
    }
    
    private TurretTarget FindClosestTarget()
    {
        TurretTarget closest = null;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        Collider[] targets = hits.Where(x => x.GetComponent<TurretTarget>() != null).OrderBy(x=>Vector3.Distance(x.transform.position,transform.position)).ToArray();

        if (targets.Length == 0) return null;
        closest = targets[0].GetComponent<TurretTarget>();
        
        return closest;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.25f);
        Gizmos.DrawSphere(transform.position,range);
    }
}
