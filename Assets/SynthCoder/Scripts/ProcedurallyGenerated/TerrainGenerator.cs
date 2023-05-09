// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain")]

    public int terrainWidth = 32;
    public int terrainDepth = 32;

    public float heightScale = 8f;
    public float heightMultiplier = 8f;
    public Vector2 heightNoiseOffset = Vector2.zero;

    [Header("Texture")]

    public float terrainTextureFrequency = 0.0075f;

    public Color gradientStartColor = Color.red;
    public Color gradientEndColor = Color.blue;

    [Header("Shader")]

    public Shader terrainShader;

    protected Mesh terrainMesh;
    protected MeshRenderer terrainRenderer;
    protected MeshFilter terrainMeshFilter;
    protected Material terrainMaterial;
    protected Texture2D terrainTexture;
    protected Texture2D gradientTexture;
    protected Texture2D noiseTexture;
    protected string terrainTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/TerrainTexture.asset";
    protected string gradientTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/GradientTexture.asset";
    protected string noiseTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/NoiseTexture.asset";

    protected void Awake()
    {
        terrainRenderer = GetComponent<MeshRenderer>();
        terrainMeshFilter = GetComponent<MeshFilter>();
    }

    protected void OnEnable()
    {
        GenerateTerrain();
    }

    protected void GenerateTerrain()
    {
        // Create the terrain
        DestroyImmediate(terrainMesh);
        terrainMesh = new Mesh();
        terrainMeshFilter.mesh = terrainMesh;

        // Generate terrain information
        int numVertices = terrainWidth * terrainDepth;
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangles = new int[(terrainWidth - 1) * (terrainDepth - 1) * 6];

        int index = 0;

        for (int z = 0; z < terrainDepth; z++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                float perlinX = (x + heightNoiseOffset.x) / (float)terrainWidth * heightScale;
                float perlinY = (z + heightNoiseOffset.y) / (float)terrainDepth * heightScale;
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

        // Update terrain material and textures
        DestroyImmediate(terrainMaterial);
        terrainMaterial = new Material(terrainShader);
        terrainRenderer.material = terrainMaterial;

        DestroyImmediate(terrainTexture);
        terrainTexture = TextureGenerator.GenerateTerrainTexture(frequency: terrainTextureFrequency);
        TextureGenerator.ExportTextureToFile(terrainTexture, terrainTextureSavePath);

        DestroyImmediate(gradientTexture);
        gradientTexture = TextureGenerator.GenerateGradientTexture(gradientStartColor, gradientEndColor);
        TextureGenerator.ExportTextureToFile(gradientTexture, gradientTextureSavePath);

        DestroyImmediate(noiseTexture);
        noiseTexture = TextureGenerator.GenerateNoiseTexture();
        TextureGenerator.ExportTextureToFile(noiseTexture, noiseTextureSavePath);

        terrainMaterial.shader = terrainShader;
        terrainMaterial.SetTexture("_MainTex", terrainTexture);
        terrainMaterial.SetTexture("_Gradient", gradientTexture);
        terrainMaterial.SetTexture("_NoiseTex", noiseTexture);
    }
}
