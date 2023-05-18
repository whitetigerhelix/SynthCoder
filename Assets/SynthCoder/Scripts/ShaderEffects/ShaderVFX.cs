// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace SynthCoder
{
    /// <summary>
    /// This script defines a ShaderVFX class that creates a sphere object and generates a texture for it. 
    /// The generated texture is based on Perlin Noise and is applied to the sphere using a specified material. 
    /// The script allows for customization of the sphere's radius and the size of the generated texture.
    /// </summary>
    public class ShaderVFX : MonoBehaviour
    {
        public float sphereRadius = 1.0f;
        public int sphereDivisions = 32;

        public Material sphereMaterial;
        public bool castShadows = false;    // Set during CreateSphere

        protected GameObject sphere;
        protected MeshFilter meshFilter;
        protected Mesh sphereMesh => meshFilter.mesh;
        protected MeshRenderer sphereRenderer;
        public Material RendererMaterial => sphereRenderer.material;

		[Header("Texture")]
        [SerializeField] protected Texture2D dynamicTexture;
        protected Color32[] colors;

        [Header("Shader Configuration")]

        [Range(0.1f, 10f)]
        public float Speed = 1f;
        [Range(1f, 10f)]
        public float Scale = 1f;
        [Range(0.1f, 10f)]
        public float Distortion = 1f;
        [Range(0.1f, 10f)]
        public float DistortionSpeed = 1f;
        public Color Color1 = new Color(1f, 0.5f, 0f, 1f);
        public Color Color2 = new Color(1f, 0f, 0.5f, 1f);
        public Color Color3 = new Color(1f, 1f, 0f, 1f);

        public string dynamicTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/ShaderVFXTexture.asset";

        protected virtual void OnEnable()
        {
            CreateSphere();
        }

        protected virtual void OnDisable()
        {
            Destroy(sphere);
            sphere = null;
        }

        protected virtual void Update()
        {
            UpdateShader();
        }

        protected virtual void CreateSphere()
        {
            //sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere = new GameObject("SphereMesh");
            sphere.transform.SetParent(transform);
            sphere.transform.localPosition = Vector3.zero;
            sphere.transform.localRotation = Quaternion.identity;

            // Get or create MeshFilter/Renderer
            if (!sphere.TryGetComponent<MeshFilter>(out meshFilter))
            {
                meshFilter = sphere.AddComponent<MeshFilter>();
            }
            if (!sphere.TryGetComponent<MeshRenderer>(out sphereRenderer))
            {
                sphereRenderer = sphere.AddComponent<MeshRenderer>();
            }

            // Generate a new sphere mesh with the given radius and number of divisions
            meshFilter.mesh = SacredGeometryGenerator.GenerateSphereMesh(sphereRadius, sphereDivisions);

            // Create a new instance of the material and assign it to the sphere
            if (sphereMaterial != null)
            {
                sphereRenderer.material = new Material(sphereMaterial);
            }
            sphereRenderer.shadowCastingMode = castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;

            GenerateTexture();
        }

		// This function generates a new Texture2D with the given dimensions, generates colors for each pixel using Perlin noise,
		// sets the pixels of the texture, assigns the texture to a shader, and exports the texture to a file. If a previous texture exists,
		// it is destroyed before creating the new one.
		public virtual void GenerateTexture(int width = 256, int height = 256)
		{
			//DestroyImmediate(dynamicTexture);
			if (dynamicTexture == null)
			{
#if UNITY_EDITOR
				// Check if a saved texture exists
				if (File.Exists(dynamicTextureSavePath))
				{
					// Load the existing texture
					dynamicTexture = TextureGenerator.LoadTextureFromFile(dynamicTextureSavePath);
				}
				else
#endif
				{
					// Create a new Texture2D with the desired dimensions
					dynamicTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

					// Generate colors for each pixel in the texture
					colors = new Color32[width * height];
					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							float r = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
							float g = Mathf.PerlinNoise(x * 0.3f, y * 0.3f);
							float b = Mathf.PerlinNoise(x * 0.5f, y * 0.5f);
							colors[x + y * width] = new Color32((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), 255);
						}
					}

					// Set the texture pixels
					dynamicTexture.SetPixels32(colors);
					dynamicTexture.Apply();

#if UNITY_EDITOR
					TextureGenerator.ExportTextureToFile(dynamicTexture, dynamicTextureSavePath);
#endif
				}
			}

			// Assign the dynamic texture to the shader
			RendererMaterial.SetTexture("_MainTex", dynamicTexture);
		}

		protected virtual void UpdateShader()
        {
            if (RendererMaterial != null)
            {
                RendererMaterial.SetFloat("_Speed", Speed);
                RendererMaterial.SetFloat("_Scale", Scale);
                RendererMaterial.SetFloat("_Distortion", Distortion);
                RendererMaterial.SetFloat("_DistortionSpeed", DistortionSpeed);
                RendererMaterial.SetColor("_Color1", Color1);
                RendererMaterial.SetColor("_Color2", Color2);
                RendererMaterial.SetColor("_Color3", Color3);
            }
        }
    }
}
