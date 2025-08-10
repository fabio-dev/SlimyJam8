using UnityEngine;

public interface IZone
{
    bool Contains(Vector2 point);
    void Reduce(float radiusAbsorption);
    void Remove();
}
