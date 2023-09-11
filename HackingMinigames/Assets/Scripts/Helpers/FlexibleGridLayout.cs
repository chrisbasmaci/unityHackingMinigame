using System;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public sealed class GridLayoutMaximiser : MonoBehaviour {
        GridLayoutGroup gridLayoutGroup;
        RectTransform rect;
        public float cellRatio = 1; // Add a public variable for the cell ratio.
        
        private int columnCount = 1;
        private int rowCount = 1;
        private float maxCellHeight;
        private float maxCellWidth;
        void Start ()
        {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
            rect = GetComponent<RectTransform> ();
            if (!gridLayoutGroup)
            {
                gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            }
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = 1;
            CalculateCellSize();
        }

        void OnRectTransformDimensionsChange ()
        {
            if (gridLayoutGroup != null && rect != null)
            {
                CalculateCellSize();
            }
        }

        // Create a method to calculate and set the cell size based on the ratio.
        private void CalculateCellSize()
        {



            CalculateMaxFit();
            Debug.Log("width: " +maxCellWidth + "height: " + maxCellHeight);

            
            float currentCellWidth = maxCellWidth;
            float currentCellHeight = maxCellHeight;
            

 
            
            Debug.Log("currentCellWidth: " +currentCellWidth + "currentCellHeight: " + currentCellHeight);
            Debug.Log("col: "+ columnCount + " row:" + rowCount);
            Debug.Log("width"+ currentCellWidth + "height:" + currentCellHeight);

            gridLayoutGroup.cellSize = new Vector2(currentCellWidth, currentCellHeight);
        }
        public void CalculateBiggestDimensions()
        {
            // Define the dimensions of the target rectangle.
            float targetWidth = rect.rect.width /columnCount;
            float targetHeight = rect.rect.height / rowCount;
            // Check if the target rectangle is taller than it is wide.
            if (targetHeight > targetWidth)
            {
                // Calculate the height and width of the largest rectangle.
                maxCellHeight = Math.Min(targetHeight, targetWidth / cellRatio);
                maxCellWidth = maxCellHeight * cellRatio;
        
                // Print the dimensions of the largest rectangle that fits.
                Console.WriteLine("Largest Rectangle Width: " + maxCellWidth);
                Console.WriteLine("Largest Rectangle Height: " + maxCellHeight);
            }
            else
            {
                // Calculate the width and height of the largest rectangle.
                maxCellWidth = Math.Min(targetWidth, targetHeight * cellRatio);
                maxCellHeight = maxCellWidth / cellRatio;
        
                // Print the dimensions of the largest rectangle that fits.
                Console.WriteLine("Largest Rectangle Width: " + maxCellWidth);
                Console.WriteLine("Largest Rectangle Height: " + maxCellHeight);
            }
        }
        
        private void CalculateMaxFit()
        {
            int n = transform.childCount;
            float rectratio = 600/895f;
            // Compute number of rows and columns, and cell size
            float ratio = rect.rect.width / rect.rect.height;
            ratio /= rectratio;
            float ncolsFloat = (float)Math.Ceiling(Math.Sqrt(n * ratio));
            float nrowsFloat = n / ncolsFloat;


            // Find best option filling the whole height
            double nrows1 = Math.Ceiling(nrowsFloat);
            double ncols1 = Math.Ceiling(n / nrows1);
            while (nrows1 * ratio < ncols1)
            {
                nrows1++;
                ncols1 = Math.Ceiling(n / nrows1);
            }
            double cellSize1 = rect.rect.height / nrows1;

            // Find best option filling the whole width
            double ncols2 = Math.Ceiling(ncolsFloat);
            double nrows2 = Math.Ceiling(n / ncols2);
            while (ncols2 < nrows2 * ratio)
            {
                ncols2++;
                nrows2 = Math.Ceiling(n / ncols2);
            }
            double cellSize2 = rect.rect.width / ncols2;

            // Find the best values
            double cellSize;
            if (cellSize1 < cellSize2)
            {
                rowCount = (int)nrows2;
                columnCount = (int)ncols2;
                cellSize = cellSize2;
                
                maxCellWidth = (float)(cellSize);
                maxCellHeight =(float)(cellSize/ rectratio);
            }
            else
            {
                rowCount = (int)nrows1;
                columnCount = (int)ncols1;
                cellSize = cellSize1;
                maxCellWidth = (float)(rectratio * cellSize);
                maxCellHeight =(float) cellSize;
            }
            Debug.Log("Max Fit! cellsize1: " +
                      cellSize1 +"cellsize2: " +cellSize2 +"ratio: "+ ratio +"rectratio: " + rectratio + "n: " + n);
            
            gridLayoutGroup.constraintCount = rowCount;

        }
        
    }                

}