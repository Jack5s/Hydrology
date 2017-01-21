using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RasterData;

namespace Hydrology
{
    /// <summary>
    /// 水流分析
    /// </summary>
    public class Flow
    {
        private double[,] dem;
        private int XMax, YMax, Count;
        private Boolean[,] Test;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dem">高程数据</param>
        public Flow(double[,] dem)
        {
            XMax = dem.GetLength(0);
            YMax = dem.GetLength(1);
            this.dem = new double[XMax, YMax];
            Test = new Boolean[XMax, YMax];
            Count = XMax * YMax;
            Array.Copy(dem, this.dem, dem.Length);
           // Border = false;
        }

        /// <summary>
        /// 洼地填充
        /// </summary>
        public double[,] Fill()
        {
            int i, j;
            List<Cell> Flag = new List<Cell>();
            Flag.Capacity = Count;
            bool[,] FlagArr = new bool[XMax, YMax];
            double t = 9999;
            for (i = 1; i < XMax - 1; i++)
            {
                for (j = 1; j < YMax - 1; j++)
                {
                    if (Test[i, j] == false)
                    {
                        //if (dem[i, j] == t)
                        //{
                        //    continue;
                        //}
                        if (dem[i, j] > dem[i, j + 1])
                        {
                            continue;
                        }
                        if (dem[i, j] > dem[i + 1, j + 1])
                        {
                            continue;
                        }
                        if (dem[i, j] > dem[i + 1, j])
                        {
                            continue;
                        }
                        if (dem[i, j] > dem[i + 1, j - 1])
                        {
                            continue;
                        }
                        if (dem[i, j] > dem[i, j - 1])
                        {
                            continue;
                        }
                        if (dem[i, j] > dem[i - 1, j - 1])
                        {
                            continue;
                        }
                        if (dem[i, j] > dem[i - 1, j])
                        {
                            continue;
                        }
                        if (dem[i, j] > dem[i - 1, j + 1])
                        {
                            continue;
                        }
                        Array.Clear(FlagArr, 0, Count);
                        Flag.Clear();
                        GetFlag(dem, FlagArr, Flag, i, j);
                        t = FillFlag(dem, FlagArr, Flag);
                        //t = FillFlag(dem, FlagArr, Flag);
                    }
                }
            }
            return dem;
        }


