using System;
using System.Collections.Generic;
using UnityEngine;

public class ContextManager
{
  IDictionary<ContextObjects, GameObject> _gameObjects;
  IDictionary<ContextObjects, Camera> _cameras;
  IDictionary<ContextObjects, IContextObject> _contextObjects;

  public ContextManager(IDictionary<ContextObjects, GameObject> gameObjects,
      IDictionary<ContextObjects, Camera> cameras,
      IDictionary<ContextObjects, IContextObject> contextObjects)
  {
    _gameObjects = gameObjects;
    _cameras = cameras;
    _contextObjects = contextObjects;
  }

  public T Get<T>(ContextObjects contextObject) where T : class
  {
    switch(typeof(T).Name) {
      case nameof(GameObject):
        return _gameObjects[contextObject] as T;
      case nameof(Camera):
        return _cameras[contextObject] as T;
      case nameof(IContextObject):
        return _contextObjects[contextObject] as T;
    }

    return null;
  }
}
