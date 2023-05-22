// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    public class TornadoSpiral : MonoBehaviour
    {
        [SerializeField] private int numSpheres = 50;
		public int NumSpheres { get => numSpheres; set => numSpheres = value; }
        [SerializeField] private float radius = 1.0f;
		public float Radius { get => radius; set => radius = value; }
		[SerializeField] private float height = 2.0f;
		public float Height { get => height; set => height = value; }
		[SerializeField] private float speed = 1.0f;
		public float Speed { get => speed; set => speed = value; }
		[SerializeField] private Color colorA = Color.red;
        [SerializeField] private Color colorB = Color.blue;

		[SerializeField, Tooltip("Prefab for a sphere - if unset, will generate a primitive")]
		private GameObject spherePrefab;

		private GameObject[] spheres;
        private float increment;
        private float phase;

        private void Start()
        {
            spheres = new GameObject[NumSpheres];

            increment = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));
            phase = 0.0f;

            for (int i = 0; i < NumSpheres; i++)
			{
				GameObject sphere = spherePrefab != null ?
					Instantiate(spherePrefab) :
					GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphere.transform.parent = transform;
                sphere.transform.localScale = Vector3.one * 0.2f;
                sphere.GetComponent<Renderer>().material.color = Color.Lerp(colorA, colorB, (float)i / NumSpheres);
                sphere.GetComponent<SphereCollider>().enabled = false;
                spheres[i] = sphere;
            }

			Debug.Log($"TornadoSpiral - Created {spheres.Length} spheres");
        }

        private void Update()
        {
            phase += Time.deltaTime * Speed;

            for (int i = 0; i < NumSpheres; i++)
            {
                float y = ((i * Height) / NumSpheres) - (Height / 2.0f);
                float r = Mathf.Sqrt(i / (float)NumSpheres);
                float theta = i * increment + phase;

                float x = r * Mathf.Cos(theta) * Radius;
                float z = r * Mathf.Sin(theta) * Radius;

                spheres[i].transform.localPosition = new Vector3(x, y, z);
            }
        }
    }
}