        private void GetFlag(double[,] dem, bool[,] FlagArr, List<Cell> Flag, int i0, int j0)
        {
            int i, j;
            Cell cell = new Cell(i0, j0);
            Queue<Cell> query = new Queue<Cell>();
            Flag.Add(cell);
            FlagArr[i0, j0] = true;
            query.Enqueue(cell);
            while (query.Count > 0)
            {
                cell = query.Dequeue();
                i = cell.Line;
                j = cell.Column;
                Flag.Add(cell);
                if (j + 1 < YMax)
                {
                    if (FlagArr[i, j + 1] == false)
                    {
                        if (dem[i, j] <= dem[i, j + 1])
                        {
                            cell = new Cell(i, j + 1);
                            FlagArr[i, j + 1] = true;
                            //Flag.Add(cell);
                            query.Enqueue(cell);
                        }
                    }
                }

                if (i + 1 < XMax && j + 1 < YMax)
                {
                    if (FlagArr[i + 1, j + 1] == false)
                    {
                        if (dem[i, j] <= dem[i + 1, j + 1])
                        {
                            cell = new Cell(i + 1, j + 1);
                            FlagArr[i + 1, j + 1] = true;
                            //Flag.Add(cell);
                            query.Enqueue(cell);
                        }
                    }
                }

                if (i + 1 < XMax)
                {
                    if (FlagArr[i + 1, j] == false)
                    {
                        if (dem[i, j] <= dem[i + 1, j])
                        {
                            cell = new Cell(i + 1, j);
                            FlagArr[i + 1, j] = true;
                            //Flag.Add(cell);
                            query.Enqueue(cell);
                        }
                    }
                }

                if (i + 1 < XMax && j - 1 >= 0)
                {
                    if (FlagArr[i + 1, j - 1] == false)
                    {
                        if (dem[i, j] <= dem[i + 1, j - 1])
                        {
                            cell = new Cell(i + 1, j - 1);
                            FlagArr[i + 1, j - 1] = true;
                            //Flag.Add(cell);
                            query.Enqueue(cell);
                        }
                    }
                }

                if (j - 1 >= 0)
                {
                    if (FlagArr[i, j - 1] == false)
                    {
                        if (dem[i, j] <= dem[i, j - 1])
                        {
                            cell = new Cell(i, j - 1);
                            FlagArr[i, j - 1] = true;
                            //Flag.Add(cell);
                            query.Enqueue(cell);
                        }
                    }
                }

                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    if (FlagArr[i - 1, j - 1] == false)
                    {
                        if (dem[i, j] <= dem[i - 1, j - 1])
                        {
                            cell = new Cell(i - 1, j - 1);
                            FlagArr[i - 1, j - 1] = true;
                            //Flag.Add(cell);
                            query.Enqueue(cell);
                        }
                    }
                }

                if (i - 1 >= 0)
                {
                    if (FlagArr[i - 1, j] == false)
                    {
                        if (dem[i, j] <= dem[i - 1, j])
                        {
                            cell = new Cell(i - 1, j);
                            FlagArr[i - 1, j] = true;
                            //Flag.Add(cell);
                            query.Enqueue(cell);
                        }
                    }
                }

                if (i - 1 >= 0 && j + 1 < YMax)
                {
                    if (FlagArr[i - 1, j + 1] == false)
                    {
                        if (dem[i, j] <= dem[i - 1, j + 1])
                        {
                            cell = new Cell(i - 1, j + 1);
                            FlagArr[i - 1, j + 1] = true;
                            //Flag.Add(cell);
                            query.Enqueue(cell);
                        }
                    }
                }
            }
        }

