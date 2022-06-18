using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public sealed class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private TMP_Text _shaderLabel;
    [SerializeField] private List<CameraSettings> _settings;
    private int _currentIndex;
    private int _maxIndex;

    private void Awake()
    {
        _maxIndex = _settings.Count - 1;
        _camera.transform.localPosition = Vector3.zero;
        _camera.transform.localRotation = Quaternion.identity;
        ApplyNextSettings();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UpdateCurrentIndex(-1);
            ApplyNextSettings();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpdateCurrentIndex(1);
            ApplyNextSettings();
        }
    }

    private void UpdateCurrentIndex(int deltaIndex)
    {
        var nextIndex = _currentIndex + deltaIndex;
        
        if (nextIndex < 0)
            nextIndex = _maxIndex - _currentIndex + nextIndex % _maxIndex + 1;

        if (nextIndex > _maxIndex)
            nextIndex = Mathf.Max(0, Mathf.RoundToInt(nextIndex % _maxIndex) - 1);
        
        _currentIndex = nextIndex;
    }

    private void ApplyNextSettings()
    {
        var nextSet = _settings[_currentIndex];
        _camera.transform.SetParent(nextSet.Parent, false);
        _camera.backgroundColor = nextSet.Background;
        _shaderLabel.text = nextSet.Name;
        BroadcastMessage("OnCameraSwitch");
    }

    [Serializable]
    class CameraSettings
    {
        public string Name;
        public Transform Parent;
        public Color Background;
    }

    [Button()]
    private void AddNewSettings()
    {
        _settings.Add(new CameraSettings()
        {
            Name = Camera.main.transform.parent.parent.name,
            Parent = Camera.main.transform.parent,
            Background = Camera.main.backgroundColor
        });
    }
    
}
