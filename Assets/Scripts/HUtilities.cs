using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HUtilities
{
  static System.Random rng = new System.Random();

  // Xeno's paradox baybeee
  // On reflection this might just be "lerp"
  public static float MoveTowards(float startPoint, float endPoint, float speed) {
      return Math.Min(
          Math.Max(((endPoint - startPoint) / 2), -speed * Time.deltaTime),
          speed * Time.deltaTime
      ) + startPoint;
  }

  public static void Shuffle<T>(this IList<T> list)  
  {  
    int n = list.Count;  
    while (n > 1) {  
      n--;  
      int k = rng.Next(n + 1);  
      T value = list[k];  
      list[k] = list[n];  
      list[n] = value;  
    }  
  }

  public static bool InPosition(Vector3 gameObjectPosition, Vector2 target)
  {
    return Math.Abs(gameObjectPosition.x - target.x) < 0.1f && Math.Abs(gameObjectPosition.y - target.y) < 0.1f;
  }

  public static bool DidMouseDown() => Input.GetKeyDown("space") || Input.GetMouseButtonDown(0) ||
                                       Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
  
  public static bool IsMouseDown() => Input.GetMouseButton(0) || Input.touchCount > 0;
  
  public static Vector2 MousePosition() => Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;

  public class Countdown
  {
    float _timeLength;
    Action _func;

    public Countdown(float timeLength, Action func)
    {
      _timeLength = timeLength;
      _func = func;
    }

    public bool Update()
    {
      if(_timeLength > 0 && _timeLength <= Time.deltaTime)
      {
        _timeLength = 0;
        _func();
        return false;
      }
      _timeLength -= Time.deltaTime;
      return _timeLength > 0;
    }

    public bool Running()
    {
      return _timeLength > 0;
    }
  }
}
