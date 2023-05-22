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
        public int numSegments = 20;
        public float amplitude = 1.0f;
        public float frequency = 1.0f;
        public float damping = 0.5f;
        public float peakRadius = 0.5f;
        [Range(0f, 1f)]
        public float smoothness = 0.5f;
        public float scrollSpeed = 0.1f; // the speed at which the noise texture scrolls
        public Texture2D noiseTexture;

        private Vector3[] originalVertices;
        private Vector3[] displacedVertices;

        protected override void CreateSphere()
        {
            base.CreateSphere();

            // Create arrays to store the original and displaced vertices
            originalVertices = sphereMesh.vertices;
            displacedVertices = new Vector3[originalVertices.Length];
        }

        protected override void Update()
        {
            base.Update();

            UpdateMusicBall();
        }

        private void UpdateMusicBall()
        {
            if (sphereMesh == null || noiseTexture == null || numSegments <= 0 || numRings <= 0)
            {
                return;
            }

            // Calculate the displacement for each vertex based on the amplitude, frequency and smoothness
            float displacement = amplitude * Mathf.Sin(Time.time * frequency) * (1.0f - smoothness);

            // Calculate the UV scale based on the number of rings and segments
            Vector2 uvScale = new Vector2(1.0f / numSegments, 1.0f / numRings);

            // Calculate the heightmap using the noise texture
            Color[] pixels = noiseTexture.GetPixels();
            float[] heightmap = new float[pixels.Length];
            for (int i = 0; i < pixels.Length; i++)
            {
                heightmap[i] = pixels[i].r;
            }

            // Calculate the time-based offset for scrolling the noise texture
            Vector2 scrollOffset = new Vector2(Time.time * scrollSpeed, Time.time * scrollSpeed);

            // Update the displaced vertices array
            for (int i = 0; i < originalVertices.Length; i++)
            {
                // Calculate the UV coordinates of the vertex
                Vector3 vertex = originalVertices[i];
                Vector2 uv = new Vector2(
                    Mathf.Atan2(vertex.z, vertex.x) / (2.0f * Mathf.PI) + 0.5f,
                    Mathf.Acos(vertex.y / sphereRadius) / Mathf.PI
                );

                // Apply the time-based offset to the UV coordinates to scroll the noise texture
                uv += scrollOffset;

                // Wrap the UV coordinates to prevent them from going out of bounds
                uv.x = uv.x % 1.0f;
                uv.y = uv.y % 1.0f;

                // Calculate the index of the heightmap based on the UV coordinates
                int x = Mathf.FloorToInt(uv.x / uvScale.x);
                int y = Mathf.FloorToInt(uv.y / uvScale.y);
                int index = y * numSegments + x;

                // Get the heightmap value and apply the peak radius
                float height = heightmap[index] * peakRadius;

                // Calculate the displacement amount based on the amplitude, heightmap value, and smoothness
                float displacementAmount = (displacement * Mathf.Clamp01((sphereRadius - height) / (sphereRadius * numRings))) + vertex.magnitude;

                // Apply damping to the displacement amount to make the effect look smoother
                if (damping > 0f)
                {
                    displacementAmount *= Mathf.Pow(damping, Time.deltaTime);
                }

                // Displace the vertex along the normal vector of the vertex
                displacedVertices[i] = vertex.normalized * displacementAmount;
            }

            // Update the mesh with the displaced vertices
            sphereMesh.vertices = displacedVertices;
            sphereMesh.RecalculateNormals();
            sphereMesh.RecalculateBounds();
        }
    }
}
