
using UnityEngine;

namespace TriangleNet
{
    using System.Linq;

    using System;
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    static class Generate
    {
        public static List<Vertex> RandomPoints(int n, WindowSize bounds)
        {
            var points = new List<Vertex>(n);

            var xmin = bounds.LeftBorder;
            var ymin = bounds.BottomBorder;

            var width = bounds.Width;
            var height = bounds.Height;

            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                double x = random.NextDouble();
                double y = random.NextDouble();
                points.Add(new Vertex(xmin + x * width, ymin + y * height));
            }

            return points;
        }

        public static List<Vertex> GetLatinSpreadPoints(int n, ref float shapeSize, WindowSize bounds)
        {
            retry:
            var points = new List<Vertex>(n);

            var xmin = bounds.LeftBorder;
            var ymin = bounds.TopBorder;

            var width = bounds.Width;
            var height = bounds.Height;
            Debug.Log("top border" + bounds.TopBorder);
            int columnsno = (int)(width / shapeSize);
            int rowsno = (int)Math.Floor(height / shapeSize);
            var availableRows = Enumerable.Range(0, rowsno).ToList();
            var availableColumns = Enumerable.Range(0, columnsno).ToList();



            Random random = new Random();
            Debug.Log("rowsnno: " + rowsno + "colsno: " + columnsno);
            if (rowsno < n)
            {
                shapeSize = (int)Math.Floor(height / n);
                goto retry;
            }

            for (int i = 0; i < n; i++)
            {
                // int row = 0;
                int rowIndex = random.Next(availableRows.Count);
                Debug.Log("rowcount:" + availableRows.Count);
                Debug.Log("row:" + rowIndex);
                Debug.Log("rowno:" + availableRows[rowIndex]);
                var row = availableRows[rowIndex];
                availableRows.RemoveAt(rowIndex);
                // int col = i;row
                int colIndex = random.Next(availableColumns.Count);
                var col = availableColumns[colIndex];
                availableColumns.RemoveAt(colIndex);

                var vert = new Vertex(xmin + (col * shapeSize) + shapeSize / 2,
                    ymin - (row * shapeSize) - shapeSize / 2);
                points.Add(vert);
            }

            return points;
        }
        /// <summary>
        /// Creates a rectangle contour.
        /// </summary>
        public static Contour Rectangle(Rectangle rect, double size = 0d, int label = 0)
        {
            return Rectangle(rect.X, rect.Y, rect.Width, rect.Height, size, label);
        }

        /// <summary>
        /// Creates a rectangle contour.
        /// </summary>
        /// <param name="x">Minimum x value (left).</param>
        /// <param name="y">Minimum y value (bottom).</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="size">The desired boundary segment length.</param>
        /// <param name="label">The vertices and boundary segment label.</param>
        /// <returns></returns>
        public static Contour Rectangle(double x, double y, double width, double height,
            double size = 0d, int label = 0)
        {
            // Horizontal and vertical step sizes.
            double stepH = 0d;
            double stepV = 0d;

            int nH = 1;
            int nV = 1;

            if (size > 0d)
            {
                size = Math.Min(size, Math.Min(width, height));

                nH = (int)Math.Ceiling(width / size);
                nV = (int)Math.Ceiling(height / size);

                stepH = width / nH;
                stepV = height / nV;
            }

            var points = new List<Vertex>(2 * nH + 2 * nV);

            double right = x + width;
            double top = y + height;

            // Left box boundary points
            for (int i = 0; i < nV; i++)
            {
                points.Add(new Vertex(x, y + i * stepV, label));
            }

            // Top box boundary points
            for (int i = 0; i < nH; i++)
            {
                points.Add(new Vertex(x + i * stepH, top, label));
            }

            // Right box boundary points
            for (int i = 0; i < nV; i++)
            {
                points.Add(new Vertex(right, top - i * stepV, label));
            }

            // Bottom box boundary points
            for (int i = 0; i < nH; i++)
            {
                points.Add(new Vertex(right - i * stepH, y, label));
            }

            return new Contour(points, label, true);
        }

        /// <summary>
        /// Create a circular contour.
        /// </summary>
        /// <param name="r">The radius.</param>
        /// <param name="center">The center point.</param>
        /// <param name="n">The number of segments.</param>
        /// <param name="label">The boundary label.</param>
        /// <returns>A circular contour.</returns>
        public static List<Vertex> CirclePoints(double r, Point center, int n, int label = 0)
        {

            double dphi = 2 * Math.PI / n;
            // double h = 2 * Math.PI * r / n;

            var points = new List<Vertex>(n);

            for (int i = 0; i < n; i++)
            {
                double x = center.X + r * Math.Cos(i * dphi);
                double y = center.Y + r * Math.Sin(i * dphi);

                points.Add(new Vertex(x, y));
            }

            return points;
        }
    }
}