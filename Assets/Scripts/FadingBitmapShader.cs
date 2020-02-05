using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class FadingBitmapShader : MonoBehaviour
{
    public float _currentValue;
    public float _minFadeValue;
    public float _maxFadeValue;
    public float _fadingSpeed;

    private TMP_Text _textMeshProElement;
    private int _propertyID;
    private bool _fading;

    void Start()
    {
        _textMeshProElement = GetComponent<TMP_Text>();
        //_propertyID = Shader.PropertyToID("_FadeValue");
        //_textMeshProElement.material.SetFloat(_propertyID, _currentValue);
    }

    void LateUpdate()
    {
        if(_fading)
            _currentValue -= Time.deltaTime / _fadingSpeed;
        else
            _currentValue += Time.deltaTime / _fadingSpeed;
        _currentValue = Mathf.Clamp(_currentValue, _minFadeValue, _maxFadeValue);
        if (_currentValue == _maxFadeValue || _currentValue == _minFadeValue) _fading = !_fading;
        //_textMeshProElement.material.SetFloat(_propertyID, _currentValue);
        _textMeshProElement.color = new Color(1f,1f,1f,_currentValue);
    }
}
