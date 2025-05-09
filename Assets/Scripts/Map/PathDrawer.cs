using UnityEngine;
using System.Collections.Generic;

public class PathDrawer : MonoBehaviour
{
    public Material pathMaterial;
    public float pathWidth = 20f;
    public int segments = 50;

    public Transform startPoint;
    public Transform endPoint;

    void Start()
    {
    }

    public void Draw()
    {
        List<Vector3> bezierPoints = GetBezierPoints(startPoint.position, endPoint.position, segments);
        Mesh pathMesh = GeneratePathMesh(bezierPoints);
        var mf = gameObject.AddComponent<MeshFilter>();
        var mr = gameObject.AddComponent<MeshRenderer>();
        mf.mesh = pathMesh;
        mr.material = pathMaterial;
    }

    List<Vector3> GetBezierPoints(Vector3 p0, Vector3 p2, int segments)
    {
        Vector3 dir = (p2 - p0).normalized;
        Vector3 midpoint = (p0 + p2) / 2;
        Vector3 p1 = midpoint + Vector3.Cross(dir, Vector3.forward) * 2f; // control point

        var points = new List<Vector3>();
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 point = Mathf.Pow(1 - t, 2) * p0 +
                            2 * (1 - t) * t * p1 +
                            Mathf.Pow(t, 2) * p2;
            points.Add(point);
        }
        return points;
    }

    Mesh GeneratePathMesh(List<Vector3> points)
    {
        int vertCount = points.Count * 2;
        Vector3[] vertices = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
        int[] triangles = new int[(points.Count - 1) * 6];

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < points.Count - 1) forward += points[i + 1] - points[i];
            if (i > 0) forward += points[i] - points[i - 1];
            forward.Normalize();

            Vector3 left = Vector3.Cross(forward, Vector3.forward).normalized;
            vertices[i * 2] = points[i] + left * pathWidth * 0.5f;
            vertices[i * 2 + 1] = points[i] - left * pathWidth * 0.5f;

            float uvX = i / (float)(points.Count - 1);
            uvs[i * 2] = new Vector2(uvX, 0);
            uvs[i * 2 + 1] = new Vector2(uvX, 1);
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            int idx = i * 6;
            int v = i * 2;
            triangles[idx] = v;
            triangles[idx + 1] = v + 2;
            triangles[idx + 2] = v + 1;

            triangles[idx + 3] = v + 1;
            triangles[idx + 4] = v + 2;
            triangles[idx + 5] = v + 3;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}