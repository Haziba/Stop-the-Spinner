using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOptionController : MonoBehaviour
{
  Func<int> _resolution;
  public event EventHandler OnOptionClicked;

  public void SetResolution(Func<int> resolution)
  {
    _resolution = resolution;
  }

  public int NextStepId()
  {
    return _resolution();
  }

  // Start is called before the first frame update
  void Start()
  {
      
  }

  // Update is called once per frame
  void Update()
  {
      
  }

  void OnMouseDown()
  {
    OnOptionClicked?.Invoke(this, new EventArgs());
  }
}
