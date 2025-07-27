using UnityEngine;

public class ChunkGO : MonoBehaviour
{
    public int Size { get; private set; }
    public Vector2Int ChunkCoords { get; private set; }

    public void Init(Vector2Int coords, int size)
    {
        Size = size;
        ChunkCoords = coords;
        gameObject.name = $"Chunk_{coords.x}_{coords.y}";
        transform.position = new Vector3(coords.x * Size, coords.y * Size, 0);
    }
}
