using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lighting_ChainShot : MonoBehaviour
{
    [SerializeField] InputActionAsset ActionMap = null;
    [SerializeField] Transform PlayerTransform = null;
    [SerializeField] Lighting_EnemyDetector PlayerEnemyDetector = null;
    [SerializeField] GameObject LineRendererPrefab = null;
    [SerializeField] float RefreshRate = 1.0f / 60.0f;

    bool IsShooting = false;            // Holds if in the current frame the player is shooting or not.
    bool IsLineRendererSpawned = false; // Used as flag to spawn a new line renderer on the first frame of began shooting, later frames we do not need to spawn anymore.
    List<GameObject> SpawnedLineRenderers = new List<GameObject>();

    void Update()
    {
        InputAction AttackAction = ActionMap.FindAction("Attack");
        if (AttackAction.WasPressedThisFrame())
        {

            if (PlayerEnemyDetector.GetEnemiesInRange().Count > 0)
            {
                if (!IsShooting) { StartShooting(); }
            }
            else { StopShooting(); }
        }

        if (AttackAction.WasReleasedThisFrame())
        {
            StopShooting();
        }
    }

    // -----------
    // Utility Functions
    // -----------
    private bool AreAllNonNull(params object[] values) => values.All(v => v != null);
    private bool AreAllTrue(params bool[] values) => values.All(v => v);

    // -----------
    // Shooting Functions
    // -----------

    private void StartShooting()
    {
        IsShooting = true;

        if (AreAllNonNull(PlayerTransform, PlayerEnemyDetector, LineRendererPrefab))
        {
            
            if (!IsLineRendererSpawned) // Only doing this on the first frame of shooting
            {
                GameObject NearestEnemy = PlayerEnemyDetector.FindClosestEnemy();
                if (NearestEnemy != null) {
                    IsLineRendererSpawned = true;
                    NewLineRenderer(PlayerTransform, NearestEnemy.transform);
                }
            }  
        }
    }
    private void StopShooting()
    {
        IsShooting = false;
        IsLineRendererSpawned = false;

        for (int i = 0; i < SpawnedLineRenderers.Count; i++) { Destroy(SpawnedLineRenderers[i]); }
        SpawnedLineRenderers.Clear();
    }

    // -----------
    // Line Renderer Functions
    // -----------
    private void NewLineRenderer(Transform StartPosition, Transform EndPosition)
    {
        GameObject NewLine = Instantiate(LineRendererPrefab);
        SpawnedLineRenderers.Add(NewLine);
        StartCoroutine(UpdateLineRenderer(NewLine, StartPosition, EndPosition));
    }

    IEnumerator UpdateLineRenderer (GameObject Line, Transform StartPosition, Transform EndPosition)
    {
        bool CheckNull = AreAllNonNull(Line, StartPosition, EndPosition);
        bool CheckBool = AreAllTrue(IsShooting, IsLineRendererSpawned);

        if (CheckNull && CheckBool)
        {
            Line.GetComponent<Lighting_LineRendererController>().SetPosition(StartPosition, EndPosition);
            yield return new WaitForSeconds(RefreshRate);

            GameObject Enemy = PlayerEnemyDetector.FindClosestEnemy();
            if (Enemy != null) {
                StartCoroutine(UpdateLineRenderer(Line, StartPosition, PlayerEnemyDetector.FindClosestEnemy().transform));
            }
            
        }
    }
}
