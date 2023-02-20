using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SacredGeometryGenerator : MonoBehaviour
{

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public int size = 1;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.mesh = GenerateMesh();
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, size),
            new Vector3(size, 0, size),
            new Vector3(size, 0, 0),
            new Vector3(0, size, 0),
            new Vector3(0, size, size),
            new Vector3(size, size, size),
            new Vector3(size, size, 0)
        };

        int[] triangles = new int[] {
            0, 1, 2, // Bottom
            0, 2, 3,
            3, 2, 6, // Front
            3, 6, 7,
            7, 6, 5, // Top
            7, 5, 4,
            4, 5, 1, // Back
            4, 1, 0,
            1, 5, 6, // Right
            1, 6, 2,
            4, 0, 3, // Left
            4, 3, 7
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
