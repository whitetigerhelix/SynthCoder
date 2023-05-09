// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

public class FlyOverTerrainGenerator : TerrainGenerator
{
    public Vector2 ScrollDirection = new Vector2(1f, 0f);

    protected void Update()
    {
        if (terrainMaterial != null)
        {
            terrainMaterial.SetVector("_ScrollDirection", ScrollDirection * Time.time);
        }
    }
}
