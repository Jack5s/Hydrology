using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowDirection
{
    public class Class1
    {
        public int[,] FlowDirection(double[,] dem)
        {
            int[,] result = new int[dem.GetLength(0), dem.GetLength(1)];
            int i, j, k;
            double[] slope = new double[8];
            for (i = 1; i < dem.GetLength(0) - 1; i++)
            {
                for (j = 1; j < dem.GetLength(1) - 1; j++)
                {
                    for (k = 0; k < 8; k++)
                    {
                        slope[k] = CalSlope(dem, i, j, k);
                    }
                    result[i, j] = Max(slope);
                    Initialize(slope);
                }
            }

            for (i = 1; i < dem.GetLength(0) - 1; i++)
            {
                slope[0] = CalSlope(dem, i, 0, 0);
                slope[1] = CalSlope(dem, i, 0, 1);
                slope[2] = CalSlope(dem, i, 0, 2);
                slope[6] = CalSlope(dem, i, 0, 6);
                slope[7] = CalSlope(dem, i, 0, 7);
                result[i, 0] = Max(slope);
                Initialize(slope);
                slope[2] = CalSlope(dem, i, dem.GetLength(1) - 1, 2);
                slope[3] = CalSlope(dem, i, dem.GetLength(1) - 1, 3);
                slope[4] = CalSlope(dem, i, dem.GetLength(1) - 1, 4);
                slope[5] = CalSlope(dem, i, dem.GetLength(1) - 1, 5);
                slope[6] = CalSlope(dem, i, dem.GetLength(1) - 1, 6);
                result[i, dem.GetLength(1) - 1] = Max(slope);
                Initialize(slope);
            }
            for (j = 1; j < dem.GetLength(1) - 1; j++)
            {
                slope[0] = CalSlope(dem, 0, j, 0);
                slope[1] = CalSlope(dem, 0, j, 1);
                slope[2] = CalSlope(dem, 0, j, 2);
                slope[3] = CalSlope(dem, 0, j, 3);
                slope[4] = CalSlope(dem, 0, j, 4);
                result[0, j] = Max(slope);
                Initialize(slope);
                slope[0] = CalSlope(dem, dem.GetLength(0) - 1, j, 0);
                slope[4] = CalSlope(dem, dem.GetLength(0) - 1, j, 4);
                slope[5] = CalSlope(dem, dem.GetLength(0) - 1, j, 5);
                slope[6] = CalSlope(dem, dem.GetLength(0) - 1, j, 6);
                slope[7] = CalSlope(dem, dem.GetLength(0) - 1, j, 7);
                result[dem.GetLength(0) - 1, j] = Max(slope);
                Initialize(slope);
            }

            slope[0] = CalSlope(dem, 0, 0, 0);
            slope[1] = CalSlope(dem, 0, 0, 1);
            slope[2] = CalSlope(dem, 0, 0, 2);
            result[0, 0] = Max(slope);
            Initialize(slope);

            slope[2] = CalSlope(dem, 0, dem.GetLength(1) - 1, 2);
            slope[3] = CalSlope(dem, 0, dem.GetLength(1) - 1, 3);
            slope[4] = CalSlope(dem, 0, dem.GetLength(1) - 1, 4);
            result[0, dem.GetLength(1) - 1] = Max(slope);
            Initialize(slope);

            slope[6] = CalSlope(dem, dem.GetLength(0) - 1, 0, 6);
            slope[7] = CalSlope(dem, dem.GetLength(0) - 1, 0, 7);
            slope[0] = CalSlope(dem, dem.GetLength(0) - 1, 0, 0);
            result[dem.GetLength(0) - 1, 0] = Max(slope);
            Initialize(slope);

            slope[4] = CalSlope(dem, dem.GetLength(0) - 1, dem.GetLength(1) - 1, 4);
            slope[5] = CalSlope(dem, dem.GetLength(0) - 1, dem.GetLength(1) - 1, 5);
            slope[6] = CalSlope(dem, dem.GetLength(0) - 1, dem.GetLength(1) - 1, 6);
            result[dem.GetLength(0) - 1, dem.GetLength(1) - 1] = Max(slope);
            Initialize(slope);

            return result;
        }
        private double CalSlope(double[,] dem, int i, int j, int index)
        {
            int size = 1;
            double slope = 0.0;
            if (index == 0)
            {
                slope = (dem[i, j] - dem[i, j + 1]) / size;
                return slope;
            }
            if (index == 1)
            {
                slope = (dem[i, j] - dem[i + 1, j + 1]) / (size * 1.414213562);
                return slope;
            }
            if (index == 2)
            {
                slope = (dem[i, j] - dem[i + 1, j]) / size;
                return slope;
            }
            if (index == 3)
            {
                slope = (dem[i, j] - dem[i + 1, j - 1]) / (size * 1.414213562);
                return slope;
            }
            if (index == 4)
            {
                slope = (dem[i, j] - dem[i, j - 1]) / size;
                return slope;
            }
            if (index == 5)
            {
                slope = (dem[i, j] - dem[i - 1, j - 1]) / (size * 1.414213562);
                return slope;
            }
            if (index == 6)
            {
                slope = (dem[i, j] - dem[i - 1, j]) / size;
                return slope;
            }
            if (index == 7)
            {
                slope = (dem[i, j] - dem[i - 1, j + 1]) / (size * 1.414213562);
                return slope;
            }
            return slope;
        }
        private int Max(double[] slope)
        {
            int i, j;
            double m = 0.0;
            j = -1;
            for (i = 0; i < 8; i++)
            {
                if (slope[i] > m)
                {
                    j = i;
                    m = slope[i];
                }
            }
            if (j == 0)
            {
                return 1;
            }
            if (j == 1)
            {
                return 2;
            }
            if (j == 2)
            {
                return 4;
            }
            if (j == 3)
            {
                return 8;
            }
            if (j == 4)
            {
                return 16;
            }
            if (j == 5)
            {
                return 32;
            }
            if (j == 6)
            {
                return 64;
            }
            if (j == 7)
            {
                return 128;
            }
            return 0;
        }
        private void Initialize(double[] a)
        {
            int i;
            for(i=0;i<a.GetLength(0);i++)
            {
                a[i] = 0;
            }
        }
    }
}
