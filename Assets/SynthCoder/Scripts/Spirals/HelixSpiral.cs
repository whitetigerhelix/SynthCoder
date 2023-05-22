// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using System.Collections.Generic;
using UnityEngine;

namespace SynthCoder
{
    /// <summary>
    /// Generates a helix spiral of spheres.
    /// </summary>
    public class HelixSpiral : MonoBehaviour
    {
        [Header("Helixes")]

        [Range(1, 8)]
        [Tooltip("The number of helix spirals.")]
        public int numHelixes = 3;

        [Tooltip("The radius of each helix spiral.")]
        public float helixRadius = 1f;

        [Tooltip("The pitch (vertical distance) between each turn of the helix spiral.")]
        public float helixPitch = 3f;

        [Tooltip("The vertical spacing between each helix spiral.")]
        public float helixSpacing = 0.1f;

        [Tooltip("The angle offset for the start of each helix spiral: 0 - 2PI.")]
        public float helixAngleOffset = 4.5f;

        [Tooltip("The ratio of the helix radius for each turn.")]
        public float helixRadiusRatio = 1f;

        [Tooltip("The height of the helix, spheres will stay within this bounds.")]
        public float helixHeight = 5.75f;

        [Header("Spheres")]

        [Range(1, 100)]
        [Tooltip("The number of spheres in each helix spiral.")]
        public int numSpheres = 23;

        [Tooltip("The radius of each sphere.")]
        public float sphereRadius = 0.5f;

        [Tooltip("The starting color of the spheres.")]
        public Color startColor = Color.red;

        [Tooltip("The ending color of the spheres.")]
        public Color endColor = Color.blue;

        [Tooltip("The time it takes for the spheres to transition from the start color to the end color.")]
        public float colorTransitionTime = 1f;

        [Tooltip("If true, ping pongs the color, else lerps based on sphere height in helix")]
        public bool pingPongColor = false;

        [Tooltip("The speed at which the spheres rotate around the vertical axis.")]
        public float rotationSpeed = 10f;

		[Tooltip("Prefab for a sphere - if unset, will generate a primitive")]
		public GameObject spherePrefab;

        private List<List<GameObject>> spheres;
        private float totalHeight;

        private void OnEnable()
        {
            GenerateSpheres();
        }

        private void OnDisable()
        {
            DestroySpheres();
        }

        private void Update()
        {
            UpdateSpheres();
        }

        private Vector3 GetSpherePosition(int helixIndex, int sphereIndex)
        {
            float ratio = (float)sphereIndex / numSpheres;
            float angle = ratio * Mathf.PI * 2.0f + helixIndex * helixAngleOffset;
            float x = Mathf.Sin(angle) * helixRadius * Mathf.Pow(helixRadiusRatio, sphereIndex);
            float y = helixIndex * helixSpacing + sphereIndex * helixPitch;
            float z = Mathf.Cos(angle) * helixRadius * Mathf.Pow(helixRadiusRatio, sphereIndex);
            Vector3 pos = new Vector3(x, y, z);

            // Add rotation around the vertical axis
            float swirlAngle = Time.time * rotationSpeed * Mathf.PI * 2.0f;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, swirlAngle, 0f), Vector3.one);
            pos = rotationMatrix.MultiplyPoint3x4(pos);

            return pos;
        }

        private void DestroySpheres()
        {
            if (spheres != null)
            {
                for (int helixIndex = 0; helixIndex < spheres.Count; helixIndex++)
                {
                    for (int sphereIndex = 0; sphereIndex < spheres[helixIndex].Count; sphereIndex++)
                    {
                        Destroy(spheres[helixIndex][sphereIndex]);
                    }
                    spheres[helixIndex].Clear();
                }
                spheres.Clear();
            }
        }

        private void GenerateSpheres()
        {
            // Delete all existing spheres
            DestroySpheres();

            // Initialize the 2D array of spheres
            spheres = new List<List<GameObject>>();

            // Spawn the spheres
            for (int helixIndex = 0; helixIndex < numHelixes; helixIndex++)
            {
                spheres.Add(new List<GameObject>());

                for (int sphereIndex = 0; sphereIndex < numSpheres; sphereIndex++)
                {
                    Vector3 pos = GetSpherePosition(helixIndex, sphereIndex);
                    GameObject sphere = spherePrefab != null ?
						Instantiate(spherePrefab) :
						GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.parent = transform;
                    sphere.transform.localPosition = pos;
                    sphere.transform.localScale = Vector3.one * sphereRadius;
                    sphere.GetComponent<SphereCollider>().enabled = false;
                    spheres[helixIndex].Add(sphere);

                    totalHeight = Mathf.Max(totalHeight, pos.y);
                }

				Debug.Log($"HelixSpiral.GenerateSpheres - Helix {helixIndex} - Created {spheres[helixIndex].Count} spheres");
			}
		}

        private void UpdateSpheres()
        {
            if (spheres == null ||
                spheres.Count != numHelixes ||
                spheres[0].Count != numSpheres ||
                spheres[0][0].transform.localScale.x != sphereRadius)
            {
                GenerateSpheres();
            }

            // Update the sphere positions and colors
            for (int helixIndex = 0; helixIndex < numHelixes; helixIndex++)
            {
                for (int sphereIndex = 0; sphereIndex < numSpheres; sphereIndex++)
                {
                    Vector3 pos = GetSpherePosition(helixIndex, sphereIndex);
                    //pos.y += Time.time * rotationSpeed;   //TODO: They shouldn't just rotate, but also not move up, they should follow the spiral's path
                    pos.y = Mathf.Repeat(pos.y, helixHeight);

                    GameObject sphere = spheres[helixIndex][sphereIndex];
                    sphere.transform.localPosition = pos;

                    // Update the sphere color
                    float t = pingPongColor
                        ? Mathf.PingPong(Time.time / colorTransitionTime, 1f)
                        : Mathf.InverseLerp(0f, helixHeight, sphere.transform.localPosition.y);
                    sphere.GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
                }
            }
        }
    }
}
