using DG.Tweening;
using UnityEngine;

public sealed class RotateAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 _endRotation;
    [SerializeField] private float _time = 1.5f;
    [SerializeField] private Ease _ease = Ease.Linear;
    [SerializeField] private LoopType _loopType = LoopType.Restart;
    [SerializeField] private RotateMode _rotateMode = RotateMode.FastBeyond360;
    [SerializeField] private bool _isRelative = true;
  
    private Tweener _tween;

    private void Awake()
    {
        _tween = transform.DOLocalRotate(_endRotation, _time, _rotateMode)
            .SetLoops(-1, _loopType)
            .SetEase(_ease);

        if (_isRelative)
            _tween.SetRelative(true);

        _tween.Play();
    }
    
}