        private double FillFlag(double[,] dem, bool[,] FlagArr, List<Cell> Flag)
        {
            int i, j, k;
            Cell Exit1 = new Cell();
            Exit1.Value = 10000;
            for (i = 0; i < XMax; i++)
            {
                j = 0;
                if (FlagArr[i, j] == true)
                {
                    if (dem[i, j] < Exit1.Value)
                    {
                        Exit1.Line = i;
                        Exit1.Column = j;
                        Exit1.Value = dem[i, j];
                    }
                }
                j = YMax - 1;
                if (FlagArr[i, j] == true)
                {
                    if (dem[i, j] < Exit1.Value)
                    {
                        Exit1.Line = i;
                        Exit1.Column = j;
                        Exit1.Value = dem[i, j];
                    }
                }
            }
            for (j = 0; j < YMax; j++)
            {
                i = 0;
                if (FlagArr[i, j] == true)
                {
                    if (dem[i, j] < Exit1.Value)
                    {
                        Exit1.Line = i;
                        Exit1.Column = j;
                        Exit1.Value = dem[i, j];
                    }
                }
                i = XMax - 1;
                if (FlagArr[i, j] == true)
                {
                    if (dem[i, j] < Exit1.Value)
                    {
                        Exit1.Line = i;
                        Exit1.Column = j;
                        Exit1.Value = dem[i, j];
                    }
                }
            }
            for (k = 0; k < Flag.Count; k++)
            {
                Cell cell = Flag[k];
                i = cell.Line;
                j = cell.Column;
                if (i > 0 && j > 0 && i < XMax - 1 && j < YMax - 1)
                {
                    bool t = IsBorder(FlagArr, i, j);
                    if (t == true)
                    {
                        if (dem[i, j] < Exit1.Value)
                        {
                            Exit1.Line = i;
                            Exit1.Column = j;
                            Exit1.Value = dem[i, j];
                        }
                    }
                }
            }

            for (k = 0; k < Flag.Count; k++)
            {
                Cell cell = Flag[k];
                i = cell.Line;
                j = cell.Column;

                if (dem[i, j] < Exit1.Value)
                {
                    dem[i, j] = Exit1.Value;//填充洼地
                    Test[i, j] = true;//洼地
                }
            }
            return Exit1.Value;
        }
        private double FillFlag2(double[,] dem, bool[,] FlagArr, List<Cell> Flag)
        {
            int i, j, k;
            i = 0; 
            j = 0;
            Cell Exit1 = new Cell();
            Exit1.Value = 10000;
            Cell cell;
            for (k = 0; k < Flag.Count; k++)
            {
                cell = Flag[k];
                i = cell.Line;
                j = cell.Column;
                if (i > 0 && j > 0 && i < XMax - 1 && j < YMax - 1)
                {
                    if (FlagArr[i, j + 1] == false || FlagArr[i + 1, j + 1] == false ||
                        FlagArr[i + 1, j] == false || FlagArr[i + 1, j - 1] == false ||
                        FlagArr[i, j - 1] == false || FlagArr[i - 1, j - 1] == false ||
                        FlagArr[i - 1, j] == false || FlagArr[i - 1, j + 1] == false)
                    {
                        break;
                    }
                }
                else
                {
                    if (FlagArr[i, j] == true)
                    {
                        break;
                    }
                }
            }
            List<Cell> Border = new List<Cell>();
            bool[,] t = new bool[XMax, YMax];
            Border.Capacity = Count;
            GetBorder(FlagArr, Border, i, j, t);
            for (k = 0; k < Border.Count; k++)
            {
                cell = Border[k];
                i = cell.Line;
                j = cell.Column;
                if (dem[i, j] < Exit1.Value)
                {
                    Exit1.Line = i;
                    Exit1.Column = j;
                    Exit1.Value = dem[i, j];
                }
                if (dem[i, j] < Exit1.Value)
                {
                    dem[i, j] = Exit1.Value;//填充洼地
                    Test[i, j] = true;//洼地
                }
            }
            for (k = 0; k < Flag.Count; k++)
            {
                cell = Flag[k];
                i = cell.Line;
                j = cell.Column;
                if (dem[i, j] < Exit1.Value)
                {
                    dem[i, j] = Exit1.Value;//填充洼地
                    Test[i, j] = true;//洼地
                }
            }
            return Exit1.Value;
        }

        private void GetBorder(bool[,] FlagArr, List<Cell> Border, int i, int j, bool[,] t)
        {
            t[i, j] = true;
            if (i > 0 && j > 0 && i < XMax - 1 && j < YMax - 1)
            {
                if (FlagArr[i, j + 1] && FlagArr[i + 1, j + 1] && FlagArr[i + 1, j] && FlagArr[i + 1, j - 1] && FlagArr[i, j - 1] && FlagArr[i - 1, j - 1] && FlagArr[i - 1, j] && FlagArr[i - 1, j + 1])
                {
                    return;
                }
            }
            //Border.Add(new Cell(i, j));
            if (j + 1 < YMax)
            {
                if (FlagArr[i, j + 1] == true && t[i, j + 1] == false)
                {
                    GetBorder(FlagArr, Border, i, j + 1, t);
                }
            }

            if (i + 1 < XMax && j + 1 < YMax)
            {
                if (FlagArr[i + 1, j + 1] == true && t[i + 1, j + 1] == false)
                {
                    GetBorder(FlagArr, Border, i + 1, j + 1, t);
                }
            }

            if (i + 1 < XMax)
            {
                if (FlagArr[i + 1, j] == true && t[i + 1, j] == false)
                {
                    GetBorder(FlagArr, Border, i + 1, j, t);
                }
            }

            if (i + 1 < XMax && j - 1 >= 0)
            {
                if (FlagArr[i + 1, j - 1] == true && t[i + 1, j - 1] == false)
                {
                    GetBorder(FlagArr, Border, i + 1, j - 1, t);
                }
            }

            if (j - 1 >= 0)
            {
                if (FlagArr[i, j - 1] == true && t[i, j - 1] == false)
                {
                    GetBorder(FlagArr, Border, i, j - 1, t);
                }
            }

            if (i - 1 >= 0 && j - 1 >= 0)
            {
                if (FlagArr[i - 1, j - 1] == true && t[i - 1, j - 1] == false)
                {
                    GetBorder(FlagArr, Border, i - 1, j - 1, t);
                }
            }

            if (i - 1 >= 0)
            {
                if (FlagArr[i - 1, j] == true && t[i - 1, j] == false)
                {
                    GetBorder(FlagArr, Border, i - 1, j, t);
                }
            }

            if (i - 1 >= 0 && j + 1 < YMax)
            {
                if (FlagArr[i - 1, j + 1] == true && t[i - 1, j + 1] == false)
                {
                    GetBorder(FlagArr, Border, i - 1, j + 1, t);
                }
            }
        }

