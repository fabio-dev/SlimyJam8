using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "LoopSpriteAnimator", menuName = "ScriptableObjects/Animators/LoopSpriteAnimator", order = 1)]
public class LoopSpriteAnimator : ASpriteAnimator
{
    protected override Sequence CreateSpriteSequence()
    {
        Sequence animation = base.CreateSpriteSequence();
        animation.SetLoops(-1);

        return animation;
    }
}
