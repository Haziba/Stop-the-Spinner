using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinnerController : MonoBehaviour
{
  public GameObject Arrow;
  public GameObject[] SpinnerEdges;
  public GameObject HitArea;
  public GameObject CritArea;

  float _spinSpeed;
  float _hit;
  float _crit;

  AgentStatusEffects _statusEffects;

  float _intoxicatedSpeed;
  float _intoxicatedAccel;
  float _intoxicatedChangeCountdown;

  // Start is called before the first frame update
  void Start()
  {
    _spinSpeed = 0f;
    _hit = 0.1f;
    _crit = 0.6f;
    UpdateSegments();
  }

  // Update is called once per frame
  void Update()
  {
    if(IsSpinning() && _statusEffects.HasFlag(AgentStatusEffects.Intoxicated)) {
      _intoxicatedSpeed += _intoxicatedAccel * Time.deltaTime;
      _intoxicatedChangeCountdown -= Time.deltaTime;
      if(_intoxicatedChangeCountdown <= 0)
        SpinIntoxicationWheel();

      Arrow.transform.Rotate(new Vector3(0, 0, -_intoxicatedSpeed * Time.deltaTime));
    }

    Arrow.transform.Rotate(new Vector3(0, 0, -_spinSpeed * Time.deltaTime));
  }

  public void UpdateSegments()
  {
    HitArea.GetComponent<Image>().fillAmount = _hit;
    HitArea.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180 * _hit);

    CritArea.GetComponent<Image>().fillAmount = _crit;
    CritArea.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180 * _crit);

    var edges = new List<float>();

    // Show Hit edge if not covered by Crit edge
    if(_hit + _crit < 1)
      edges.AddRange(new [] { -_hit * 180, _hit * 180 });
    edges.AddRange(new [] { 180 - (_crit * 180), 180 + (_crit * 180) });

    SetEdges(edges.ToArray());
  }

  void SetEdges(float[] positions)
  {
    for(var i = 0; i < SpinnerEdges.Length; i++)
    {
      var edge = SpinnerEdges[i];

      if(i >= positions.Length) {
        edge.SetActive(false);
        continue;
      }

      edge.SetActive(true);
      edge.transform.rotation = Quaternion.Euler(0, 0, positions[i] - 90);
    }
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

    _spinSpeed = 100f;//360f*multiplier;
  }

  public void UpdateConfig(SpinnerConfiguration config)
  {
    _hit = config.Hit();
    _crit = config.Crit();
    UpdateSegments();
  }

  void SpinIntoxicationWheel()
  {
    _intoxicatedAccel = UnityEngine.Random.Range(180f, 720f) * 2;
    if(UnityEngine.Random.Range(0, 2) == 0)
      _intoxicatedAccel *= -1;
    if(_intoxicatedSpeed >= 720f)
      _intoxicatedAccel = -Math.Abs(_intoxicatedAccel);
    if(_intoxicatedSpeed <= -720f)
      _intoxicatedAccel = Math.Abs(_intoxicatedAccel);
    _intoxicatedChangeCountdown = 0.1f;
  }

  public SpinnerResult StopSpinning()
  {
    _spinSpeed = 0f;

    var direction = Arrow.transform.rotation.eulerAngles.z;

    if (direction > 360 - (_hit * 180) || direction < (_hit * 180))
      return SpinnerResult.Hit;
    if (direction < 180 + (_crit * 180) && direction > 180 - (_crit * 180))
      return SpinnerResult.Crit;

    return SpinnerResult.Miss;
  }

  public bool IsSpinning()
  {
    return _spinSpeed != 0f;
  }
}