        private bool IsBorder(bool[,] Flag, int i, int j)
        {
            if (Flag[i, j + 1] == false)
            {
                return true;
            }
            if (Flag[i + 1, j + 1] == false)
            {
                return true;
            }
            if (Flag[i + 1, j] == false)
            {
                return true;
            }
            if (Flag[i + 1, j - 1] == false)
            {
                return true;
            }
            if (Flag[i, j - 1] == false)
            {
                return true;
            }
            if (Flag[i - 1, j - 1] == false)
            {
                return true;
            }
            if (Flag[i - 1, j] == false)
            {
                return true;
            }
            if (Flag[i - 1, j + 1] == false)
            {
                return true;
            }
            return false;
        }
        private double CalSlope(double[,] dem, int i, int j, int index)
        {
            double slope = 0.0;
            if (index == 0)
            {
                slope = (dem[i, j] - dem[i, j + 1]) / 1;
            }
            if (index == 1)
            {
                slope = (dem[i, j] - dem[i + 1, j + 1]) / (1.414213562);
            }
            if (index == 2)
            {
                slope = (dem[i, j] - dem[i + 1, j]) / 1;
            }
            if (index == 3)
            {
                slope = (dem[i, j] - dem[i + 1, j - 1]) / (1.414213562);
            }
            if (index == 4)
            {
                slope = (dem[i, j] - dem[i, j - 1]) / 1;
            }
            if (index == 5)
            {
                slope = (dem[i, j] - dem[i - 1, j - 1]) / (1.414213562);
            }
            if (index == 6)
            {
                slope = (dem[i, j] - dem[i - 1, j]) / 1;
            }
            if (index == 7)
            {
                slope = (dem[i, j] - dem[i - 1, j + 1]) / (1.414213562);
            }
            slope = System.Math.Abs(slope);
            return slope;
        }

