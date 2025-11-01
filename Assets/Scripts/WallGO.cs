using UnityEngine;

public class WallGO : MonoBehaviour
{
    [SerializeField] private Sprite _bottomSprite;
    [SerializeField] private Sprite _topSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void SetAsTopWall()
    {
        _spriteRenderer.sprite = _topSprite;
    }

    public void SetAsBottomWall()
    {
        _spriteRenderer.sprite = _bottomSprite;
    }
}