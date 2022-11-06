using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
  Vector3 _initialPosition;
  
  bool _shaking;
  float _shakeMagnitude;
  HUtilities.Countdown _shakeCountdown;
  
  public void Shake(float time, float magnitude, Action callback)
  {
    _shaking = true;
    _shakeMagnitude = 1f;

    _shakeCountdown = new HUtilities.Countdown(time, () =>
    {
      _shaking = false;
      callback();
    });
  }

  void Start()
  {
    _initialPosition = transform.position;
  }

  void Update()
  {
    if (_shakeCountdown != null)
      if(!_shakeCountdown.Update())
        _shakeCountdown = null;
    
    if (!_shaking)
      return;

    transform.position = _initialPosition + Random.insideUnitSphere * _shakeMagnitude * 0.1f;
  }
}
