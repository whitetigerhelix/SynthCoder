// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    //[ExecuteInEditMode]
    public class SkyboxGenerator_ProceduralTexture : MonoBehaviour
    {
        [Header("Skybox Settings")]
        public Color topColor = Color.blue;
        public Color middleColor = Color.cyan;
        public Color bottomColor = Color.white;
        public float gradientHeight = 1.0f;

        private Material skyboxMaterial;
        Texture2D gradientTexture;

        void OnEnable()
        {
            UpdateSkybox();
        }

        private void UpdateSkybox()
        {
            Debug.Log("SkyboxGenerator_ProceduralTexture - UpdateSkybox");

            // Create the gradient texture
            DestroyImmediate(gradientTexture);
            gradientTexture = new Texture2D(1, 256);
            gradientTexture.wrapMode = TextureWrapMode.Clamp;
            gradientTexture.filterMode = FilterMode.Bilinear;
            gradientTexture.SetPixel(0, 0, topColor);
            gradientTexture.SetPixel(0, 127, middleColor);
            gradientTexture.SetPixel(0, 255, bottomColor);
            for (int i = 1; i < 127; i++)
            {
                float t = i / 127.0f;
                gradientTexture.SetPixel(0, i, Color.Lerp(topColor, middleColor, t));
            }
            for (int i = 128; i < 255; i++)
            {
                float t = (i - 128) / 127.0f;
                gradientTexture.SetPixel(0, i, Color.Lerp(middleColor, bottomColor, t));
            }
            gradientTexture.Apply();

            TextureGenerator.ExportTextureToFile(gradientTexture, "Assets/SynthCoder/Resources/ProceduralTextures/SkyboxGradientTexture.asset");

            // Create the skybox material
            DestroyImmediate(skyboxMaterial);
            skyboxMaterial = new Material(Shader.Find("Skybox/Procedural"));
            skyboxMaterial.SetTexture("_Tex", gradientTexture);
            skyboxMaterial.SetFloat("_AtmosphereThickness", gradientHeight);

            // Set the skybox material on the camera
            Camera.main.clearFlags = CameraClearFlags.Skybox;
            RenderSettings.skybox = skyboxMaterial;
        }
    }
}