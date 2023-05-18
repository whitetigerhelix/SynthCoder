// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    public class SkyboxGenerator_SetMaterial : MonoBehaviour
    {
        public Material skyboxMaterial;

        private void OnEnable()
        {
            RenderSettings.skybox = skyboxMaterial;
        }
    }
}
