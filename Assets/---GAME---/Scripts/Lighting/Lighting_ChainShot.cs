using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Lighting_ChainShot : MonoBehaviour
{
    [SerializeField] InputActionAsset ActionMap = null;

    [SerializeField] GameObject LineRendererPrefab = null;

    [SerializeField] private GameObject Particles;

    [SerializeField] float minRayLength = 1.0f;
    [SerializeField] float maxRayLength = 3.0f;

    [SerializeField] float regularDeviationAngle = 15.0f;
    [SerializeField] float splittingDeviationAngle = 45.0f;

    [SerializeField] float splitChancePerLength = .1f;

    [SerializeField] float maxTravelDist = 45.0f;

    [SerializeField] float snapToEnemyRadius = 4.0f;

    [SerializeField] int damageAmount = 10;

    [SerializeField] float lineDuration = 0.4f;

    Queue<GameObject> myQueue = new Queue<GameObject>();
    List<GameObject> spawnedLiners = new List<GameObject>();

    public static event Action HitEnemy;
    
    struct ChainLine
    {
        public ChainLine(Vector2 a_start, Vector2 a_end)
        {
            start = a_start;
            end = a_end;
            hit = null;
        }

        public Vector2 start;
        public Vector2 end;
        public enemy_behaviour hit;
    }

    void Update()
    {
        InputAction AttackAction = ActionMap.FindAction("Attack");
        if (AttackAction.WasPressedThisFrame())
        {
            StartShooting();
        }
    }

    UnityEngine.Vector2 RotateVec2ByAngle(UnityEngine.Vector2 vec, float angle)
    {
        float cosTheta = Mathf.Cos(angle * Mathf.Deg2Rad);
        float sinTheta = Mathf.Sin(angle * Mathf.Deg2Rad);
        return new UnityEngine.Vector2(vec.x * cosTheta - vec.y * sinTheta,
                vec.x * sinTheta + vec.y * cosTheta);
    }

    private void OnDestroy()
    {
        ClearLines();
    }

    IEnumerator ClearLinesAfterDelay()
    {
        yield return new WaitForSeconds(lineDuration);
        StartCoroutine(ClearLines());
    }

    IEnumerator ClearLines()
    {

        while (true)
        {
            for (int i =0; i < 3; i++){ 
                if (myQueue.Count > 0) 
                {
                    Destroy(myQueue.Dequeue());
                }
                else
                {
                    yield break; 
                }
            }

            yield return new WaitForSeconds(0.01f);
        }

    }

    // -----------
    // Utility Functions
    // -----------
    private bool AreAllNonNull(params object[] values) => values.All(v => v != null);
    private bool AreAllTrue(params bool[] values) => values.All(v => v);

    ChainLine GetNewLine(Vector2 prevStart, Vector2 prevEnd, bool isSplitter)
    {
        Vector2 prevDelta = prevEnd - prevStart;
        Vector2 forward = prevDelta.normalized;

        float angleDeviation = isSplitter ? Random.Range(-regularDeviationAngle, regularDeviationAngle)
                : Random.Range(-splittingDeviationAngle, splittingDeviationAngle);

        Vector2 newForward = RotateVec2ByAngle(forward, angleDeviation);

        float length = Random.Range(minRayLength, maxRayLength);

        Vector2 newEnd = prevEnd + newForward * length;
        Vector3 newEnd3D = new Vector3(newEnd.x, 0, newEnd.y);

        Collider[] colliders = Physics.OverlapSphere(newEnd3D, snapToEnemyRadius);
        colliders.OrderBy(x => Vector3.Distance(x.transform.position, newEnd3D));

        enemy_behaviour hit = null;

        foreach (Collider coll in colliders)
        {
            enemy_behaviour enemy = coll.gameObject.GetComponent<enemy_behaviour>();

            if (enemy == null
                || enemy.Health <= 0
                || enemy.transform.position == new Vector3(prevEnd.x, 0, prevEnd.y))
            {
                continue;
            }

            newEnd = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
            
            hit = enemy;
            HitEnemy?.Invoke();
            break;
        }
        var ret = new ChainLine(prevEnd, newEnd);
        ret.hit = hit;
        return ret;
    }

    float Orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        return (q.x - p.x) * (r.y - p.y) - (q.y - p.y) * (r.x - p.x);
    }

    bool AreIntersecting(ChainLine lhs, ChainLine rhs)
    {
        Vector2 A = lhs.start;
        Vector2 B = lhs.end;
        Vector2 C = rhs.start;
        Vector2 D = rhs.end;

        if (A == B || C == D)
            return false;

        float o1 = Orientation(A, B, C);
        float o2 = Orientation(A, B, D);
        float o3 = Orientation(C, D, A);
        float o4 = Orientation(C, D, B);

        if (o1 * o2 < 0 && o3 * o4 < 0)
            return true;

        return false;
    }

    bool IsIntersecting(List<ChainLine> list, ChainLine candidate)
    {
        foreach (ChainLine line in list)
        {
            if (AreIntersecting(line, candidate)) return true;
        }
        return false;
    }

    void DebugDraw(List<ChainLine> list)
    {
        foreach (ChainLine line in list)
        {
            Debug.DrawLine(new Vector3(line.start.x, .1f, line.start.y),
                new Vector3(line.end.x, .1f, line.end.y),
                 Color.blue, 10000.0f, false);
        }
    }

    Quaternion RotationFromTwoPositions(Vector3 startPos, Vector3 endPos)
    {
        Vector3 direction = endPos - startPos;

        // Handle the case where positions are the same to avoid zero direction
        if (direction == Vector3.zero)
            return Quaternion.identity;

        // Create a rotation looking toward the direction vector
        return Quaternion.LookRotation(direction);
    }

    void Draw(List<ChainLine> list)
    {
        foreach (ChainLine line in list)
        {
            Vector3 start = new Vector3(line.start.x, .1f, line.start.y);
            
            Vector3 end = new Vector3(line.end.x, .1f, line.end.y);
            GameObject spawnedLiner = Instantiate(LineRendererPrefab);
            LineRenderer ren = spawnedLiner.GetComponent<LineRenderer>();
            ren.positionCount = 2;
            ren.SetPosition(1, start);
            ren.SetPosition(0, end);
            myQueue.Enqueue(spawnedLiner);

            float rand = Random.value; // random number between 0 and 1
            if (rand < 0.1)
            {
                
                Instantiate(Particles, end, RotationFromTwoPositions(start, end));

            }
        }
    }
