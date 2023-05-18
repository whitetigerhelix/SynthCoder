// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    public sealed class CameraController : MonoBehaviour
    {
        public float speed = 5.0f;
        public float sensitivity = 2.0f;

        private bool isLooking = false;
        public bool IsLooking => isLooking;

        float xRotation = 0f;
        float yRotation = 0f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            // Move the camera based on WSAD + QE keys
            float x = Input.GetAxis("Horizontal");
            float y = 0f;
            if (Input.GetKey(KeyCode.E))
            {
                y = 1f;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                y = -1f;
            }
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.up * y + transform.forward * z;

            transform.position += speed * Time.deltaTime * move;

            // Look around using the mouse when the right mouse button is pressed
            if (Input.GetMouseButton(1))
            {
                isLooking = true;
                float mouseX = Input.GetAxis("Mouse X") * sensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                yRotation += mouseX;

                transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            }
            else
            {
                isLooking = false;
            }
        }
    }
}
