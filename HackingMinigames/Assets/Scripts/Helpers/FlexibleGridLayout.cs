using System;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public sealed class GridLayoutMaximiser : MonoBehaviour {
        GridLayoutGroup gridLayoutGroup;
        RectTransform rect;
        public float cellRatio = 1; // Public variable for the cell ratio.
        
        // Configurable rectangle ratio
        private int columnCount = 1;
        private int rowCount = 1;
        private float maxCellHeight;
        private float maxCellWidth;

        void Start() {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
            rect = GetComponent<RectTransform>();
            if (!gridLayoutGroup) {
                gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            }
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = 1;
            CalculateCellSize();
        }

        void OnRectTransformDimensionsChange() {
            if (gridLayoutGroup != null && rect != null) {
                CalculateCellSize();
            }
        }

        private void CalculateCellSize() {
            CalculateMaxFit();
            float currentCellWidth = maxCellWidth;
            float currentCellHeight = maxCellHeight;
            gridLayoutGroup.cellSize = new Vector2(currentCellWidth, currentCellHeight);
        }

        public void CalculateBiggestDimensions() {
            float targetWidth = rect.rect.width / columnCount;
            float targetHeight = rect.rect.height / rowCount;
            if (targetHeight > targetWidth) {
                maxCellHeight = Math.Min(targetHeight, targetWidth / cellRatio);
                maxCellWidth = maxCellHeight * cellRatio;
                Debug.Log("Largest Rectangle Width: " + maxCellWidth);
                Debug.Log("Largest Rectangle Height: " + maxCellHeight);
            } else {
                maxCellWidth = Math.Min(targetWidth, targetHeight * cellRatio);
                maxCellHeight = maxCellWidth / cellRatio;
                Debug.Log("Largest Rectangle Width: " + maxCellWidth);
                Debug.Log("Largest Rectangle Height: " + maxCellHeight);
            }
        }

        private void CalculateMaxFit() {
            int n = transform.childCount;

            // Compute the aspect ratio of the rectangle
            float ratio = rect.rect.width / rect.rect.height;
            ratio /= cellRatio;

            // Calculate the number of columns and rows based on the aspect ratio
            float ncolsFloat = (float)Math.Ceiling(Math.Sqrt(n * ratio));
            float nrowsFloat = n / ncolsFloat;

            // Adjust the number of rows and columns to fit the height
            double nrows1 = Math.Ceiling(nrowsFloat);
            double ncols1 = Math.Ceiling(n / nrows1);
            while (nrows1 * ratio < ncols1) {
                nrows1++;
                ncols1 = Math.Ceiling(n / nrows1);
            }
            double cellSize1 = rect.rect.height / nrows1;

            // Adjust the number of rows and columns to fit the width
            double ncols2 = Math.Ceiling(ncolsFloat);
            double nrows2 = Math.Ceiling(n / ncols2);
            while (ncols2 < nrows2 * ratio) {
                ncols2++;
                nrows2 = Math.Ceiling(n / ncols2);
            }
            double cellSize2 = rect.rect.width / ncols2;

            // Determine the best cell size based on the calculated values
            if (cellSize1 < cellSize2) {
                rowCount = (int)nrows2;
                columnCount = (int)ncols2;
                maxCellWidth = (float)(cellSize2);
                maxCellHeight = (float)(cellSize2 / cellRatio);
            } else {
                rowCount = (int)nrows1;
                columnCount = (int)ncols1;
                maxCellWidth = (float)(cellRatio * cellSize1);
                maxCellHeight = (float)cellSize1;
            }

            gridLayoutGroup.constraintCount = rowCount;
        }
    }
}
