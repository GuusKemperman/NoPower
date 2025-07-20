using DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour, DependencyInjection.IDependencyProvider
{
    struct Chunk
    {
        public Vector2Int cellPos;
        public List<GameObject> objectsInChunk;
    };
    List<Chunk> chunks = new List<Chunk>();

    [DependencyInjection.Provide]
    public TerrainGenerator Provide()
    {
        return this;
    }

    [Inject] private PlayerTag player = null;

    [SerializeField]
    float updateInterval = 5.0f;

    [SerializeField]
    float objectDensity = .1f;

    [SerializeField]
    float maxNonYRotation = 15.0f;

    [SerializeField]
    float heightOffset = -5.0f;

    [SerializeField]
    float maxScaleDeviation = .2f;

    [SerializeField]
    float chunkSize = 20.0f;

    [SerializeField] float numberOfBuildingsInChunk = 0.5f;

    [SerializeField]
    float generationRadius = 100.0f;

    [SerializeField]
    float destroyRadiusFactor = 1.5f;

    [SerializeField]
    GameObject chunkGround = null;

    [SerializeField] private GameObject navmeshPrefab = null;
    
    [SerializeField]
    List<GameObject> decoration = new List<GameObject>();

    [SerializeField] 
    List<GameObject> buildings = new List<GameObject>();

    [SerializeField]
    GameObject reactorPrefab = null;

    [SerializeField]
    float initialReactorDistance = 10f;

    [SerializeField]
    float reactorDistanceIncrease = 30f;

    [SerializeField]
    float numReactorsPerCircumference = .8f;

    [SerializeField]
    float minDistanceBetweenReactorsInSameLayer = 20f;

    [SerializeField]
    float numReactorsPerCircumferenceDistanceFactor = .9f;

    float currentlySpawnedReactorsAtDistance = -1;
    private NavMeshSurface surface = null;

    Vector2 CellToWorld(Vector2Int cellPos)
    {
        return new Vector2((float)cellPos.x * chunkSize, (float)cellPos.y * chunkSize);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeNavmesh();
        StartCoroutine(UpdateTerrain());
        StartCoroutine(SpawnReactors());
    }

    private void InitializeNavmesh()
    {
       surface = Instantiate(navmeshPrefab, transform.position, quaternion.identity).GetComponent<NavMeshSurface>();
    }

    private void InstanciateManMadeObject(Chunk chunk, System.Random rnd, Vector3 chunkPos)
    {
        int objIndex = rnd.Next() % buildings.Count;
        GameObject toSpawn = buildings[objIndex];

        Vector3 offset = new Vector3((float)rnd.NextDouble() * chunkSize,
            heightOffset,
            (float)rnd.NextDouble() * chunkSize);

        Quaternion rot = Quaternion.Euler(0, (float)rnd.NextDouble() * 360.0f, 0);

        GameObject newDecoration = Instantiate(toSpawn, chunkPos + offset, rot);
     
        chunk.objectsInChunk.Add(newDecoration);
    }

    IEnumerator UpdateTerrain()
    {
        while (true)
        {
            Vector3 playerPos3D = player.transform.position;
            Vector2 playerPos2D = new Vector2(playerPos3D.x, playerPos3D.z);

            foreach (Chunk chunk in chunks)
            {
                if (chunk.objectsInChunk == null)
                {
                    continue;
                }

                Vector2 centre = CellToWorld(chunk.cellPos) + new Vector2(chunkSize, chunkSize) * .5f;

                if (Vector2.Distance(playerPos2D, centre) <= generationRadius * destroyRadiusFactor + chunkSize * .5f)
                {
                    continue;
                }

                // Otherwise destroy
                foreach (GameObject obj in chunk.objectsInChunk)
                {
                    Destroy(obj);
                }
                chunk.objectsInChunk.Clear();
            }
            chunks.RemoveAll(
                chunk => chunk.objectsInChunk == null || chunk.objectsInChunk.Count == 0);
            chunks.Clear();

            Vector2 worldPosOfPlayerChunk = new Vector2(playerPos3D.x - playerPos3D.x % chunkSize,
                playerPos3D.z - playerPos3D.z % chunkSize);

            Vector2Int chunkPosOfPlayerChunk = new Vector2Int(Mathf.RoundToInt(worldPosOfPlayerChunk.x / chunkSize),
                Mathf.RoundToInt(worldPosOfPlayerChunk.y / chunkSize));

            int radiusInCells = (int)MathF.Ceiling(generationRadius / chunkSize) + 1;

            float area = chunkSize * chunkSize;
            float avgNumObjectsPerChunk = objectDensity * area;

            for (int x = chunkPosOfPlayerChunk.x - radiusInCells; x <= chunkPosOfPlayerChunk.x + radiusInCells; x++)
            {
                for (int y = chunkPosOfPlayerChunk.y - radiusInCells; y <= chunkPosOfPlayerChunk.y + radiusInCells; y++)
                {
                    Vector2Int cellpos = new Vector2Int(x, y);

                    if (chunks.Exists(chunk => chunk.cellPos == cellpos))
                    {
                        continue;
                    }

                    int chunkHash = HashCode.Combine(x, y);
                    System.Random rnd = new System.Random(chunkHash);

                    Chunk chunk = new Chunk();
                    chunk.cellPos = new Vector2Int(x, y);
                    chunk.objectsInChunk = new List<GameObject>();

                    Vector2 cellWorld2D = CellToWorld(chunk.cellPos);
                    Vector3 cellWorld3D = new Vector3(cellWorld2D.x, 0f, cellWorld2D.y);

                    float numObjectsFloat = (float)rnd.NextDouble() * avgNumObjectsPerChunk * 2.0f;

                    int numObjectsInt = Convert.ToBoolean(rnd.Next() & 1) ? (int)MathF.Floor(numObjectsFloat) :
                            (int)MathF.Ceiling(numObjectsFloat);

                    for (int i = 0; i < numObjectsInt; i++)
                    {
                        int objIndex = rnd.Next() % decoration.Count;
                        GameObject toSpawn = decoration[objIndex];

                        Vector3 offset = new Vector3((float)rnd.NextDouble() * chunkSize,
                            heightOffset, 
                            (float)rnd.NextDouble() * chunkSize);

                        Quaternion rot = Quaternion.Euler((float)rnd.NextDouble() * 2.0f * maxNonYRotation - maxNonYRotation, 
                            (float)rnd.NextDouble() * 360.0f,
                            (float)rnd.NextDouble() * 2.0f * maxNonYRotation - maxNonYRotation);

                        float scale = (float)rnd.NextDouble() * maxScaleDeviation * 2.0f + 1f;

                        GameObject newDecoration = Instantiate(toSpawn, cellWorld3D + offset, rot);
                        newDecoration.transform.localScale = new Vector3(scale, scale, scale);
                        chunk.objectsInChunk.Add(newDecoration);
                    }



                    int numManMadeObjectsInt = Convert.ToBoolean(rnd.Next() & 1)
                        ? (int)MathF.Floor(numberOfBuildingsInChunk)
                        : (int)MathF.Ceiling(numberOfBuildingsInChunk);


                    for (int i = 0; i < numManMadeObjectsInt; i++)
                    {
                        InstanciateManMadeObject(chunk, rnd,cellWorld3D);
                    }

                    GameObject ground = Instantiate(chunkGround, cellWorld3D, Quaternion.identity);
                    ground.transform.localScale = new Vector3(chunkSize, 1.0f, chunkSize);
                    chunk.objectsInChunk.Add(ground);

                    chunks.Add(chunk);

                    Vector2 corner1 = CellToWorld(chunk.cellPos);
                    Vector2 corner2 = CellToWorld(chunk.cellPos + new Vector2Int(1, 1));

                    // Get the other two corners
                    Vector2 corner3 = new Vector2(corner1.x, corner2.y); // top-left
                    Vector2 corner4 = new Vector2(corner2.x, corner1.y); // bottom-right

                    // Draw all four sides of the square
                    Debug.DrawLine(new Vector3(corner1.x, 0.0f, corner1.y), new Vector3(corner3.x, 0.0f, corner3.y), Color.red, updateInterval); // left side
                    Debug.DrawLine(new Vector3(corner3.x, 0.0f, corner3.y), new Vector3(corner2.x, 0.0f, corner2.y), Color.red, updateInterval); // top side
                    Debug.DrawLine(new Vector3(corner2.x, 0.0f, corner2.y), new Vector3(corner4.x, 0.0f, corner4.y), Color.red, updateInterval); // right side
                    Debug.DrawLine(new Vector3(corner4.x, 0.0f, corner4.y), new Vector3(corner1.x, 0.0f, corner1.y), Color.red, updateInterval); // bottom side
                }
            }

            UpdateNavigation();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void UpdateNavigation()
    {
        surface.transform.position = new Vector3(player.transform.position.x,0,player.transform.position.z);
        surface.BuildNavMesh();
    }

    IEnumerator SpawnReactors()
    {
        while (true)
        {
            float playerDist = player.transform.position.magnitude;
            float maxDistToSpawn = playerDist + generationRadius;
            float circumFactor = 1.0f;

            for (float currentDist = initialReactorDistance;
                currentDist <= maxDistToSpawn; 
                currentDist += reactorDistanceIncrease)
            {
                circumFactor *= numReactorsPerCircumferenceDistanceFactor;

                if (currentDist > currentlySpawnedReactorsAtDistance)
                {
                    float cirumferenceHere = currentDist * 2.0f * MathF.PI * circumFactor;
                    int numToSpawn = Mathf.Max(1, (int)MathF.Ceiling(numReactorsPerCircumference * cirumferenceHere));

                    List<Vector3> alreadySpawnedPrefabs = new List<Vector3>();

                    for (int i = 0; i < numToSpawn; i++)
                    {
                        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
                        Vector3 spawnPos = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
                        spawnPos *= currentDist;

                        bool invalid = false;

                        foreach (Vector3 pos in alreadySpawnedPrefabs)
                        {
                            if (Vector3.Distance(pos, spawnPos) < minDistanceBetweenReactorsInSameLayer)
                            {
                                invalid = true;
                                break;
                            }
                        }

                        if (!invalid)
                        {
                            Instantiate(reactorPrefab, spawnPos, Quaternion.Euler(0.0f, UnityEngine.Random.Range(0f, 360f), 0.0f));

                            alreadySpawnedPrefabs.Add(spawnPos);
                        }
                    }
                }
            }

            currentlySpawnedReactorsAtDistance = maxDistToSpawn;
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