// -----------
    // Shooting Functions
    // -----------

    private void StartShooting()
    {
        ClearLines();
        List<ChainLine> open = new List<ChainLine>();
        List<ChainLine> closed = new List<ChainLine>();

        float totalLength = 0;

        {
            Vector2 start = new Vector2(transform.position.x, transform.position.z); 
            Vector2 delta = new Vector2(transform.forward.x, transform.forward.z) * .1f;

            totalLength = minRayLength * 3;

            open.Add(new ChainLine(start, start + RotateVec2ByAngle(delta, -5f)));
            open.Add(new ChainLine(start, start + delta));
            open.Add(new ChainLine(start, start + RotateVec2ByAngle(delta,  5)));
        }

        while (open.Count > 0 && totalLength < maxTravelDist)
        {
            ChainLine curr = open[0];
            open.RemoveAt(0);

            float dist = (curr.end - curr.start).magnitude;
            float splitChance = splitChancePerLength * dist;

            for (int i = 0; i < 5; i++)
            {
                ChainLine next = GetNewLine(curr.start, curr.end, false);

                if (next.hit != null)
                {
                    totalLength += (next.start - next.end).magnitude;

                    next.hit.TakeDamage(damageAmount);
                    closed.Add(next);
                    break;
                }
                else if (IsIntersecting(open, next) || IsIntersecting(closed, next))
                {
                    continue;
                }
                else
                {
                    totalLength += (next.start - next.end).magnitude;

                    open.Add(next);
                    break;
                }
            }

            bool hasSplitter = UnityEngine.Random.Range(0f, 1f) < splitChance;

            if (hasSplitter)
            {
                for (int i = 0; i < 5; i++)
                {
                    ChainLine next = GetNewLine(curr.start, curr.end, true);

                    if (next.hit != null)
                    {
                        totalLength += (next.start - next.end).magnitude;

                        next.hit.TakeDamage(damageAmount);
                        closed.Add(next);
                        break;
                    }
                    else if (IsIntersecting(open, next) || IsIntersecting(closed, next))
                    {
                        continue;
                    }
                    else
                    {
                        totalLength += (next.start - next.end).magnitude;

                        open.Add(next);
                        break;
                    }
                }
            }

            closed.Add(curr);
        }

        DebugDraw(open);
        DebugDraw(closed);

        Draw(open);
        Draw(closed);

        StartCoroutine(ClearLinesAfterDelay());
    }
}
