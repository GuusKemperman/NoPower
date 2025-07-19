using System.Collections;
using System.Collections.Generic;
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
            // Start zapping bitches
            if (!IsShooting) { StartShooting(); }

        }
        else { StopShooting(); }


        if (AttackAction.WasReleasedThisFrame())
        {
            // Stop zapping bitches



        }
        else { }

    }

    private void StartShooting()
    {
        IsShooting = true;

        if (PlayerTransform != null 
            && PlayerEnemyDetector != null
            && LineRendererPrefab != null)
        {
           
            // Only doing this on the first frame of shooting
            if (!IsLineRendererSpawned) { NewLineRenderer(PlayerTransform, PlayerEnemyDetector.FindClosestEnemy().transform); }


            
        }
    }

    private void NewLineRenderer(Transform StartPosition, Transform EndPosition)
    {
        GameObject NewLine = Instantiate(LineRendererPrefab);
        SpawnedLineRenderers.Add(NewLine);
        StartCoroutine(UpdateLineRenderer(NewLine, StartPosition, EndPosition));
    }

    IEnumerator UpdateLineRenderer (GameObject Line, Transform StartPosition, Transform EndPosition)
    {
        if (IsShooting && IsLineRendererSpawned && Line != null)
        {
            Line.GetComponent<Lighting_LineRendererController>().SetPosition(StartPosition, EndPosition);
            yield return new WaitForSeconds(RefreshRate);
        }
    }

    private void StopShooting()
    {


    }

}
