using DependencyInjection;
using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    float chunkSize = 20.0f;

    [SerializeField]
    float generationRadius = 100.0f;

    [SerializeField]
    GameObject chunkGround = null;

    [SerializeField]
    List<GameObject> decoration = new List<GameObject>();

    Vector2 CellToWorld(Vector2Int cellPos)
    {
        return new Vector2((float)cellPos.x * chunkSize, (float)cellPos.y * chunkSize);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpdateTerrain());
    }

    IEnumerator UpdateTerrain()
    {
        while (true)
        {
            // Placeholder terrain clearing
            foreach (Chunk chunk in chunks)
            {
                if (chunk.objectsInChunk == null)
                {
                    continue;
                }

                foreach (GameObject obj in chunk.objectsInChunk)
                {
                    Destroy(obj);
                }
            }
            chunks.Clear();

            Vector3 playerPos3D = player.transform.position;
            Vector2 worldPosOfPlayerChunk = new Vector2(playerPos3D.x - playerPos3D.x % chunkSize,
                playerPos3D.z - playerPos3D.z % chunkSize);

            Vector2Int chunkPosOfPlayerChunk = new Vector2Int(Mathf.RoundToInt(worldPosOfPlayerChunk.x / chunkSize),
                Mathf.RoundToInt(worldPosOfPlayerChunk.y / chunkSize));

            int radiusInCells = (int)MathF.Ceiling(generationRadius / chunkSize) + 1;

            for (int x = chunkPosOfPlayerChunk.x - radiusInCells; x <= chunkPosOfPlayerChunk.x + radiusInCells; x++)
            {
                for (int y = chunkPosOfPlayerChunk.y - radiusInCells; y <= chunkPosOfPlayerChunk.y + radiusInCells; y++)
                {
                    Chunk chunk = new Chunk();
                    chunk.cellPos = new Vector2Int(x, y);
                    chunk.objectsInChunk = new List<GameObject>();

                    Vector2 cellWorld2D = CellToWorld(chunk.cellPos);
                    Vector3 cellWorld3D = new Vector3(cellWorld2D.x, 0f, cellWorld2D.y);

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

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
