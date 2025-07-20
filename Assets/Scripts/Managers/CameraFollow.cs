using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _followTime = .3f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -10f);

    private bool _paused = false;

    public void Pause()
    {
        _paused = true;
    }

    public void Resume()
    {
        _paused = false;
    }

    private void LateUpdate()
    {
        if (_target == null || _paused)
        {
            return;
        }

        Vector3 targetPos = _target.position + _offset;
        transform.DOMove(targetPos, _followTime).SetEase(Ease.OutQuad);
    }
}