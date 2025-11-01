using Assets.Scripts.Domain;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private int _minRooms = 15;
    [SerializeField] private int _maxRooms = 25;
    [SerializeField] private int _maxAttempts = 200;
    [SerializeField, Range(0f, 1f)] private float _branchingChance = 0.3f; // Plus bas = plus étalé

    [Header("Prefabs")]
    [SerializeField] private ChunkGO[] _chunks;
    [SerializeField] private WallGO _wallPrefab;
    [SerializeField] private Transform _player;

    [Header("Props")]
    [SerializeField] private DropManager _potDropManager;
    [SerializeField] private DropManager _chestDropManager;
    [SerializeField] private PotGO _potPrefab;
    [SerializeField] private ChestGO _chestPrefab;
    [Range(0f, 1f), SerializeField] private float _potChanceToSpawn = 0.5f;
    [Range(0f, 1f), SerializeField] private float _chestChanceToSpawn = 0.3f;

    private const int ChunkSize = 20;
    private Dictionary<Vector2Int, ChunkGO> generatedChunks = new();
    private Vector2Int startRoomCoord;

    void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        HashSet<Vector2Int> roomPositions = GenerateRoomLayout();

        foreach (Vector2Int pos in roomPositions)
        {
            InstantiateRoom(pos);
        }

        GenerateWalls(roomPositions);
        SpawnProps(roomPositions);
    }

    private HashSet<Vector2Int> GenerateRoomLayout()
    {
        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();

        // Commencer au centre
        Vector2Int startPos = Vector2Int.zero;
        startRoomCoord = startPos;
        rooms.Add(startPos);
        frontier.Enqueue(startPos);

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        int targetRooms = Random.Range(_minRooms, _maxRooms + 1);
        int attempts = 0;

        while (rooms.Count < targetRooms && attempts < _maxAttempts)
        {
            attempts++;

            if (frontier.Count == 0)
                break;

            Vector2Int currentPos = frontier.Dequeue();

            List<Vector2Int> shuffledDirections = directions.OrderBy(x => Random.value).ToList();

            foreach (Vector2Int direction in shuffledDirections)
            {
                if (rooms.Count >= targetRooms)
                    break;

                Vector2Int nextPos = currentPos + direction;

                if (!rooms.Contains(nextPos))
                {
                    if (CountAdjacentRooms(nextPos, rooms) <= 1)
                    {
                        rooms.Add(nextPos);

                        if (Random.value > _branchingChance)
                        {
                            frontier.Enqueue(currentPos);
                        }

                        frontier.Enqueue(nextPos);
                        break; 
                    }
                }
            }
        }

        Debug.Log($"Donjon généré avec {rooms.Count} salles");
        return rooms;
    }

    private int CountAdjacentRooms(Vector2Int pos, HashSet<Vector2Int> rooms)
    {
        int count = 0;
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            if (rooms.Contains(pos + dir))
                count++;
        }

        return count;
    }

    private void InstantiateRoom(Vector2Int coords)
    {
        ChunkGO chunk = Instantiate(RandomChunk());
        chunk.Init(coords, ChunkSize);
        generatedChunks[coords] = chunk;
    }

    private void GenerateWalls(HashSet<Vector2Int> roomPositions)
    {
        HashSet<Vector3> topWallPositions = new HashSet<Vector3>();

        foreach (Vector2Int pos in roomPositions)
        {
            Vector3 roomCenter = new Vector3(pos.x * ChunkSize, pos.y * ChunkSize, 0);

            // Créer des murs avec ou sans ouverture selon les salles adjacentes
            if (!roomPositions.Contains(pos + Vector2Int.up))
            {
                bool hasOpeningNorth = false;
                CreateWallSegments(roomCenter, Direction.North, hasOpeningNorth, topWallPositions);
            }
            else
            {
                // Salle adjacente au nord = créer une ouverture
                bool hasOpeningNorth = true;
                CreateWallSegments(roomCenter, Direction.North, hasOpeningNorth, topWallPositions);
            }

            if (!roomPositions.Contains(pos + Vector2Int.down))
            {
                bool hasOpeningSouth = false;
                CreateWallSegments(roomCenter, Direction.South, hasOpeningSouth, topWallPositions);
            }
            else
            {
                bool hasOpeningSouth = true;
                CreateWallSegments(roomCenter, Direction.South, hasOpeningSouth, topWallPositions);
            }

            if (!roomPositions.Contains(pos + Vector2Int.left))
            {
                bool hasOpeningWest = false;
                CreateWallSegments(roomCenter, Direction.West, hasOpeningWest, topWallPositions);
            }
            else
            {
                bool hasOpeningWest = true;
                CreateWallSegments(roomCenter, Direction.West, hasOpeningWest, topWallPositions);
            }

            if (!roomPositions.Contains(pos + Vector2Int.right))
            {
                bool hasOpeningEast = false;
                CreateWallSegments(roomCenter, Direction.East, hasOpeningEast, topWallPositions);
            }
            else
            {
                bool hasOpeningEast = true;
                CreateWallSegments(roomCenter, Direction.East, hasOpeningEast, topWallPositions);
            }
        }

        CreateBottomWalls(topWallPositions);
    }

    private void CreateWallSegments(Vector3 roomCenter, Direction direction, bool hasOpening, HashSet<Vector3> topWallPositions)
    {
        float halfSize = ChunkSize / 2f;
        float doorSize = 4f;

        switch (direction)
        {
            case Direction.North:
                if (hasOpening)
                {
                    CreateWall(roomCenter + new Vector3(-halfSize, halfSize, 0), roomCenter + new Vector3(-doorSize / 2, halfSize, 0), true, topWallPositions);
                    CreateWall(roomCenter + new Vector3(doorSize / 2, halfSize, 0), roomCenter + new Vector3(halfSize, halfSize, 0), true, topWallPositions);
                }
                else
                {
                    CreateWall(roomCenter + new Vector3(-halfSize, halfSize, 0), roomCenter + new Vector3(halfSize, halfSize, 0), true, topWallPositions);
                }
                break;

            case Direction.South:
                if (hasOpening)
                {
                    CreateWall(roomCenter + new Vector3(-halfSize, -halfSize, 0), roomCenter + new Vector3(-doorSize / 2, -halfSize, 0), true, topWallPositions);
                    CreateWall(roomCenter + new Vector3(doorSize / 2, -halfSize, 0), roomCenter + new Vector3(halfSize, -halfSize, 0), true, topWallPositions);
                }
                else
                {
                    CreateWall(roomCenter + new Vector3(-halfSize, -halfSize, 0), roomCenter + new Vector3(halfSize, -halfSize, 0), true, topWallPositions);
                }
                break;

            case Direction.West:
                if (hasOpening)
                {
                    CreateWall(roomCenter + new Vector3(-halfSize, -halfSize, 0), roomCenter + new Vector3(-halfSize, -doorSize / 2, 0), true, topWallPositions);
                    CreateWall(roomCenter + new Vector3(-halfSize, doorSize / 2, 0), roomCenter + new Vector3(-halfSize, halfSize, 0), true, topWallPositions);
                }
                else
                {
                    CreateWall(roomCenter + new Vector3(-halfSize, -halfSize, 0), roomCenter + new Vector3(-halfSize, halfSize, 0), true, topWallPositions);
                }
                break;

            case Direction.East:
                if (hasOpening)
                {
                    CreateWall(roomCenter + new Vector3(halfSize, -halfSize, 0), roomCenter + new Vector3(halfSize, -doorSize / 2, 0), true, topWallPositions);
                    CreateWall(roomCenter + new Vector3(halfSize, doorSize / 2, 0), roomCenter + new Vector3(halfSize, halfSize, 0), true, topWallPositions);
                }
                else
                {
                    CreateWall(roomCenter + new Vector3(halfSize, -halfSize, 0), roomCenter + new Vector3(halfSize, halfSize, 0), true, topWallPositions);
                }
                break;
        }
    }

    private void CreateWall(Vector3 start, Vector3 end, bool isTop, HashSet<Vector3> topWallPositions)
    {
        float segmentLength = 1f;
        Vector3 direction = (end - start).normalized;
        float totalLength = Vector3.Distance(start, end);
        int segments = Mathf.CeilToInt(totalLength / segmentLength);

        for (int i = 0; i < segments; i++)
        {
            Vector3 position = start + direction * (i * segmentLength);

            if (isTop)
            {
                WallGO wall = Instantiate(_wallPrefab, position, Quaternion.identity, transform);
                wall.SetAsTopWall();
                topWallPositions.Add(position);
            }
        }
    }

    private void CreateBottomWalls(HashSet<Vector3> topWallPositions)
    {
        foreach (Vector3 topPos in topWallPositions)
        {
            Vector3 posBelow = topPos + Vector3.down;

            if (!topWallPositions.Contains(posBelow))
            {
                WallGO wall = Instantiate(_wallPrefab, posBelow, Quaternion.identity, transform);
                wall.SetAsBottomWall();
            }
        }
    }

    private void SpawnProps(HashSet<Vector2Int> roomPositions)
    {
        foreach (Vector2Int coords in roomPositions)
        {
            if (coords == startRoomCoord)
                continue;

            if (Random.value <= _potChanceToSpawn)
            {
                SpawnPot(coords);
            }

            if (Random.value <= _chestChanceToSpawn)
            {
                SpawnChest(coords);
            }
        }
    }

    private void SpawnPot(Vector2Int coords)
    {
        int x = coords.x * ChunkSize + Random.Range(-ChunkSize / 4, ChunkSize / 4);
        int y = coords.y * ChunkSize + Random.Range(-ChunkSize / 4, ChunkSize / 4);
        PotGO pot = Instantiate(_potPrefab, new Vector2(x, y), Quaternion.identity);
        pot.SetDropManager(_potDropManager);
    }

    private void SpawnChest(Vector2Int coords)
    {
        int x = coords.x * ChunkSize + Random.Range(-ChunkSize / 4, ChunkSize / 4);
        int y = coords.y * ChunkSize + Random.Range(-ChunkSize / 4, ChunkSize / 4);
        ChestGO chest = Instantiate(_chestPrefab, new Vector2(x, y), Quaternion.identity);
        chest.SetDropManager(_chestDropManager);
    }

    private ChunkGO RandomChunk()
    {
        return _chunks[Random.Range(0, _chunks.Length)];
    }

    public ChunkGO GetPlayerCurrentChunk()
    {
        if (_player == null)
            return generatedChunks.FirstOrDefault().Value;

        Vector2Int playerChunkCoord = GetChunkCoord(_player.position);

        if (generatedChunks.ContainsKey(playerChunkCoord))
        {
            return generatedChunks[playerChunkCoord];
        }

        return generatedChunks.FirstOrDefault().Value;
    }

    private Vector2Int GetChunkCoord(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x + ChunkSize / 2f) / ChunkSize);
        int y = Mathf.FloorToInt((position.y + ChunkSize / 2f) / ChunkSize);
        return new Vector2Int(x, y);
    }
}

public enum Direction
{
    North,
    South,
    East,
    West
}