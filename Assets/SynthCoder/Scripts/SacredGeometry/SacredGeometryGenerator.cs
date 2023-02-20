// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SacredGeometryGenerator : MonoBehaviour
{
    public enum ShapeType { Cube, Sphere, Cylinder, Capsule, Pyramid, StarTetrahedron }

    [SerializeField] private ShapeType Type = ShapeType.StarTetrahedron;

    [SerializeField] private float size = 1f;
    [SerializeField] private Material material;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void OnEnable()
    {
        GenerateSacredGeometry();
    }

    private void GenerateSacredGeometry()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer.material = material;

        switch (Type)
        {
            case ShapeType.Cube:
                meshFilter.mesh = GenerateCubeMesh(size);
                break;
            case ShapeType.Sphere:
                meshFilter.mesh = GenerateSphereMesh(size);
                break;
            case ShapeType.Cylinder:
                meshFilter.mesh = GenerateCylinderMesh(size);
                break;
            case ShapeType.Capsule:
                meshFilter.mesh = GenerateCapsuleMesh(size);
                break;
            case ShapeType.Pyramid:
                meshFilter.mesh = GeneratePyramidMesh(size);
                break;
            case ShapeType.StarTetrahedron:
                meshFilter.mesh = GenerateStarTetrahedronMesh(size);
                break;
        }
    }

    public static Mesh GenerateCubeMesh(float size)
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

    public static Mesh GenerateSphereMesh(float size)
    {
        //TODO: Implement sphere mesh generation
        return null;
    }

    public static Mesh GenerateCylinderMesh(float size)
    {
        //TODO: Implement cylinder mesh generation
        return null;
    }

    public static Mesh GenerateCapsuleMesh(float size)
    {
        //TODO: Implement capsule mesh generation
        return null;
    }

    public static Mesh GeneratePyramidMesh(float size)
    {
        //TODO: Implement pyramid mesh generation
        return null;
    }

    public static Mesh GenerateStarTetrahedronMesh(float size)
    {
        Mesh mesh = new Mesh();

        float root2 = Mathf.Sqrt(2f);
        float root3 = Mathf.Sqrt(3f);
        float root6 = Mathf.Sqrt(6f);

        Vector3[] vertices = new Vector3[] {
            new Vector3(0f, 0f, size / root3), // Apex
            new Vector3(size / root2, 0f, -size / (2f * root3)), // Base vertex 1
            new Vector3(-size / (2f * root2), size / 2f, -size / (2f * root3)), // Base vertex 2
            new Vector3(-size / (2f * root2), -size / 2f, -size / (2f * root3)) // Base vertex 3
        };

        int[] triangles = new int[] {
            0, 1, 2, // Bottom pyramid
            0, 2, 3,
            0, 3, 1,
            1, 2, 3, // Top pyramid
        };

        Vector3[] normals = new Vector3[] {
            -Vector3.forward, -Vector3.forward, -Vector3.forward, // Bottom pyramid
            Vector3.forward, Vector3.forward, Vector3.forward // Top pyramid
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        return mesh;
    }
}
