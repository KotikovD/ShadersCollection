using JetBrains.Annotations;
using UnityEngine;


public class ActionProvider : MonoBehaviour
{
    [SerializeField] private GameObject _cameraTransform;
    [SerializeField] private GameObject _canvasTransform;
    
    
    [UsedImplicitly]
    public void OnCameraSwitch()
    {
        _canvasTransform.SetActive(_cameraTransform.transform.childCount > 0);
    }
    
}
