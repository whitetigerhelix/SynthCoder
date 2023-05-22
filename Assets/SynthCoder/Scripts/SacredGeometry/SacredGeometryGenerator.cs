// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SynthCoder
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SacredGeometryGenerator : MonoBehaviour
    {
//TODO: Idea: Generate sacred shader effects, material

//TODO: More shapes: Platonic solids, etc
        public enum ShapeType { Cube, Sphere, Cylinder, Capsule, Cone, Pyramid, Torus, StarTetrahedron, FlowerOfLife }

        [SerializeField] private ShapeType Type = ShapeType.StarTetrahedron;

        [SerializeField] private float size = 1f;
        [SerializeField] private float height = 2f;
        [SerializeField] private float radius = 1f;
        [SerializeField] private int numDivisions = 32;
        [SerializeField] private Material material;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private GameObject container;

        private void OnEnable()
        {
            GenerateSacredGeometry();
        }

        private void OnDisable()
        {
            DestroyImmediate(container);
            container = null;
        }

        private void GenerateSacredGeometry()
        {
            DestroyImmediate(container);
            container = null;

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
                    meshFilter.mesh = GenerateSphereMesh(radius, numDivisions);
                    break;
                case ShapeType.Cylinder:
                    meshFilter.mesh = GenerateCylinderMesh(radius, height, numDivisions);
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
                    //meshFilter.mesh = GenerateStarTetrahedronMesh(size);
                    GenerateStarTetrahedronAsync();
                    break;
                case ShapeType.FlowerOfLife:
                    GenerateFlowerOfLifeAsync();
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
                0, 2, 1, // Bottom
                0, 3, 2,
                3, 6, 2, // Front
                3, 7, 6,
                7, 5, 6, // Top
                7, 4, 5,
                4, 1, 5, // Back
                4, 0, 1,
                1, 6, 5, // Right
                1, 2, 6,
                4, 3, 0, // Left
                4, 7, 3
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
        public static Mesh GenerateSphereMesh(float radius, int numDivisions)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

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

        // This function generates a Mesh for a cylinder with a given radius, height, and number of segments.
        // It calculates the vertices, triangles, normals, and UV coordinates for the cylinder, and then sets these values to a new Mesh object.
        // The vertices are positioned at even intervals around the circumference of the cylinder and have alternating heights to create a side face.
        // Two additional vertices are added for the top and bottom faces. The triangles are created using the vertices to form the faces of the cylinder,
        // with separate triangles for the side, top, and bottom faces. The normals are calculated to point outwards from the cylinder for lighting calculations.
        // Finally, UV coordinates are calculated to map the material onto the cylinder.
        public static Mesh GenerateCylinderMesh(float radius, float height, int numSegments)
        {
            Mesh mesh = new Mesh();

            // Vertices
            Vector3[] vertices = new Vector3[numSegments * 2 + 2]; // add 2 for top and bottom vertices
            float angleStep = 2 * Mathf.PI / numSegments;
            float angle = 0f;
            for (int i = 0; i < numSegments; i++)
            {
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                vertices[i * 2] = new Vector3(x, -height / 2f, z);
                vertices[i * 2 + 1] = new Vector3(x, height / 2f, z);
                angle += angleStep;
            }

            // add top and bottom vertices
            vertices[numSegments * 2] = Vector3.up * height / 2f;
            vertices[numSegments * 2 + 1] = Vector3.down * height / 2f;

            // Triangles
            int[] triangles = new int[numSegments * 6 + numSegments * 6]; // numSegments * 6 for side faces, and numSegments * 3 for top and bottom faces
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

                // Top face
                triangles[triangleIndex++] = baseIndex + 1;
                triangles[triangleIndex++] = numSegments * 2;
                triangles[triangleIndex++] = nextBaseIndex + 1;

                // Bottom face
                triangles[triangleIndex++] = nextBaseIndex;
                triangles[triangleIndex++] = numSegments * 2 + 1;
                triangles[triangleIndex++] = baseIndex;
            }

            // Normals
            Vector3[] normals = new Vector3[numSegments * 2 + 2];
            for (int i = 0; i < numSegments; i++)
            {
                normals[i * 2] = normals[i * 2 + 1] = new Vector3(Mathf.Cos(i * angleStep), 0f, Mathf.Sin(i * angleStep));
            }

            // Top and bottom normals
            normals[numSegments * 2] = Vector3.up;
            normals[numSegments * 2 + 1] = Vector3.down;

            // UVs
            Vector2[] uvs = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                uvs[i] = new Vector2(Mathf.Atan2(vertices[i].x, vertices[i].z) / (2f * Mathf.PI) + 0.5f, vertices[i].y / height + 0.5f);
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uvs;

            return mesh;
        }

//TODO: Capsule isn't working perfectly - the end caps are inverted 
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
/*
//TODO: StarTetrahedron isn't working quite right yet - it's a bit malformed
        // Generates a mesh for a star tetrahedron shape. It creates a tetrahedron with an apex at the center of the shape
        // and three base vertices that form an equilateral triangle. It then creates three pyramids by connecting the apex
        // to each of the base vertices, and one more pyramid by connecting the base vertices to each other. Finally, it sets
        // the normals of all faces to point upwards, and recalculates the bounds of the mesh
        public static Mesh GenerateStarTetrahedronMesh(float size)
        {
            Mesh mesh = new Mesh();

            float root2 = Mathf.Sqrt(2f);
            float root3 = Mathf.Sqrt(3f);

            Vector3[] vertices = new Vector3[]
            {
                // Base vertices of the tetrahedron
                new Vector3(-size / (2f * root2), 0f, size / (2f * root3)),
                new Vector3(size / (2f * root2), 0f, size / (2f * root3)),
                new Vector3(0f, 0f, -size / root3),

                // Apex of the tetrahedron
                new Vector3(0f, size / (2f * root3), 0f),

                // Base vertices of the pyramid connected to the first base vertex
                new Vector3(-size / (2f * root2), size / root3, size / (2f * root3)),
                new Vector3(-size / root2, 0f, 0f),
                new Vector3(0f, size / root3, 0f),

                // Base vertices of the pyramid connected to the second base vertex
                new Vector3(size / root2, 0f, 0f),
                new Vector3(size / (2f * root2), size / root3, size / (2f * root3)),
                new Vector3(0f, size / root3, 0f),

                // Base vertices of the pyramid connected to the third base vertex
                new Vector3(-size / (2f * root2), size / root3, -size / (2f * root3)),
                new Vector3(size / (2f * root2), size / root3, -size / (2f * root3)),
                new Vector3(0f, size / root3, 0f),
            };

            int[] triangles = new int[]
            {
                // Bottom pyramid
                0, 2, 1,
                0, 1, 3,
                1, 2, 3,
                2, 0, 3,

                // Pyramid connected to the first base vertex
                4, 6, 5,
                5, 6, 3,
                6, 4, 3,
                4, 5, 7,
                3, 6, 7,
                5, 3, 7,

                // Pyramid connected to the second base vertex
                7, 8, 5,
                5, 8, 3,
                8, 7, 3,
                7, 10, 8,
                3, 8, 10,
                5, 3, 10,

                // Pyramid connected to the third base vertex
                9, 11, 10,
                10, 11, 3,
                11, 9, 3,
                9, 10, 12,
                3, 11, 12,
                10, 3, 12,

                // Pyramid connecting all base vertices
                0, 1, 5,
                1, 8, 5,
                1, 2, 8,
                2, 10, 8,
                2, 0, 10,
                0, 5, 10,
                7, 9, 11,
                9, 10, 11,
                10, 12, 11,
                10, 5, 12,
                5, 7, 12,
                7, 11, 12
            };

            // Calculate normals for each vertex
            Vector3[] normals = new Vector3[vertices.Length];
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int i1 = triangles[i];
                int i2 = triangles[i + 1];
                int i3 = triangles[i + 2];
                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];
                Vector3 v3 = vertices[i3];
                Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;
                normals[i1] += normal;
                normals[i2] += normal;
                normals[i3] += normal;
            }
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i].normalized;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;

            // Recalculate bounds
            mesh.RecalculateBounds();

            return mesh;
        }
*/
//TODO: This version of star tetrahedron looks cool but still isn't quite right
        // Use pyramid mesh generation
        public static GameObject GenerateStarTetrahedron(float size, Material material)
        {
            GameObject starTetrahedron = new GameObject("Star Tetrahedron");

            float root2 = Mathf.Sqrt(2f);
            float root3 = Mathf.Sqrt(3f);
            float apexHeight = size / (2f * root3);
            float baseSize = size / root2;

            // Generate two pyramids for the tetrahedron
            for (int i = 0; i < 2; i++)
            {
                GameObject pyramid = new GameObject("Pyramid " + (i + 1));
                pyramid.transform.parent = starTetrahedron.transform;

                MeshFilter meshFilter = pyramid.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = pyramid.AddComponent<MeshRenderer>();
                meshRenderer.material = material;

                // Generate pyramid mesh
                Mesh pyramidMesh = GeneratePyramidMesh(baseSize, apexHeight);
                meshFilter.mesh = pyramidMesh;

                // Rotate pyramid to align with tetrahedron geometry
                pyramid.transform.Rotate(Vector3.right, 180f * i);

                // Invert odd numbered pyramids to create the star tetrahedron shape
                if (i % 2 == 1)
                {
                    meshRenderer.material.color = Color.red;    //TODO: Handy for debugging, but shouldn't change the color like this
                    meshFilter.mesh = InvertMesh(pyramidMesh);
                }
            }

            // Rotate one pyramid to form the diamond shape
            starTetrahedron.transform.GetChild(1).Rotate(Vector3.up, 36.8699f);

            return starTetrahedron;
        }

        // Can't parent the meshes until the following frame
        public async void GenerateStarTetrahedronAsync()
        {
            container = GenerateStarTetrahedron(size, material);

            await Task.Yield(); // Wait a frame

            container.transform.parent = gameObject.transform;
            container.transform.localPosition = Vector3.zero;
            container.transform.localRotation = Quaternion.identity;
        }

//TODO: GenerateFlowerOfLife3DMesh - use spheres instead of torus
//TODO: The generated flower of life isn't quite right, but looks fairly cool
        // This function defines a list of positions for the centers of the circles in the flower of life,
        // and then loops through each position, creating a torus mesh using the GenerateTorusMesh function
        // and adding it as a child to a parent game object.
        // The resulting parent game object is returned as the output of the function.
        public static GameObject GenerateFlowerOfLife(float radius, float innerRadius, Material material, int segments = 32, int sides = 32)
        {
            // Define the positions of the centers of the circles in the flower of life
            var positions = new List<Vector3>();
            positions.Add(Vector3.zero); // Center of the first circle
            positions.Add(new Vector3(radius * 2f, 0f, 0f)); // Center of the second circle
            positions.Add(new Vector3(radius, 0f, radius * Mathf.Sqrt(3))); // Center of the third circle
            positions.Add(new Vector3(-radius, 0f, radius * Mathf.Sqrt(3))); // Center of the fourth circle
            positions.Add(new Vector3(-radius * 2f, 0f, 0f)); // Center of the fifth circle
            positions.Add(new Vector3(-radius, 0f, -radius * Mathf.Sqrt(3))); // Center of the sixth circle
            positions.Add(new Vector3(radius, 0f, -radius * Mathf.Sqrt(3))); // Center of the seventh circle

            // Create a parent game object to hold all of the torus game objects
            var parentObject = new GameObject("Flower of Life Container");

            // Loop through each position and create a torus mesh
            foreach (var position in positions)
            {
                var torus = GenerateTorusMesh(radius, innerRadius, segments, sides);

                // Create a child game object to hold the torus mesh
                var childObject = new GameObject("Torus");
                childObject.transform.parent = parentObject.transform;
                childObject.transform.position = position;
                childObject.AddComponent<MeshFilter>().mesh = torus;
                var meshRenderer = childObject.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
            }

            return parentObject;
        }

        // Can't parent the meshes until the following frame
        public async void GenerateFlowerOfLifeAsync()
        {
            container = GenerateFlowerOfLife(radius, size, material, numDivisions, numDivisions);

            await Task.Yield(); // Wait a frame

            container.transform.parent = gameObject.transform;
            container.transform.localPosition = Vector3.zero;
            container.transform.localRotation = Quaternion.identity;
        }

        // This function takes in a Mesh object and returns a new inverted Mesh object.
        // It first creates new arrays for the inverted vertices and normals, and then
        // loops through each vertex to invert its position and normal. It also reverses the
        // triangle winding order to maintain proper surface orientation. Finally, it assigns
        // the new arrays to the inverted mesh, recalculates its bounds and tangents, and returns it.
        public static Mesh InvertMesh(Mesh originalMesh)
        {
            Mesh invertedMesh = new Mesh();

            // Create new arrays for inverted vertices and normals
            Vector3[] invertedVertices = new Vector3[originalMesh.vertexCount];
            Vector3[] invertedNormals = new Vector3[originalMesh.vertexCount];

            // Invert vertex positions and normals
            for (int i = 0; i < originalMesh.vertexCount; i++)
            {
                invertedVertices[i] = originalMesh.vertices[i] * -1f;
                invertedNormals[i] = originalMesh.normals[i] * -1f;
            }

            // Reverse the triangle winding order to maintain proper surface orientation
            int[] invertedTriangles = new int[originalMesh.triangles.Length];
            for (int i = 0; i < originalMesh.triangles.Length; i += 3)
            {
                invertedTriangles[i] = originalMesh.triangles[i + 2];
                invertedTriangles[i + 1] = originalMesh.triangles[i + 1];
                invertedTriangles[i + 2] = originalMesh.triangles[i];
            }

            // Assign new arrays to inverted mesh
            invertedMesh.vertices = invertedVertices;
            invertedMesh.triangles = invertedTriangles;
            invertedMesh.normals = invertedNormals;

            // Recalculate bounds and tangents
            invertedMesh.RecalculateBounds();
            invertedMesh.RecalculateTangents();

            return invertedMesh;
        }

        public static void DeleteChildren(GameObject obj)
        {
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
            {
                GameObject childObject = obj.transform.GetChild(i).gameObject;
                DestroyImmediate(childObject);
            }
        }
    }
}
