using UnityEngine;
using System.Collections.Generic;

public class MapHandler : MonoBehaviour
{
  public GameObject NodePrefab;
  public GameObject PathPrefab;
  public Material PathMaterial;

  public void Start()
  {
    var nodePositions = new Dictionary<Vector2, Vector3>();
    var nodes = new Dictionary<Vector2, GameObject>();

    for(var i = 0; i < 2; i++)
    {
      for(var j = 0; j < 5; j++)
      {
        nodePositions.Add(new Vector2(i, j), new Vector3((i * 4) - 2, (j * 2) - 4, 0));
      }
    }

    foreach (var position in nodePositions)
    {
      var node = Instantiate(NodePrefab, transform);
      node.transform.position = position.Value;
      nodes[position.Key] = node;
    }

    var path = Instantiate(PathPrefab);
    path.GetComponent<PathDrawer>().pathMaterial = PathMaterial;
    path.GetComponent<PathDrawer>().startPoint = nodes[new Vector2(0, 0)].transform;
    path.GetComponent<PathDrawer>().endPoint = nodes[new Vector2(1, 4)].transform;
    path.GetComponent<PathDrawer>().Draw();
    Debug.Log("Instantiated");
  }
}
