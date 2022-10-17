using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : MonoBehaviour
{
  public GameObject Arrow;

  float _spinSpeed;

  AgentStatusEffects _statusEffects;

  float _intoxicatedSpeed;
  float _intoxicatedAccel;
  float _intoxicatedChangeCountdown;

  // Start is called before the first frame update
  void Start()
  {
    _spinSpeed = 0f;
  }

  // Update is called once per frame
  void Update()
  {
    if(_statusEffects.HasFlag(AgentStatusEffects.Intoxicated)) {
      _intoxicatedSpeed += _intoxicatedAccel * Time.deltaTime;
      _intoxicatedChangeCountdown -= Time.deltaTime;
      if(_intoxicatedChangeCountdown <= 0)
        SpinIntoxicationWheel();
    }

    Debug.Log(_intoxicatedSpeed);

    Arrow.transform.Rotate(new Vector3(0, 0, -(_spinSpeed + _intoxicatedSpeed) * Time.deltaTime));
  }

  public void StartSpinning(AgentStatusEffects statusEffects)
  {
    _statusEffects = statusEffects;

    var multiplier = 2f;

    if(statusEffects.HasFlag(AgentStatusEffects.Distracted) && !statusEffects.HasFlag(AgentStatusEffects.Focused))
      multiplier = 3f;
    if(statusEffects.HasFlag(AgentStatusEffects.Focused) && !statusEffects.HasFlag(AgentStatusEffects.Distracted))
      multiplier = 1f;

    SpinIntoxicationWheel();

    _spinSpeed = 360f*multiplier;
  }

  void SpinIntoxicationWheel()
  {
    _intoxicatedAccel = UnityEngine.Random.Range(360f, 720f) * 2;
    if(UnityEngine.Random.Range(0, 2) == 0)
      _intoxicatedAccel *= -1;
    if(_intoxicatedSpeed >= 720f)
      _intoxicatedAccel = -Math.Abs(_intoxicatedAccel);
    if(_intoxicatedSpeed <= -720f)
      _intoxicatedAccel = Math.Abs(_intoxicatedAccel);
    _intoxicatedChangeCountdown = 0.25f;
  }

  public SpinnerResult StopSpinning()
  {
    _spinSpeed = 0f;

    var direction = Arrow.transform.rotation.eulerAngles.z;

    if(direction > 265 || direction < 90)
      return SpinnerResult.Hit;
    if(direction < 192 && direction > 160)
      return SpinnerResult.Crit;

    return SpinnerResult.Miss;
  }

  public bool IsSpinning()
  {
    //Debug.Log(_spinSpeed);
    return _spinSpeed != 0f;
  }
}

public enum SpinnerResult
{
  Hit,
  Miss,
  Crit
}
