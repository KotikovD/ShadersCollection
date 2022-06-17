using TMPro;
using UnityEngine;


public class TextUpdate : MonoBehaviour
{

    [SerializeField] private TMP_Text _textLabel;
    [SerializeField] private string _text;
    
    void Start()
    {
        _textLabel.text = _text;
    }


}
