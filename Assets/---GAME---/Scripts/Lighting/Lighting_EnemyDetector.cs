using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting_EnemyDetector : MonoBehaviour
{
    List<GameObject> EnemiesInRange = new List<GameObject>();
    public List<GameObject> GetEnemiesInRange()
    {
        return EnemiesInRange;
    }

    public GameObject FindClosestEnemy()
    {
        if (EnemiesInRange.Count > 0)
        {
            GameObject ClosestEnemy = null;
            float ClosestDistanceSquare = -1.0f;
            Vector3 CurrentPosition = transform.position;

            foreach (GameObject CurrentEnemy in EnemiesInRange)
            {
                Vector3 DirectionToEnemy = CurrentEnemy.transform.position - CurrentPosition;
                float CurrentDistanceSquare = DirectionToEnemy.sqrMagnitude;

                if (CurrentDistanceSquare < ClosestDistanceSquare) 
                {
                    ClosestDistanceSquare = CurrentDistanceSquare;
                    ClosestEnemy = CurrentEnemy;
                }
            }
            return ClosestEnemy;
        }
        Debug.Log("WARN: @EnemyDetector, GetClosestEnemy(). No enemies in range.");
        return null;
    }

    private void OnTriggerEnter(Collider OtherCollider)
    {
        if (OtherCollider.CompareTag("enemies")
            && !EnemiesInRange.Contains(OtherCollider.gameObject))
        {
            EnemiesInRange.Add(OtherCollider.gameObject);
        }
    }

    private void OnTriggerExit(Collider OtherCollider)
    {
        if (OtherCollider.CompareTag("enemies")
            && EnemiesInRange.Contains(OtherCollider.gameObject))
        {
            EnemiesInRange.Remove(OtherCollider.gameObject);
        }
    }
}
