// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    /// <summary>
    /// SpiralMovement class is responsible for creating and animating a spiral of spheres in a helix - like shape.
    /// It provides properties to control the radius, height, speed, number, and color of the spheres.
    /// </summary>
    public class SpiralMovement : MonoBehaviour
    {
        [Tooltip("The radius of the helix.")]
        public float radius = 2f;
        [Tooltip("The radius of the spheres.")]
        public float sphereRadius = 0.5f;
        [Tooltip("The height of each loop of the helix.")]
        public float height = 3f;
        [Tooltip("The speed at which the spheres move.")]
        public float speed = 100f;
        [Tooltip("The number of spheres in the helix.")]
        public int numSpheres = 20;
        [Tooltip("The color of the spheres.")]
        public Gradient colorGradient;
        [Tooltip("The speed at which the color changes.")]
        public float colorSpeed = 1f;

        private float angle = 0f;
        private float angleStep;
        private float yPos;
        private float lastSphereRadius;
        private int lastNumSpheres;

        private void Start()
        {
            angleStep = 360f / numSpheres;
            yPos = transform.position.y;

            // Create the spheres
            CreateSpheres();
        }

        private void CreateSpheres()
        {
            // Destroy existing spheres
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            // Create new spheres
            for (int i = 0; i < numSpheres; i++)
            {
                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                float y = yPos + Mathf.Sin(angle * Mathf.Deg2Rad) * height;
                float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
                Vector3 pos = new Vector3(x, y, z);

                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.SetParent(transform);
                sphere.transform.localPosition = pos;
                sphere.transform.localScale = Vector3.one * sphereRadius;
                sphere.GetComponent<SphereCollider>().enabled = false;

                angle += angleStep;
            }

            lastNumSpheres = numSpheres;
            lastSphereRadius = sphereRadius;
        }

        private void Update()
        {
            // If the radius or number of spheres has changed, recreate the spheres
            if (lastSphereRadius != sphereRadius || lastNumSpheres != numSpheres)
            {
                CreateSpheres();
            }

            // Animate the height value
            height = Mathf.PingPong(Time.time * 2, 6f);

            angle += speed * Time.deltaTime;
            if (angle >= 360f)
            {
                angle = 0f;
            }

            for (int i = 0; i < numSpheres; i++)
            {
                float x = Mathf.Cos(angle * Mathf.Deg2Rad + i * angleStep * Mathf.Deg2Rad) * radius;
                float y = yPos + Mathf.Sin(angle * Mathf.Deg2Rad + i * angleStep * Mathf.Deg2Rad) * height;
                float z = Mathf.Sin(angle * Mathf.Deg2Rad + i * angleStep * Mathf.Deg2Rad) * radius;
                Vector3 pos = new Vector3(x, y, z);

                Transform sphere = transform.GetChild(i);
                sphere.localPosition = pos;

                // Update sphere color based on position and movement
                float distance = Vector3.Distance(sphere.position, transform.position);
                float normalizedDistance = distance / (radius * numSpheres * 0.5f);
                //float t = Mathf.Repeat(Time.time * colorSpeed, 1f);
                float t = Mathf.PingPong(Time.time * colorSpeed, 1f);
                Color color = colorGradient.Evaluate(normalizedDistance + t);
                sphere.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}
