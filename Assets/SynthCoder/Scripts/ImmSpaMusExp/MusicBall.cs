// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    public class MusicBall : ShaderVFX
    {
        [Header("Music Ball")]
        public int numRings = 10;
        public float amplitude = 1.0f;
        public float frequency = 1.0f;
        public float damping = 0.5f;
        public float noiseScale = 1.0f;
        public float noiseSpeed = 1.0f;

        private Vector3[] originalVertices;
        private Vector3[] displacedVertices;

        protected override void Update()
        {
            base.Update();

            UpdateMusicBall();
        }

        protected override void CreateSphere()
        {
            base.CreateSphere();

            // Create arrays to store the original and displaced vertices
            originalVertices = sphereMesh.vertices;
            displacedVertices = new Vector3[originalVertices.Length];
        }

        private void UpdateMusicBall()
        {
            if (sphereMesh == null)
            {
                return;
            }

            // Calculate the displacement for each vertex based on the amplitude and frequency
            float displacement = amplitude * Mathf.Sin(Time.time * frequency);

            // Update the displaced vertices array
            for (int i = 0; i < originalVertices.Length; i++)
            {
                Vector3 vertex = originalVertices[i];

                // Calculate the distance of the vertex from the center of the sphere
                float distanceFromCenter = Vector3.Distance(vertex, Vector3.zero);
                distanceFromCenter *= sphereRadius;

                // Calculate the amount to displace the vertex based on the distance from the center and the displacement amount
                float noiseValue = Mathf.PerlinNoise((vertex.x + Time.time * noiseSpeed) * noiseScale, (vertex.y + Time.time * noiseSpeed) * noiseScale);
                float displacementAmount = displacement * Mathf.Clamp01((sphereRadius - distanceFromCenter) / (sphereRadius * numRings)) * noiseValue;

                // Apply damping to the displacement amount to make the effect look smoother
                displacementAmount *= Mathf.Pow(damping, Time.deltaTime);

                // Displace the vertex along the normal vector of the vertex
                displacedVertices[i] = vertex + sphereMesh.normals[i] * displacementAmount;
            }

            // Update the mesh with the displaced vertices
            sphereMesh.vertices = displacedVertices;
            sphereMesh.RecalculateNormals();
            sphereMesh.RecalculateBounds();
        }
    }
}
