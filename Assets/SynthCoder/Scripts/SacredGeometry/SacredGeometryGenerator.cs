// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SacredGeometryGenerator : MonoBehaviour
{
//TODO: Platonic solids, etc
    public enum ShapeType { Cube, Sphere, Cylinder, Capsule, Cone, Pyramid, Torus, StarTetrahedron, FlowerOfLife }

//TODO: Generate sacred shader effects

    [SerializeField] private ShapeType Type = ShapeType.StarTetrahedron;

    [SerializeField] private float size = 1f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private int numDivisions = 32;
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
                meshFilter.mesh = GenerateSphereMesh(radius);
                break;
            case ShapeType.Cylinder:
                meshFilter.mesh = GenerateCylinderMesh(size, height, numDivisions);
                break;
            case ShapeType.Capsule:
                meshFilter.mesh = GenerateCapsuleMesh(radius, height, numDivisions);
                break;
            case ShapeType.Cone:
                meshFilter.mesh = GenerateConeMesh(radius, height, numDivisions);
                break;
            case ShapeType.Pyramid:
                meshFilter.mesh = GeneratePyramidMesh(size, height);
                break;
            case ShapeType.Torus:
                meshFilter.mesh = GenerateTorusMesh(radius, size);
                break;
            case ShapeType.StarTetrahedron:
                meshFilter.mesh = GenerateStarTetrahedronMesh(size);
                break;
            case ShapeType.FlowerOfLife:
                meshFilter.mesh = GenerateFlowerOfLifeMesh(size);
                break;
        }
    }

    // This function generates a Mesh object representing a cube of the given size.
    // The cube is created by defining an array of vertices and an array of triangles,
    // and setting them on the Mesh object. The vertices are defined as eight corner points of the cube,
    // with two adjacent corners forming a side of the cube. The triangles are defined as sets of three indices
    // into the vertices array, forming the six faces of the cube. The function then recalculates the normals
    // of the Mesh to ensure correct lighting, and returns the Mesh object.
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
    
    // This implementation generates a sphere mesh by first dividing the sphere into a set of rings and dividing each ring into a set of vertices.
    // It then generates triangles between adjacent vertices to form a grid of triangles that covers the sphere.
    // The numDivisions parameter controls the resolution of the mesh.
    // Higher values will produce a smoother sphere with more triangles.
    public static Mesh GenerateSphereMesh(float radius)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int numDivisions = 8;

        // Generate vertices
        for (int i = 0; i <= numDivisions; i++)
        {
            float y = Mathf.Cos(Mathf.PI * i / numDivisions) * radius;
            float ringRadius = Mathf.Sin(Mathf.PI * i / numDivisions) * radius;
            for (int j = 0; j <= numDivisions; j++)
            {
                float x = Mathf.Cos(2 * Mathf.PI * j / numDivisions) * ringRadius;
                float z = Mathf.Sin(2 * Mathf.PI * j / numDivisions) * ringRadius;
                vertices.Add(new Vector3(x, y, z));
            }
        }

        // Generate triangles
        for (int i = 0; i < numDivisions; i++)
        {
            for (int j = 0; j < numDivisions; j++)
            {
                int first = (i * (numDivisions + 1)) + j;
                int second = first + numDivisions + 1;
                triangles.Add(first);
                triangles.Add(second);
                triangles.Add(first + 1);

                triangles.Add(second);
                triangles.Add(second + 1);
                triangles.Add(first + 1);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

//TODO: Cylinder doesn't work
    // This function generates a cylinder mesh with a given radius, height, and number of segments.
    // The mesh is represented as a list of vertices and a list of indices for the faces.
    // The vertices are arranged in a circle around the base of the cylinder, another circle around the top,
    // and two center points at the top and bottom. The faces are generated by connecting the vertices in a specific pattern.
    public static Mesh GenerateCylinderMesh(float radius, float height, int numSegments)
    {
        Mesh mesh = new Mesh();

        // Vertices
        Vector3[] vertices = new Vector3[numSegments * 2 + 2];
        float angleStep = 2 * Mathf.PI / numSegments;
        float angle = 0f;
        for (int i = 0; i <= numSegments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            vertices[i] = new Vector3(x, height / 2f, z);
            vertices[i + numSegments + 1] = new Vector3(x, -height / 2f, z);
            angle += angleStep;
        }

        // Triangles
        int[] triangles = new int[numSegments * 12];
        int triangleIndex = 0;
        for (int i = 0; i < numSegments; i++)
        {
            int baseIndex = i * 2;
            int nextBaseIndex = ((i + 1) % numSegments) * 2;

            // Side faces
            triangles[triangleIndex++] = baseIndex;
            triangles[triangleIndex++] = baseIndex + 1;
            triangles[triangleIndex++] = nextBaseIndex + 1;

            triangles[triangleIndex++] = baseIndex;
            triangles[triangleIndex++] = nextBaseIndex + 1;
            triangles[triangleIndex++] = nextBaseIndex;

            // Top faces
            triangles[triangleIndex++] = baseIndex + 1;
            triangles[triangleIndex++] = vertices.Length - 2;
            triangles[triangleIndex++] = nextBaseIndex + 1;

            // Bottom faces
            triangles[triangleIndex++] = baseIndex;
            triangles[triangleIndex++] = nextBaseIndex;
            triangles[triangleIndex++] = vertices.Length - 1;
        }

        // Normals
        Vector3[] normals = new Vector3[vertices.Length];
        for (int i = 0; i < numSegments; i++)
        {
            normals[i * 2] = normals[i * 2 + 1] = new Vector3(Mathf.Cos(i * angleStep), 0f, Mathf.Sin(i * angleStep));
        }
        normals[vertices.Length - 2] = Vector3.up;
        normals[vertices.Length - 1] = Vector3.down;

        // UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius * 0.5f + 0.5f, vertices[i].z / radius * 0.5f + 0.5f);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;

        return mesh;
    }

    //TODO: Capsule doesn't work
    // Generates a capsule mesh with given size, using a sphere mesh for the rounded ends and a cylinder mesh for the middle.
    public static Mesh GenerateCapsuleMesh(float radius, float height, int nbSides)
    {
        Mesh mesh = new Mesh();

        // Compute angles and steps
        float halfHeight = height / 2f;
        int nbHeightSeg = Mathf.RoundToInt(nbSides * height / (2 * radius));
        float angleStep = Mathf.PI / nbHeightSeg;
        float sideStep = Mathf.PI * 2f / nbSides;

        // Generate vertices
        List<Vector3> verticesList = new List<Vector3>();
        List<Vector3> normalsList = new List<Vector3>();
        List<Vector2> uvList = new List<Vector2>();
        for (int i = 0; i <= nbHeightSeg; i++)
        {
            float h = Mathf.Clamp((i * height / nbHeightSeg) - halfHeight, -halfHeight, halfHeight);
            float r = Mathf.Sqrt(radius * radius - h * h);
            for (int j = 0; j <= nbSides; j++)
            {
                float angle = j * sideStep;
                Vector3 vertexPos;
                if (i < nbHeightSeg / 2)
                {
                    vertexPos = new Vector3(Mathf.Sin(angle) * r, Mathf.Cos(angle) * r, h + halfHeight - radius);
                }
                else
                {
                    vertexPos = new Vector3(Mathf.Sin(angle) * r, Mathf.Cos(angle) * r, h - halfHeight + radius);
                }
                verticesList.Add(vertexPos);
                normalsList.Add(vertexPos.normalized);
                uvList.Add(new Vector2(j / (float)nbSides, i / (float)nbHeightSeg));
            }
        }

        // Generate triangles
        List<int> trianglesList = new List<int>();
        int nbVerticesPerCircle = nbSides + 1;
        for (int i = 0; i < nbHeightSeg; i++)
        {
            for (int j = 0; j < nbSides; j++)
            {
                int vertexIndex = i * nbVerticesPerCircle + j;
                if (i == 0)
                {
                    int nextIndex = vertexIndex + 1;
                    trianglesList.Add(nextIndex);
                    trianglesList.Add(vertexIndex);
                    trianglesList.Add(nbVerticesPerCircle * (nbHeightSeg + 1) - nbVerticesPerCircle + j);
                }
                else if (i == nbHeightSeg - 1)
                {
                    int nextIndex = vertexIndex + 1;
                    trianglesList.Add(nextIndex);
                    trianglesList.Add(vertexIndex);
                    trianglesList.Add(nbVerticesPerCircle * nbHeightSeg + j);
                }
                else
                {
                    int nextIndex = vertexIndex + 1;
                    int nextCircleIndex = vertexIndex + nbVerticesPerCircle;
                    int nextCircleNextIndex = nextCircleIndex + 1;
                    trianglesList.Add(vertexIndex);
                    trianglesList.Add(nextIndex);
                    trianglesList.Add(nextCircleIndex);
                    trianglesList.Add(nextCircleIndex);
                    trianglesList.Add(nextIndex);
                    trianglesList.Add(nextCircleNextIndex);
                }
            }
        }

        mesh.vertices = verticesList.ToArray();
        mesh.normals = normalsList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.triangles = trianglesList.ToArray();

        mesh.RecalculateBounds();

        return mesh;
    }

    // This implementation creates a cone mesh with the specified size, which consists of a bottom circle,
    // a top vertex, and sides connecting the two.The mesh is generated using vertices, indices, and UV coordinates,
    // which are then used to create the mesh object.
    public static Mesh GenerateConeMesh(float radius, float height, int numDivisions)
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        // Create the bottom vertex
        vertices.Add(Vector3.zero);
        uvs.Add(new Vector2(0.5f, 0.5f));

        // Create the side vertices
        float size = radius * 2f;
        float angleIncrement = (2f * Mathf.PI) / numDivisions;
        for (int i = 0; i <= numDivisions; i++)
        {
            float angle = i * angleIncrement;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 vertex = new Vector3(x, 0f, z);
            vertices.Add(vertex);
            float u = (x / size) + 0.5f;
            float v = (z / size) + 0.5f;
            uvs.Add(new Vector2(u, v));
        }

        // Create the indices for the bottom and sides
        for (int i = 1; i <= numDivisions; i++)
        {
            indices.Add(0);
            indices.Add(i);
            indices.Add(i + 1);
        }

        // Create the top vertex
        vertices.Add(new Vector3(0f, height, 0f));
        uvs.Add(new Vector2(0.5f, 0.5f));

        // Create the indices for the top and sides
        int topVertexIndex = vertices.Count - 1;
        for (int i = 1; i <= numDivisions; i++)
        {
            int index = i == numDivisions ? 1 : i + 1;
            indices.Add(index);
            indices.Add(i);
            indices.Add(topVertexIndex);
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        mesh.SetUVs(0, uvs);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }


    // This function creates a mesh representing a pyramid with the given base size and height.
    // The function first defines the vertices of the pyramid, including the apex and four base corners.
    // Then, it defines the triangles of the pyramid, connecting the vertices to form the faces.
    // The function also defines UVs for texturing the mesh and normals for lighting the mesh.
    // Finally, the mesh is scaled to the given height, and the bounds and normals are recalculated before returning the generated mesh.
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

    // This implementation generates a torus mesh with segments and sides number of segments in each direction.
    // The radius parameter determines the overall radius of the torus, while the innerRadius parameter determines
    // the radius of the inner part of the torus shape. The function first generates all the vertices and UV coordinates,
    // and then uses them to generate the triangles for the mesh. Finally, the function recalculates the normals and bounds
    // of the mesh and returns it.
    public static Mesh GenerateTorusMesh(float radius, float innerRadius, int segments = 32, int sides = 32)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[(segments + 1) * (sides + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[segments * sides * 6];

        float segmentAngle = Mathf.PI * 2f / segments;
        float sideAngle = Mathf.PI * 2f / sides;

        int vertexIndex = 0;
        int triangleIndex = 0;

        // Generate vertices and UVs
        for (int side = 0; side <= sides; side++)
        {
            float cosSide = Mathf.Cos(side * sideAngle);
            float sinSide = Mathf.Sin(side * sideAngle);

            for (int segment = 0; segment <= segments; segment++)
            {
                float cosSegment = Mathf.Cos(segment * segmentAngle);
                float sinSegment = Mathf.Sin(segment * segmentAngle);

                // Calculate position and UV coordinates
                float x = (radius + innerRadius * cosSegment) * cosSide;
                float y = innerRadius * sinSegment;
                float z = (radius + innerRadius * cosSegment) * sinSide;
                vertices[vertexIndex] = new Vector3(x, y, z);
                uv[vertexIndex] = new Vector2((float)segment / segments, (float)side / sides);
                vertexIndex++;
            }
        }

        // Generate triangles
        for (int side = 0; side < sides; side++)
        {
            for (int segment = 0; segment < segments; segment++)
            {
                int currentVertex = side * (segments + 1) + segment;
                int nextVertex = currentVertex + segments + 1;

                // First triangle
                triangles[triangleIndex] = currentVertex;
                triangles[triangleIndex + 1] = currentVertex + 1;
                triangles[triangleIndex + 2] = nextVertex;

                // Second triangle
                triangles[triangleIndex + 3] = nextVertex;
                triangles[triangleIndex + 4] = currentVertex + 1;
                triangles[triangleIndex + 5] = nextVertex + 1;

                triangleIndex += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }


//TODO: StarTetrahedron doesn't work
    // Generates a mesh for a star tetrahedron shape. It creates a tetrahedron with an apex at the center of the shape
    // and three base vertices that form an equilateral triangle. It then creates three pyramids by connecting the apex
    // to each of the base vertices, and one more pyramid by connecting the base vertices to each other. Finally, it sets
    // the normals of all faces to point upwards, and recalculates the bounds of the mesh
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
            2, 0, 3 // Back pyramid
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

//TODO: FoL doesn't work
    // The Flower of Life mesh is generated using a series of circles with increasing radius,
    // and each circle is divided into six segments. Points are added at the intersection of
    // overlapping circles and at the intersection of every third circle. Triangles are then
    // created between these points and the vertices on the two adjacent circles.
    // The resulting mesh is then returned.
    public static Mesh GenerateFlowerOfLifeMesh(float size)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        // Constants used for Flower of Life generation
        const float angleBetweenCircles = Mathf.PI / 6f;
        const int numCircles = 6;
        const int numPointsPerCircle = 6;

        // Generate the first circle
        vertices.Add(Vector3.zero);
        for (int i = 0; i < numPointsPerCircle; i++)
        {
            float angle = i * Mathf.PI * 2f / numPointsPerCircle;
            vertices.Add(new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * size);
            indices.Add(0);
            indices.Add(i + 1);
            indices.Add((i + 1) % numPointsPerCircle + 1);
        }

        // Generate the remaining circles
        for (int i = 1; i < numCircles; i++)
        {
            float radius = i * angleBetweenCircles * size;

            for (int j = 0; j < numPointsPerCircle; j++)
            {
                float angle = j * Mathf.PI * 2f / numPointsPerCircle;
                Vector3 center = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;

                bool isOverlapCircle = i % 2 == 0 && j % 2 == 0;
                bool isIntersectionPoint = i % 3 == 2 && j % 3 == 2;

                if (isOverlapCircle || isIntersectionPoint)
                {
                    vertices.Add(center);
                    int currentIndex = vertices.Count - 1;

                    // Connect the new point to the six points on the previous circle
                    int numPointsOnPrevCircle = numPointsPerCircle + (isOverlapCircle ? 0 : 1);
                    int prevCircleStartIndex = vertices.Count - numPointsOnPrevCircle;
                    for (int k = 0; k < numPointsOnPrevCircle; k++)
                    {
                        int nextIndex = prevCircleStartIndex + (k % numPointsPerCircle);
                        indices.Add(currentIndex);
                        indices.Add(nextIndex);
                        indices.Add((nextIndex + 1) % numPointsOnPrevCircle + prevCircleStartIndex);
                    }

                    // Connect the new point to the six points on the current circle
                    int currentCircleStartIndex = vertices.Count - numPointsPerCircle;
                    for (int k = 0; k < numPointsPerCircle; k++)
                    {
                        int nextIndex = currentCircleStartIndex + k;
                        indices.Add(currentIndex);
                        indices.Add((nextIndex + 1) % numPointsPerCircle + currentCircleStartIndex);
                        indices.Add(nextIndex);
                    }
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        return mesh;
    }
}
