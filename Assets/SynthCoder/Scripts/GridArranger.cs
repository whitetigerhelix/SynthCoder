// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

using UnityEngine;

namespace SynthCoder
{
    /// <summary>
    /// This script arranges the child objects into a grid based on the number of columns and the spacing between objects specified in the inspector. 
    /// It calculates the number of rows required to fit all the children in the grid, then calculates the starting position for the grid centered on the parent object. 
    /// Finally, it arranges the child objects in the grid by setting their local position. You can adjust the number of columns and distance between objects in the inspector to achieve the desired grid arrangement.
    /// </summary>
    public class GridArranger : MonoBehaviour
    {
        [SerializeField] private int columns = 3;
        [SerializeField] private float distance = 2f;

        private void OnEnable()
        {
            Arrange();
        }

        private void Arrange()
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            int numChildren = childTransforms.Length - 1; // exclude parent

            float rowSpacing = distance;
            float colSpacing = distance;

            // Calculate the number of rows required to fit all children in the grid
            int rows = numChildren / columns;
            if (numChildren % columns != 0) rows++;

            // Calculate the starting position for the grid (centered on the parent object)
            float startX = -colSpacing * (columns - 1) / 2f;
            float startZ = -rowSpacing * (rows - 1) / 2f;

            // Arrange the child objects in a grid
            for (int i = 0; i < numChildren; i++)
            {
                int col = i % columns;
                int row = i / columns;

                Vector3 newPos = new Vector3(startX + col * colSpacing, 0f, startZ + row * rowSpacing);
                childTransforms[i + 1].localPosition = newPos; // +1 to skip parent
            }
        }
    }
}
