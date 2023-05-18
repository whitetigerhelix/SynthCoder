// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEditor;
using UnityEngine;

namespace SynthCoder
{
    public static class TextureGenerator
    {
        // This function generates a Perlin noise texture to simulate terrain.
        // It creates a Texture2D of a specified width and height and uses Perlin noise to generate values between 0 and 1.
        // It then sets the pixels of the texture to a gradient of green colors based on the noise values,
        // with darker green representing lower values and brighter green representing higher values.
        // The frequency parameter controls the level of detail in the noise. The resulting texture is returned.
        public static Texture2D GenerateTerrainTexture(int width = 512, int height = 512, float frequency = 0.0075f)
        {
            Texture2D terrainTexture = new(width, height);
            Color32[] colors = new Color32[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float noise = Mathf.PerlinNoise(x * frequency, y * frequency);
                    colors[y * width + x] = new Color(0f, noise, 0f, 1f);
                }
            }
            terrainTexture.SetPixels32(colors);
            terrainTexture.Apply();
            return terrainTexture;
        }

        // This function generates a texture with a gradient between two colors. The width and height of the texture can be specified.
        public static Texture2D GenerateGradientTexture(Color32 gradientStartColor, Color32 gradientEndColor, int width = 512, int height = 512)
        {
            Texture2D gradientTexture = new(width, height);
            Color32[] pixels = new Color32[width * height];

            // Loop through the pixels and set their colors based on their position
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float t = (float)y / height;
                    pixels[x + y * width] = Color32.Lerp(gradientStartColor, gradientEndColor, t);
                }
            }
            gradientTexture.SetPixels32(pixels);
            gradientTexture.Apply();
            return gradientTexture;
        }

        // This function generates a Texture2D with Perlin noise colors for each pixel in the texture.
        // The function takes in the width and height of the texture as optional parameters,
        // and then creates a new Texture2D with the specified dimensions. It then generates colors for each pixel
        // in the texture using Perlin noise with different frequencies for each color channel.
        // Finally, it sets the texture pixels with the generated colors and returns the Texture2D.
        public static Texture2D GenerateNoiseTexture(int width = 512, int height = 512)
        {
            // Create a new Texture2D with the desired dimensions
            Texture2D noiseTexture = new(width, height, TextureFormat.RGBA32, false);

            // Generate colors for each pixel in the texture
            var colors = new Color32[width * height];
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
            noiseTexture.SetPixels32(colors);
            noiseTexture.Apply();

            return noiseTexture;
        }

        // Can get the procedural texture from the material like this:
        // Texture2D proceduralTexture = GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex") as Texture2D;
        public static void ExportTextureToFile(Texture2D proceduralTexture, string savePath = "Assets/SynthCoder/Resources/ProceduralTextures/SavedTexture.asset")
        {
#if UNITY_EDITOR
            // Create a new RenderTexture with the same dimensions as the procedural texture
            RenderTexture renderTexture = new(proceduralTexture.width, proceduralTexture.height, 0);

            // Set the active RenderTexture to the new RenderTexture
            RenderTexture.active = renderTexture;

            // Render the procedural texture onto the new RenderTexture
            Graphics.Blit(proceduralTexture, renderTexture);

            // Create a new Texture2D to read the pixels of the RenderTexture
            Texture2D savedTexture = new(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

            // Read the pixels of the RenderTexture into the new Texture2D
            savedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            // Apply the changes to the new Texture2D
            savedTexture.Apply();

            // Save the new Texture2D to disk using AssetDatabase.CreateAsset()
            AssetDatabase.CreateAsset(savedTexture, savePath);
#else
			Debug.LogWarning("ExportTextureToFile is only supported in the Unity Editor.");
#endif
		}

		public static Texture2D LoadTextureFromFile(string filePath)
		{
#if UNITY_EDITOR
			// Load the Texture2D from the specified file path using AssetDatabase.LoadAssetAtPath()
			Texture2D loadedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
			if (loadedTexture == null)
			{
				Debug.LogError("LoadTextureFromFile - Failed to load texture from file: " + filePath);
			}
			return loadedTexture;
#else
			Debug.LogWarning("LoadTextureFromFile is only supported in the Unity Editor.");
			return null;
#endif
		}
	}
}
