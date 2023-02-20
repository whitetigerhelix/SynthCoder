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
                meshFilter.mesh = GeneratePyramidMesh(size, size);  //TODO: Height param
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

    public static Mesh GeneratePyramidMesh(float baseSize, float height)
    {
        var mesh = new Mesh();

        // Define vertices of the pyramid
        var vertices = new Vector3[5];
        vertices[0] = new Vector3(0f, height, 0f);                          // Apex
        vertices[1] = new Vector3(-baseSize / 2f, 0f, baseSize / 2f);       // Front left corner
        vertices[2] = new Vector3(baseSize / 2f, 0f, baseSize / 2f);        // Front right corner
        vertices[3] = new Vector3(baseSize / 2f, 0f, -baseSize / 2f);       // Back right corner
        vertices[4] = new Vector3(-baseSize / 2f, 0f, -baseSize / 2f);      // Back left corner

        // Define triangles of the pyramid
        var triangles = new int[18];
        // Front face
        triangles[0] = 0; triangles[1] = 1; triangles[2] = 2;
        // Right face
        triangles[3] = 0; triangles[4] = 2; triangles[5] = 3;
        // Back face
        triangles[6] = 0; triangles[7] = 3; triangles[8] = 4;
        // Left face
        triangles[9] = 0; triangles[10] = 4; triangles[11] = 1;
        // Bottom face
        triangles[12] = 4; triangles[13] = 3; triangles[14] = 2;
        triangles[15] = 4; triangles[16] = 2; triangles[17] = 1;

        // Define UVs for texturing the mesh
        var uvs = new Vector2[5];
        uvs[0] = new Vector2(0.5f, 1f);     // Apex
        uvs[1] = new Vector2(0f, 0f);       // Front left corner
        uvs[2] = new Vector2(1f, 0f);       // Front right corner
        uvs[3] = new Vector2(1f, 1f);       // Back right corner
        uvs[4] = new Vector2(0f, 1f);       // Back left corner

        // Define normals for lighting the mesh
        var normals = new Vector3[5];
        normals[0] = Vector3.up;            // Apex
        normals[1] = Vector3.up;            // Front left corner
        normals[2] = Vector3.up;            // Front right corner
        normals[3] = Vector3.up;            // Back right corner
        normals[4] = Vector3.up;            // Back left corner

        // Set up the mesh with the defined vertices, triangles, UVs, and normals
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;

        // Scale the mesh to the given height
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y *= height;
        }
        mesh.vertices = vertices;

        // Recalculate the bounds and the normals of the mesh
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh GenerateStarTetrahedronMesh(float size)
    {
        Mesh mesh = new Mesh();

        float root2 = Mathf.Sqrt(2f);
        float root3 = Mathf.Sqrt(3f);
        float root6 = Mathf.Sqrt(6f);

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, size / (2f * root3), 0f), // Apex
            new Vector3(-size / (2f * root2), 0f, size / (2f * root3)), // Base vertex 1
            new Vector3(size / (2f * root2), 0f, size / (2f * root3)), // Base vertex 2
            new Vector3(0f, -size / root3, 0f) // Base vertex 3
        };

        int[] triangles = new int[]
        {
            0, 2, 1, // Bottom pyramid
            0, 1, 3,
            1, 2, 3, // Front pyramid
            0, 3, 2 // Back pyramid
        };

        Vector3[] normals = new Vector3[]
        {
            Vector3.up, Vector3.up, Vector3.up, Vector3.up // All faces point up
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.RecalculateBounds();

        return mesh;
    }
}
