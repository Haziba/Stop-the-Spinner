using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
  public event EventHandler OnArrowClicked;

  public int PathIndex;

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
    OnArrowClicked?.Invoke(this, new PathChosenEventArgs(PathIndex));
  }
}

public class PathChosenEventArgs : EventArgs
{
    int _pathIndex;

    public PathChosenEventArgs(int pathIndex)
    {
        _pathIndex = pathIndex;
    }

    public int PathIndex => _pathIndex;
}
