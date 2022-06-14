using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PingPongManyMaterialsAnimation : MonoBehaviour
{
    [SerializeField] private string _propertyName;
    [SerializeField] private float _startValue = 0f;
    [SerializeField] private float _endValue = 0f;
    [SerializeField] private float _time = 4f;
    [SerializeField] private Ease ease = Ease.Linear;
  
    private Tweener _tween;
    [SerializeField] private List<Renderer> _renderers;
    private List<Material> _materials;

    private void Awake()
    {
        _materials = new List<Material>();
        
        foreach (var renderer in _renderers)
        {
            var material = renderer.material;
            _materials.Add(material);
        }

        foreach (var material in _materials)
        {
            Play(material);
        }
    }
    
    private void Play(Material material)
    {
        material.SetFloat(_propertyName, _startValue);
        _tween = DOTween.To(() => material.GetFloat(_propertyName), value => material.SetFloat(_propertyName, value), _endValue, _time)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);

        _tween.Play();
    }

    [Button()]
    private void FindAndAddRenderersInChildren()
    {
        _renderers.Clear();
        var renderers = GetComponentsInChildren<Renderer>();
        _renderers.AddRange(renderers);
    }
    
}