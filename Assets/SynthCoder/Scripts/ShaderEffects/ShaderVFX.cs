// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

/// <summary>
/// This script defines a ShaderVFX class that creates a sphere object and generates a texture for it. 
/// The generated texture is based on Perlin Noise and is applied to the sphere using a specified material. 
/// The script allows for customization of the sphere's radius and the size of the generated texture.
/// </summary>
public class ShaderVFX : MonoBehaviour
{
    public float sphereRadius = 1.0f;
    public Material sphereMaterial;

    private GameObject sphere;
    
    Texture2D dynamicTexture;
    Color32[] colors;

    private string dynamicTextureSavePath = "Assets/SynthCoder/Resources/ProceduralTextures/ShaderVFXTexture.asset";

    private void OnEnable()
    {
        CreateSphere();
    }

    private void OnDisable()
    {
        Destroy(sphere);
        sphere = null;
    }

    private void CreateSphere()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent(transform);
        sphere.transform.localPosition = Vector3.zero;
        sphere.transform.localRotation = Quaternion.identity;
        sphere.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);
        sphere.GetComponent<Renderer>().material = sphereMaterial;

        GenerateTexture();
    }

    // This function generates a new Texture2D with the given dimensions, generates colors for each pixel using Perlin noise,
    // sets the pixels of the texture, assigns the texture to a shader, and exports the texture to a file. If a previous texture exists,
    // it is destroyed before creating the new one.
    public void GenerateTexture(int width = 256, int height = 256)
    {
        if (dynamicTexture == null)
        {
            Destroy(dynamicTexture);
        }

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

        // Assign the dynamic texture to the shader
        sphere.GetComponent<Renderer>().material.SetTexture("_MainTex", dynamicTexture);

        TextureGenerator.ExportTextureToFile(dynamicTexture, dynamicTextureSavePath);
    }
}
