using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : MonoBehaviour
{
  public GameObject Arrow;

  float _spinSpeed;

  // Start is called before the first frame update
  void Start()
  {
    _spinSpeed = 0f;
  }

  // Update is called once per frame
  void Update()
  {
    Arrow.transform.Rotate(new Vector3(0, 0, -_spinSpeed * Time.deltaTime));
  }

  public void StartSpinning()
  {
    _spinSpeed = 360f*2f;
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
    Debug.Log(_spinSpeed);
    return _spinSpeed != 0f;
  }
}

public enum SpinnerResult
{
  Hit,
  Miss,
  Crit
}
