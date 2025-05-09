using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MapHandler : MonoBehaviour
{
  public GameObject Canvas;
  public GameObject NodePrefab;
  public GameObject PathPrefab;
  public Material PathMaterial;

  public RectTransform linePrefab;   // Assign a prefab of a thin Image (e.g., 1x1 px)
  public RectTransform canvas;       // Your main canvas

  class MapNode
  {
    public Vector2 Position;
    public GameObject GameObject;
    public List<Vector2> Neighbors;
  }

  public void Start()
  {
    DrawLine(new Vector2(0, 0), new Vector2(1, 4));
    var map = new List<MapNode> {
      new MapNode { Position = new Vector2(0, 0), Neighbors = new List<Vector2> { new Vector2(0, 1), new Vector2(1, 1) } },

      new MapNode { Position = new Vector2(0, 1), Neighbors = new List<Vector2> { new Vector2(0, 2), new Vector2(1, 2) } },
      new MapNode { Position = new Vector2(1, 1), Neighbors = new List<Vector2> { new Vector2(2, 2) } },

      new MapNode { Position = new Vector2(0, 2), Neighbors = new List<Vector2> { new Vector2(0, 3), new Vector2(1, 3) } },
      new MapNode { Position = new Vector2(1, 2), Neighbors = new List<Vector2>() },
      new MapNode { Position = new Vector2(2, 2), Neighbors = new List<Vector2> { new Vector2(2, 3) } },

      new MapNode { Position = new Vector2(0, 3), Neighbors = new List<Vector2> { new Vector2(0, 4) } },
      new MapNode { Position = new Vector2(1, 3), Neighbors = new List<Vector2> { new Vector2(0, 4) } },
      
      new MapNode { Position = new Vector2(0, 4), Neighbors = new List<Vector2>() },
    };

    map.ForEach(node => {
      var nodesPerRow = map.Where(x => x.Position.y == node.Position.y).Count(x => x.Position.x == node.Position.x);
      var totalColumns = map.Max(x => x.Position.y);

      node.GameObject = Instantiate(NodePrefab, transform);
      node.GameObject.transform.position = new Vector3((node.Position.x * 2) - (nodesPerRow-1)*2, (node.Position.y * 2) - (totalColumns), 0);
    });

    // map.ForEach(node => {
    //   node.Neighbors.ForEach(neighbor => {
    //     DrawLine(node.Position, neighbor);
    //   });
    // });
  }

  public void DrawLine(Vector2 start, Vector2 end)
  {
      // Create line object
      RectTransform line = Instantiate(linePrefab, canvas);
      
      // Set position to midpoint
      Vector2 direction = end - start;
      Vector2 midpoint = (start + end) / 2f;
      line.position = midpoint;

      // Set size
      float length = direction.magnitude;
      line.sizeDelta = new Vector2(length, line.sizeDelta.y);

      // Set rotation
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      line.rotation = Quaternion.Euler(0, 0, angle);
  }
}
