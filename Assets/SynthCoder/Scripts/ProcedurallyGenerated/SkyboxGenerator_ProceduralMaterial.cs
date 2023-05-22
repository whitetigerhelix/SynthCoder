// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    [ExecuteInEditMode]
    public class SkyboxGenerator_ProceduralMaterial : MonoBehaviour
    {
        [Tooltip("Color at the top of the skybox")]
        public Color topColor = new Color(0.5f, 0.7f, 1.0f, 1.0f);

        [Tooltip("Color at the bottom of the skybox")]
        public Color bottomColor = new Color(0.0f, 0.5f, 1.0f, 1.0f);

        [Tooltip("The number of horizontal bands in the skybox")]
        public int numBands = 4;

        [Tooltip("The height of each band in the skybox")]
        public float bandHeight = 0.25f;

        [Tooltip("The width of each band in the skybox")]
        public float bandWidth = 1.0f;

        private Material skyboxMaterial;

        private void OnEnable()
        {
            DestroyImmediate(skyboxMaterial);
            skyboxMaterial = new Material(Shader.Find("Skybox/Procedural"));

            UpdateSkybox();
        }

        /*private void Update()
        {
            // Only update the skybox in the editor
    #if UNITY_EDITOR
            if (isActiveAndEnabled)
            {
                UpdateSkybox();
            }
    #endif
        }*/

        private void UpdateSkybox()
        {
            Debug.Log("SkyboxGenerator_Material - UpdateSkybox");

            float bandSpacing = 1.0f / numBands;
            skyboxMaterial.SetColor("_TopColor", topColor);
            skyboxMaterial.SetColor("_BottomColor", bottomColor);
            skyboxMaterial.SetInt("_NumBands", numBands);
            skyboxMaterial.SetFloat("_BandHeight", bandHeight);
            skyboxMaterial.SetFloat("_BandSpacing", bandSpacing);
            skyboxMaterial.SetFloat("_BandWidth", bandWidth);
            skyboxMaterial.SetFloat("_BandOffset", Time.time % bandSpacing);

            RenderSettings.skybox = skyboxMaterial;
        }
    }
}
