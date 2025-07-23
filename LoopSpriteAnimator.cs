using DG.Tweening;

public class LoopSpriteAnimator : ASpriteAnimator
{
    protected override Sequence CreateSpriteSequence()
    {
        Sequence animation = base.CreateSpriteSequence();
        animation.SetLoops(-1);

        return animation;
    }
}
