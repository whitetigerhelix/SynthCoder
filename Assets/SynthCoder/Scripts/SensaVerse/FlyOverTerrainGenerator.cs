// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    public class FlyOverTerrainGenerator : TerrainGenerator
    {
        [Header("Scrolling")]

        [SerializeField] private Vector2 scrollDirection = new Vector2(1f, 1f);
        [SerializeField] private float scrollSpeed = 10f;

        [SerializeField] private TiltWithCamera cameraTilt;

        public Vector2 ScrollDirection => cameraTilt != null ? cameraTilt.Direction : scrollDirection;

        public Vector2 ScrollOffset => scrollSpeed * Time.time * ScrollDirection;

        protected override void Update()
        {
            base.Update();

            GenerateTerrainMesh();
        }

        protected override void UpdateShader()
        {
            base.UpdateShader();

            if (terrainMaterial != null)
            {
                terrainMaterial.SetVector("_ScrollDirection", ScrollOffset);
            }
        }

        protected override void GenerateTerrainMesh()
        {
            int numVertices = terrainWidth * terrainDepth;
            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uv = new Vector2[numVertices];
            int[] triangles = new int[(terrainWidth - 1) * (terrainDepth - 1) * 6];

            var scrollOffset = ScrollOffset;
            int index = 0;
            for (int z = 0; z < terrainDepth; z++)
            {
                for (int x = 0; x < terrainWidth; x++)
                {
                    float perlinX = (x + heightNoiseOffset.x + scrollOffset.x) / (float)terrainWidth * heightScale;
                    float perlinY = (z + heightNoiseOffset.y + scrollOffset.y) / (float)terrainDepth * heightScale;
                    float y = Mathf.PerlinNoise(perlinX, perlinY);
                    y *= heightMultiplier;

                    vertices[index] = new Vector3(x, y, z);
                    uv[index] = new Vector2(x / (float)terrainWidth, z / (float)terrainDepth);

                    if (x < terrainWidth - 1 && z < terrainDepth - 1)
                    {
                        int topLeft = index;
                        int topRight = index + 1;
                        int bottomLeft = index + terrainWidth;
                        int bottomRight = index + terrainWidth + 1;

                        triangles[(x + z * (terrainWidth - 1)) * 6 + 0] = topLeft;
                        triangles[(x + z * (terrainWidth - 1)) * 6 + 1] = bottomLeft;
                        triangles[(x + z * (terrainWidth - 1)) * 6 + 2] = topRight;
                        triangles[(x + z * (terrainWidth - 1)) * 6 + 3] = topRight;
                        triangles[(x + z * (terrainWidth - 1)) * 6 + 4] = bottomLeft;
                        triangles[(x + z * (terrainWidth - 1)) * 6 + 5] = bottomRight;
                    }

                    index++;
                }
            }

            // Update the terrain mesh
            terrainMesh.vertices = vertices;
            terrainMesh.uv = uv;
            terrainMesh.triangles = triangles;
            terrainMesh.RecalculateNormals();
            terrainMesh.RecalculateBounds();
        }
    }
}
