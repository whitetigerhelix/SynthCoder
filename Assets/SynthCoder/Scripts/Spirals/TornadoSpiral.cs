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
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private float height = 2.0f;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private Color colorA = Color.red;
        [SerializeField] private Color colorB = Color.blue;

        private GameObject[] spheres;
        private float increment;
        private float phase;

        private void Start()
        {
            spheres = new GameObject[numSpheres];

            increment = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));
            phase = 0.0f;

            for (int i = 0; i < numSpheres; i++)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.parent = transform;
                sphere.transform.localScale = Vector3.one * 0.2f;
                sphere.GetComponent<Renderer>().material.color = Color.Lerp(colorA, colorB, (float)i / numSpheres);
                sphere.GetComponent<SphereCollider>().enabled = false;
                spheres[i] = sphere;
            }
        }

        private void Update()
        {
            phase += Time.deltaTime * speed;

            for (int i = 0; i < numSpheres; i++)
            {
                float y = ((i * height) / numSpheres) - (height / 2.0f);
                float r = Mathf.Sqrt(i / (float)numSpheres);
                float theta = i * increment + phase;

                float x = r * Mathf.Cos(theta) * radius;
                float z = r * Mathf.Sin(theta) * radius;

                spheres[i].transform.localPosition = new Vector3(x, y, z);
            }
        }
    }
}
