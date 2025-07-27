using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private ChunkGO[] _chunks;
    [SerializeField] private Transform _player;

    private const int ChunkSize = 20;

    private Dictionary<Vector2Int, ChunkGO> generatedChunks = new();

    void Update()
    {
        Vector2Int currentChunkCoord = GetChunkCoord(_player.position);

        // Génère le chunk courant + tous les voisins
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighborCoord = currentChunkCoord + new Vector2Int(x, y);
                if (!generatedChunks.ContainsKey(neighborCoord))
                {
                    GenerateChunk(neighborCoord);
                }
            }
        }
    }

    private Vector2Int GetChunkCoord(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / ChunkSize);
        int y = Mathf.FloorToInt(position.y / ChunkSize);
        return new Vector2Int(x, y);
    }

    private void GenerateChunk(Vector2Int coords)
    {
        ChunkGO chunk = Instantiate(RandomChunk());
        chunk.Init(coords, ChunkSize);

        generatedChunks[coords] = chunk;
    }

    private ChunkGO RandomChunk()
    {
        int rng = Random.Range(0, _chunks.Length);
        return _chunks[rng];
    }
}
