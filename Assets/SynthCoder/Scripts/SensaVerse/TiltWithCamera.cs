// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    public class TiltWithCamera : MonoBehaviour
    {
        public float minXRotation = -30f;
        public float maxXRotation = 30f;
        public float minZRotation = -30f;
        public float maxZRotation = 30f;
        public bool constrainToCameraYaw = false;

        private Camera mainCamera => Camera.main;

        public Vector2 Direction => new Vector2(mainCamera.transform.forward.x, mainCamera.transform.forward.z);

        private void Update()
        {
            if (mainCamera != null)
            {
                // Get camera rotation
                float cameraXRotation = mainCamera.transform.rotation.eulerAngles.x;
                float cameraYRotation = mainCamera.transform.rotation.eulerAngles.y;

                // Calculate new rotation based on camera pitch
                Quaternion newRotation = Quaternion.Euler(
                    Mathf.Clamp(cameraXRotation, minXRotation, maxXRotation),
                    constrainToCameraYaw ? cameraYRotation : transform.rotation.eulerAngles.y,
                    Mathf.Clamp(cameraXRotation, minZRotation, maxZRotation)
                );

                // Update object rotation
                transform.rotation = newRotation;
            }
        }
    }
}
