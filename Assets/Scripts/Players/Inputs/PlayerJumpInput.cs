using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpInput : BasePlayerInput
{
    private Tween _jumpAnimation;
    private float _lastJumpTime = float.MinValue;

    void Start()
    {
        Init();
        InputManager.Instance.Player.Jump.performed += JumpPerformed;
    }

    private Tween GetAnimation()
    {
        if (_jumpAnimation == null)
        {
            _jumpAnimation = _playerGO
                .SpriteRenderer
                .transform
                .DOLocalMoveY(1f, Player.JumpDuration / 2)
                .SetEase(Ease.OutQuad)
                .SetLoops(2, LoopType.Yoyo)
                .SetAutoKill(false)
                .OnComplete(() => Player.EndJump());
            _jumpAnimation.Pause();
        }

        return _jumpAnimation;
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if (Player.IsJumping || Player.IsDashing)
        {
            return;
        }

        float timeSinceLastJump = Time.time - _lastJumpTime;
        if (timeSinceLastJump < Player.JumpAbility.Cooldown)
        {
            return;
        }

        Player.Jump();
        _lastJumpTime = Time.time;
        GetAnimation()?.Restart();
    }

    private void OnDestroy()
    {
        InputManager.Instance.Player.Jump.performed -= JumpPerformed;
    }

    void OnEnable() => InputManager.Instance.Player.Jump.Enable();
    void OnDisable()
    {
        if (InputManager.Instance?.Player != null)
        {
            InputManager.Instance.Player.Jump.Disable();
        }
    }
}
