// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEditor;
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

    private Mesh terrainMesh;
    private Material terrainMaterial;
    private Texture2D terrainTexture;
    private Texture2D gradientTexture;
    private Texture2D noiseTexture;
    private string terrainTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/TerrainTexture.asset";
    private string gradientTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/GradientTexture.asset";
    private string noiseTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/NoiseTexture.asset";

    private void OnEnable()
    {
        GenerateTerrain();
    }

    Texture2D GenerateTerrainTexture(int width = 512, int height = 512)
    {
        DestroyImmediate(terrainTexture);
        terrainTexture = new Texture2D(width, height);

        Color[] colors = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float noise = Mathf.PerlinNoise(x * terrainTextureFrequency, y * terrainTextureFrequency);
                colors[y * width + x] = new Color(0f, noise, 0f, 1f);
            }
        }

        terrainTexture.SetPixels(colors);
        terrainTexture.Apply();

        ExportTextureToFile(terrainTexture, terrainTextureSavePath);

        return terrainTexture;
    }

    private Texture2D GenerateGradientTexture(int width = 512, int height = 512)
    {
        DestroyImmediate(gradientTexture);
        gradientTexture = new Texture2D(width, height);

        Color[] pixels = new Color[width * height];

        // Loop through the pixels and set their colors based on their position
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float t = (float)y / height;
                pixels[x + y * width] = Color.Lerp(gradientStartColor, gradientEndColor, t);
            }
        }

        gradientTexture.SetPixels(pixels);
        gradientTexture.Apply();

        ExportTextureToFile(gradientTexture, gradientTextureSavePath);

        return gradientTexture;
    }

    private Texture2D GenerateNoiseTexture(int width = 512, int height = 512)
    {
        if (noiseTexture == null)
        {
            Destroy(noiseTexture);
        }

        // Create a new Texture2D with the desired dimensions
        noiseTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // Generate colors for each pixel in the texture
        var colors = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float r = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                float g = Mathf.PerlinNoise(x * 0.3f, y * 0.3f);
                float b = Mathf.PerlinNoise(x * 0.5f, y * 0.5f);
                colors[x + y * width] = new Color(r, g, b, 1.0f);
            }
        }

        // Set the texture pixels
        noiseTexture.SetPixels(colors);
        noiseTexture.Apply();

        ExportTextureToFile(noiseTexture, noiseTextureSavePath);

        return noiseTexture;
    }

    private void GenerateTerrain()
    {
        // Create the terrain
        DestroyImmediate(terrainMesh);
        terrainMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = terrainMesh;

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
        GetComponent<MeshRenderer>().material = terrainMaterial;

        GenerateTerrainTexture();
        GenerateGradientTexture();
        GenerateNoiseTexture();
        terrainMaterial.shader = terrainShader;
        terrainMaterial.SetTexture("_MainTex", terrainTexture);
        terrainMaterial.SetTexture("_Gradient", gradientTexture);
        terrainMaterial.SetTexture("_NoiseTex", noiseTexture);
    }

    // Can get the procedural texture from the material like this:
    // Texture2D proceduralTexture = GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex") as Texture2D;
    private static void ExportTextureToFile(Texture2D proceduralTexture, string savePath = "Assets/SynthCoder/Resources/ProceduralTextures/SavedTexture.asset")
    {
#if UNITY_EDITOR
        // Create a new RenderTexture with the same dimensions as the procedural texture
        RenderTexture renderTexture = new RenderTexture(proceduralTexture.width, proceduralTexture.height, 0);

        // Set the active RenderTexture to the new RenderTexture
        RenderTexture.active = renderTexture;

        // Render the procedural texture onto the new RenderTexture
        Graphics.Blit(proceduralTexture, renderTexture);

        // Create a new Texture2D to read the pixels of the RenderTexture
        Texture2D savedTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Read the pixels of the RenderTexture into the new Texture2D
        savedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        // Apply the changes to the new Texture2D
        savedTexture.Apply();

        // Save the new Texture2D to disk using AssetDatabase.CreateAsset()
        AssetDatabase.CreateAsset(savedTexture, savePath);
#endif
    }
}
