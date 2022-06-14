using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


public sealed class SetMaterialProperty : MonoBehaviour
{
    [SerializeField] private string _updatePropertyName;
    [Space(10)]
    [SerializeField] private string _awakePropertyName;
    [SerializeField] private float _awakePropertyValue;
    
    [SerializeField] private List<Renderer> _renderers;
    private List<Material> _materials;

    private void Awake()
    {
        _materials = new List<Material>();
        var shouldUpdateOnAwake = !string.IsNullOrEmpty(_awakePropertyName);
        
        foreach (var renderer in _renderers)
        {
            var material = renderer.material;
            if(shouldUpdateOnAwake)
                material.SetFloat(_awakePropertyName, _awakePropertyValue);
            
            _materials.Add(material);
        }
    }

    private void Update()
    {
        foreach (var material in _materials)
            material.SetVector(_updatePropertyName, transform.position);
    }
    
    [Button()]
    private void FindAndAddRenderersInChildren()
    {
        _renderers.Clear();
        var renderers = GetComponentsInChildren<Renderer>();
        _renderers.AddRange(renderers);
    }
}