        /// <summary>
        /// 求水流方向
        /// </summary>
        /// <returns></returns>
        public int[,] FlowDirection()
        {
            int[,] result = new int[XMax, YMax];
            int i, j, k;
            double[] slope = new double[8];
            for (i = 1; i < XMax - 1; i++)
            {
                for (j = 1; j < YMax - 1; j++)
                {
                    for (k = 0; k < 8; k++)
                    {
                        slope[k] = CalSlope(dem, i, j, k);
                    }
                    result[i, j] = Max(slope);
                    Initialize(slope);
                }
            }
            for (i = 1; i < XMax - 1; i++)
            {
                slope[0] = CalSlope(dem, i, 0, 0);
                slope[1] = CalSlope(dem, i, 0, 1);
                slope[2] = CalSlope(dem, i, 0, 2);
                slope[6] = CalSlope(dem, i, 0, 6);
                slope[7] = CalSlope(dem, i, 0, 7);
                result[i, 0] = Max(slope);
                Initialize(slope);
                slope[2] = CalSlope(dem, i, YMax - 1, 2);
                slope[3] = CalSlope(dem, i, YMax - 1, 3);
                slope[4] = CalSlope(dem, i, YMax - 1, 4);
                slope[5] = CalSlope(dem, i, YMax - 1, 5);
                slope[6] = CalSlope(dem, i, YMax - 1, 6);
                result[i, YMax - 1] = Max(slope);
                Initialize(slope);
            }
            for (j = 1; j < YMax - 1; j++)
            {
                slope[0] = CalSlope(dem, 0, j, 0);
                slope[1] = CalSlope(dem, 0, j, 1);
                slope[2] = CalSlope(dem, 0, j, 2);
                slope[3] = CalSlope(dem, 0, j, 3);
                slope[4] = CalSlope(dem, 0, j, 4);
                result[0, j] = Max(slope);
                Initialize(slope);
                slope[0] = CalSlope(dem, XMax - 1, j, 0);
                slope[4] = CalSlope(dem, XMax - 1, j, 4);
                slope[5] = CalSlope(dem, XMax - 1, j, 5);
                slope[6] = CalSlope(dem, XMax - 1, j, 6);
                slope[7] = CalSlope(dem, XMax - 1, j, 7);
                result[XMax - 1, j] = Max(slope);
                Initialize(slope);
            }

            slope[0] = CalSlope(dem, 0, 0, 0);
            slope[1] = CalSlope(dem, 0, 0, 1);
            slope[2] = CalSlope(dem, 0, 0, 2);
            result[0, 0] = Max(slope);
            Initialize(slope);

            slope[2] = CalSlope(dem, 0, YMax - 1, 2);
            slope[3] = CalSlope(dem, 0, YMax - 1, 3);
            slope[4] = CalSlope(dem, 0, YMax - 1, 4);
            result[0, YMax - 1] = Max(slope);
            Initialize(slope);

            slope[6] = CalSlope(dem, XMax - 1, 0, 6);
            slope[7] = CalSlope(dem, XMax - 1, 0, 7);
            slope[0] = CalSlope(dem, XMax - 1, 0, 0);
            result[XMax - 1, 0] = Max(slope);
            Initialize(slope);

            slope[4] = CalSlope(dem, XMax - 1, YMax - 1, 4);
            slope[5] = CalSlope(dem, XMax - 1, YMax - 1, 5);
            slope[6] = CalSlope(dem, XMax - 1, YMax - 1, 6);
            result[XMax - 1, YMax - 1] = Max(slope);
            Initialize(slope);

            return result;
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
            return 1;
        }
        private void Initialize(double[] a)
        {
            int i;
            for (i = 0; i < a.GetLength(0); i++)
            {
                a[i] = 0;
            }
        }

