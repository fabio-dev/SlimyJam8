using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Animations", menuName = "ScriptableObjects/Sprite Animations", order = 1)]
public class SpriteAnimations : ScriptableObject
{
    [SerializeField] protected SpriteAnimationDuration[] _sprites;

    public SpriteAnimationDuration[] Sprites => _sprites;
}
