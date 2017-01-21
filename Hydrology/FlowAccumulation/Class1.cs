using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowAccumulation
{
    public class Class1
    {
        public int[,] Accumulation(int[,] direction)
        {
            int i, j;
            int[,] result = new int[direction.GetLength(0), direction.GetLength(1)];
            Boolean[,] origin = CalOrigin(direction);
            Boolean[,] OutFlow = CalOut(direction);
            for (i = 0; i < direction.GetLength(0); i++)
            {
                for (j = 0; j < direction.GetLength(1); j++)
                {
                    if (direction[i, j] == 0)
                    {
                        Add(direction, origin, result, i, j);
                    }
                }
            }
            return result;
        }
        private Boolean[,] CalOut(int[,] test)
        {
            int i, j;
            Boolean[,] flag = new Boolean[test.GetLength(0), test.GetLength(1)];
            for (j = 0; j < test.GetLength(1); j++)
            {
                i = 0;
                if (test[i, j] >= 32)
                {
                    flag[i, j] = true;
                }
                else
                {
                    flag[i, j] = false;
                }
                i = test.GetLength(0) - 1;
                if (test[i, j] >= 2 && test[i, j] <= 8)
                {
                    flag[i, j] = true;
                }
                else
                {
                    flag[i, j] = false;
                }
            }
            for (i = 0; i < test.GetLength(0); i++)
            {
                j = 0;
                if (test[i, j] >= 8 && test[i, j] <= 32)
                {
                    flag[i, j] = true;
                }
                else
                {
                    flag[i, j] = false;
                }
                j = test.GetLength(1) - 1;
                if (test[i, j] == 1 || test[i, j] == 2 || test[i, j] == 128)
                {
                    flag[i, j] = true;
                }
                else
                {
                    flag[i, j] = false;
                }
            }
            return flag;
        }
        private int Add(int[,] direction, Boolean[,] origin, int[,] result, int i, int j)
        {
            if (origin[i, j] == true)
            {
                result[i, j] = 0;
                return 0;
            }
            if (j - 1 >= 0)
            {
                if (direction[i, j - 1] == 1)
                {
                    result[i, j] += Add(direction, origin, result, i, j - 1);
                    result[i, j]++;
                }
            }
            if (i - 1 >= 0 && j - 1 >= 0)
            {
                if (direction[i - 1, j - 1] == 2)
                {
                    result[i, j] += Add(direction, origin, result, i - 1, j - 1);
                    result[i, j]++;
                }
            }
            if (i - 1 >= 0)
            {
                if (direction[i - 1, j] == 4)
                {

                    result[i, j] += Add(direction, origin, result, i - 1, j);
                    result[i, j]++;
                }
            }
            if (i - 1 >= 0 && j + 1 < direction.GetLength(1))
            {
                if (direction[i - 1, j + 1] == 8)
                {
                    result[i, j] += Add(direction, origin, result, i - 1, j + 1);
                    result[i, j]++;
                }
            }
            if (j + 1 < direction.GetLength(1))
            {
                if (direction[i, j + 1] == 16)
                {
                    result[i, j] += Add(direction, origin, result, i, j + 1);
                    result[i, j]++;
                }
            }
            if (i + 1 < direction.GetLength(0) && j + 1 < direction.GetLength(1))
            {
                if (direction[i + 1, j + 1] == 32)
                {
                    result[i, j] += Add(direction, origin, result, i + 1, j + 1);
                    result[i, j]++;
                }
            }
            if (i + 1 < direction.GetLength(0))
            {
                if (direction[i + 1, j] == 64)
                {
                    result[i, j] += Add(direction, origin, result, i + 1, j);
                    result[i, j]++;
                }
            }
            if (i + 1 < direction.GetLength(0) && j - 1 >= 0)
            {
                if (direction[i + 1, j - 1] == 128)
                {
                    result[i, j] += Add(direction, origin, result, i + 1, j - 1);
                    result[i, j]++;
                }
            }
            return result[i, j];
        }

        private Boolean[,] CalOrigin(int[,] direction)
        {
            int i, j;
            Boolean[,] origin = new Boolean[direction.GetLength(0), direction.GetLength(1)];
            for (i = 0; i < origin.GetLength(1); i++)
            {
                origin[0, i] = true;
                origin[direction.GetLength(0) - 1, i] = true;
            }
            for (i = 0; i < origin.GetLength(0); i++)
            {
                origin[i, 0] = true;
                origin[i, direction.GetLength(1) - 1] = true;
            }
            for (i = 0; i < origin.GetLength(0); i++)
            {
                for (j = 0; j < origin.GetLength(1); j++)
                {
                    if (GetNumber(direction, i, j) == 0)
                    {
                        origin[i, j] = true;
                    }
                    else
                    {
                        origin[i, j] = false;
                    }
                }
            }
            return origin;
        }

        private int GetNumber(int[,] t, int i, int j)
        {
            int num = 0;
            if (j - 1 >= 0)
            {
                if (t[i, j - 1] == 1)
                {
                    num++;
                }
            }
            if (i - 1 >= 0 && j - 1 >= 0)
            {
                if (t[i - 1, j - 1] == 2)
                {
                    num++;
                }
            }
            if (i - 1 >= 0)
            {
                if (t[i - 1, j] == 4)
                {
                    num++;
                }
            }
            if (i - 1 >= 0 && j + 1 < t.GetLength(1))
            {
                if (t[i - 1, j + 1] == 8)
                {
                    num++;
                }
            }
            if (j + 1 < t.GetLength(1))
            {
                if (t[i, j + 1] == 16)
                {
                    num++;
                }
            }
            if (i + 1 < t.GetLength(0) && j + 1 < t.GetLength(1))
            {
                if (t[i + 1, j + 1] == 32)
                {
                    num++;
                }
            }
            if (i + 1 < t.GetLength(0))
            {
                if (t[i + 1, j] == 64)
                {
                    num++;
                }
            }
            if (i + 1 < t.GetLength(0) && j - 1 >= 0)
            {
                if (t[i + 1, j - 1] == 128)
                {
                    num++;
                }
            }
            return num;
        }
    }
}