        /// <summary>
        /// 求汇流累积量
        /// </summary>
        /// <param name="Direction">水流方向矩阵</param>
        /// <returns></returns>
        public int[,] Accumulation(int[,] Direction)
        {
            int i, j;
            int[,] result = new int[Direction.GetLength(0), Direction.GetLength(1)];
            Boolean[,] Origin = CalOrigin(Direction);
            Boolean[,] OutFlow = CalOut(Direction);
            for (i = 0; i < Direction.GetLength(0); i++)
            {
                for (j = 0; j < Direction.GetLength(1); j++)
                {
                    if (OutFlow[i, j] == true)
                    {
                        Add(Direction, Origin, result, i, j);
                    }
                }
            }
            return result;
        }
        private Boolean[,] CalOut(int[,] Direction)
        {
            int i, j;
            Boolean[,] flag = new Boolean[Direction.GetLength(0), Direction.GetLength(1)];
            for (j = 0; j < Direction.GetLength(1); j++)
            {
                i = 0;
                if (Direction[i, j] >= 32)
                {
                    flag[i, j] = true;
                }
                else
                {
                    flag[i, j] = false;
                }
                i = Direction.GetLength(0) - 1;
                if (Direction[i, j] >= 2 && Direction[i, j] <= 8)
                {
                    flag[i, j] = true;
                }
                else
                {
                    flag[i, j] = false;
                }
            }
            for (i = 0; i < Direction.GetLength(0); i++)
            {
                j = 0;
                if (Direction[i, j] >= 8 && Direction[i, j] <= 32)
                {
                    flag[i, j] = true;
                }
                else
                {
                    flag[i, j] = false;
                }
                j = Direction.GetLength(1) - 1;
                if (Direction[i, j] == 1 || Direction[i, j] == 2 || Direction[i, j] == 128)
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
        private int Add(int[,] Direction, Boolean[,] Origin, int[,] Result, int i, int j)
        {
            if (Origin[i, j] == true)
            {
                Result[i, j] = 0;
                return 0;
            }
            if (j - 1 >= 0)
            {
                if (Direction[i, j - 1] == 1)
                {
                    Result[i, j] += Add(Direction, Origin, Result, i, j - 1);
                    Result[i, j]++;
                }
            }
            if (i - 1 >= 0 && j - 1 >= 0)
            {
                if (Direction[i - 1, j - 1] == 2)
                {
                    Result[i, j] += Add(Direction, Origin, Result, i - 1, j - 1);
                    Result[i, j]++;
                }
            }
            if (i - 1 >= 0)
            {
                if (Direction[i - 1, j] == 4)
                {

                    Result[i, j] += Add(Direction, Origin, Result, i - 1, j);
                    Result[i, j]++;
                }
            }
            if (i - 1 >= 0 && j + 1 < Direction.GetLength(1))
            {
                if (Direction[i - 1, j + 1] == 8)
                {
                    Result[i, j] += Add(Direction, Origin, Result, i - 1, j + 1);
                    Result[i, j]++;
                }
            }
            if (j + 1 < Direction.GetLength(1))
            {
                if (Direction[i, j + 1] == 16)
                {
                    Result[i, j] += Add(Direction, Origin, Result, i, j + 1);
                    Result[i, j]++;
                }
            }
            if (i + 1 < Direction.GetLength(0) && j + 1 < Direction.GetLength(1))
            {
                if (Direction[i + 1, j + 1] == 32)
                {
                    Result[i, j] += Add(Direction, Origin, Result, i + 1, j + 1);
                    Result[i, j]++;
                }
            }
            if (i + 1 < Direction.GetLength(0))
            {
                if (Direction[i + 1, j] == 64)
                {
                    Result[i, j] += Add(Direction, Origin, Result, i + 1, j);
                    Result[i, j]++;
                }
            }
            if (i + 1 < Direction.GetLength(0) && j - 1 >= 0)
            {
                if (Direction[i + 1, j - 1] == 128)
                {
                    Result[i, j] += Add(Direction, Origin, Result, i + 1, j - 1);
                    Result[i, j]++;
                }
            }
            return Result[i, j];
        }
        private Boolean[,] CalOrigin(int[,] Direction)
        {
            int i, j;
            Boolean[,] origin = new Boolean[Direction.GetLength(0), Direction.GetLength(1)];
            for (j = 0; j < origin.GetLength(1); j++)
            {
                origin[0, j] = true;
                origin[Direction.GetLength(0) - 1, j] = true;
            }
            for (i = 0; i < origin.GetLength(0); i++)
            {
                origin[i, 0] = true;
                origin[i, Direction.GetLength(1) - 1] = true;
            }
            for (i = 0; i < origin.GetLength(0); i++)
            {
                for (j = 0; j < origin.GetLength(1); j++)
                {
                    if (GetNumber(Direction, i, j) == 0)
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
