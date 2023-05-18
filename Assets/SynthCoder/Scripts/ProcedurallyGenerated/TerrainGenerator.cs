// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using System.IO;
using UnityEngine;

namespace SynthCoder
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class TerrainGenerator : MonoBehaviour
    {
        [Header("Terrain Configuration")]

        public int terrainWidth = 32;
        public int terrainDepth = 32;

        public float heightScale = 8f;
        public float heightMultiplier = 8f;
        public Vector2 heightNoiseOffset = Vector2.zero;

        [Header("Terrain Texture Generation")]

        public float terrainTextureFrequency = 0.0075f;

        [Header("Gradient Texture Generation")]

        public Color gradientStartColor = Color.red;
        public Color gradientEndColor = Color.blue;

        [Header("Shader")]

        public Shader terrainShader;

        [Range(0f, 1f)]
        public float amplitude = 0.95f;
        [Range(0f, 10f)]
        public float frequency = 8f;
        [Range(-1f, 1f)]
        public float speed = 0.58f;
        [Range(0f, 1f)]
        public float distortion = 0.35f;
        public Color color1 = new Color(0.6f, 0.007f, 0.007f, 1f);
        public Color color2 = new Color(0.04f, 0f, 1f, 1f);
        public Color color3 = new Color(1f, 0.37f, 0f, 1f);

        public string terrainTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/TerrainTexture.asset";
        public string gradientTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/GradientTexture.asset";
        public string noiseTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/NoiseTexture.asset";

        protected Mesh terrainMesh;
        protected MeshRenderer terrainRenderer;
        protected MeshFilter terrainMeshFilter;
        protected Material terrainMaterial;

		[Header("Generated Textures")]
        [SerializeField] protected Texture2D terrainTexture;
		[SerializeField] protected Texture2D gradientTexture;
		[SerializeField] protected Texture2D noiseTexture;

        protected virtual void Awake()
        {
            terrainRenderer = GetComponent<MeshRenderer>();
            terrainMeshFilter = GetComponent<MeshFilter>();
        }

        protected virtual void OnEnable()
        {
            GenerateTerrain();
        }

        protected virtual void Update()
        {
            UpdateShader();
        }

        protected virtual void UpdateShader()
        {
            if (terrainMaterial != null)
            {
                terrainMaterial.SetFloat("_Amplitude", amplitude);
                terrainMaterial.SetFloat("_Frequency", frequency);
                terrainMaterial.SetFloat("_Speed", speed);
                terrainMaterial.SetFloat("_Distortion", distortion);
                terrainMaterial.SetColor("_Color1", color1);
                terrainMaterial.SetColor("_Color2", color2);
                terrainMaterial.SetColor("_Color3", color3);
            }
        }

        protected virtual void GenerateTerrain()
        {
            // Create the terrain
            DestroyImmediate(terrainMesh);
            terrainMesh = new Mesh();
            terrainMeshFilter.mesh = terrainMesh;

            // Generate terrain information
            GenerateTerrainMesh();

            // Update terrain material and textures
            GenerateTextures();
        }

        protected virtual void GenerateTerrainMesh()
        {
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
        }

		protected virtual void GenerateTextures()
		{
#if DESTROY_EXISTING_MATERIAL
			DestroyImmediate(terrainMaterial);
			terrainMaterial = null;
#endif
			if (terrainRenderer.material == null)
			{
				terrainMaterial = new Material(terrainShader);
				terrainRenderer.material = terrainMaterial;
			}
			else
			{
				terrainMaterial = terrainRenderer.material;
			}

			// Terrain
			//DestroyImmediate(terrainTexture);
			if (terrainTexture == null)
			{
#if UNITY_EDITOR
				if (File.Exists(terrainTextureSavePath))
				{
					terrainTexture = TextureGenerator.LoadTextureFromFile(terrainTextureSavePath);
				}
				else
				{
					terrainTexture = TextureGenerator.GenerateTerrainTexture(frequency: terrainTextureFrequency);
					TextureGenerator.ExportTextureToFile(terrainTexture, terrainTextureSavePath);
				}
#else
				terrainTexture = TextureGenerator.GenerateTerrainTexture(frequency: terrainTextureFrequency);
#endif
			}

			// Gradient
			//DestroyImmediate(gradientTexture);
			if (gradientTexture == null)
			{
#if UNITY_EDITOR
				if (File.Exists(gradientTextureSavePath))
				{
					gradientTexture = TextureGenerator.LoadTextureFromFile(gradientTextureSavePath);
				}
				else
				{
					gradientTexture = TextureGenerator.GenerateGradientTexture(gradientStartColor, gradientEndColor);
					TextureGenerator.ExportTextureToFile(gradientTexture, gradientTextureSavePath);
				}
#else
				gradientTexture = TextureGenerator.GenerateGradientTexture(gradientStartColor, gradientEndColor);
#endif
			}

			// Noise
			//DestroyImmediate(noiseTexture);
			if (noiseTexture == null)
			{
#if UNITY_EDITOR
				if (File.Exists(noiseTextureSavePath))
				{
					noiseTexture = TextureGenerator.LoadTextureFromFile(noiseTextureSavePath);
				}
				else
				{
					noiseTexture = TextureGenerator.GenerateNoiseTexture();
					TextureGenerator.ExportTextureToFile(noiseTexture, noiseTextureSavePath);
				}
#else
				noiseTexture = TextureGenerator.GenerateNoiseTexture();
#endif
			}

			terrainMaterial.SetTexture("_MainTex", terrainTexture);
            terrainMaterial.SetTexture("_Gradient", gradientTexture);
            terrainMaterial.SetTexture("_NoiseTex", noiseTexture);
        }
    }
}
