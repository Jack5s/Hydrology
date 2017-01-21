using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RasterData;
using Hydrology;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime Start = DateTime.Now;
            //string InputFileName = @"D:\ArcGISData\dem\dem.txt";
            //string TestFileName = @"D:\ArcGISData\dem\filldem.txt";

            string InputFileName = @"D:\ArcGISData\dem\chinarect.txt";
            string TestFileName = @"D:\ArcGISData\dem\fillchinarec.txt";
            string ResultFileName = @"D:\ArcGISData\dem\Cal.txt";
            string TestRFileName = @"D:\ArcGISData\dem\t.txt";
            Raster InputData = new Raster();
            InputData = ReadFile(InputFileName);
            Console.WriteLine("读取成功!");            
            Flow a = new Flow(InputData.dem);

            Raster resultRaster = new Raster();
            Start = DateTime.Now;
            resultRaster.dem = a.Fill();
            DateTime End = DateTime.Now;
            TimeSpan t = End - Start;
            Console.WriteLine(t.ToString());
            WriteFile(ResultFileName, resultRaster);

            Raster TestRaster = ReadFile(TestFileName);
            Raster OutputData = ReadFile(InputFileName);
  
            int i, j;
            for (i = 0; i < OutputData.dem.GetLength(0); i++)
            {
                for (j = 0; j < OutputData.dem.GetLength(1); j++)
                {
                    OutputData.dem[i, j] = resultRaster.dem[i, j] - TestRaster.dem[i, j];
                }
            }
            WriteFile(TestRFileName, OutputData);
            Console.WriteLine("计算完成!");
            Console.ReadKey();
        }

        static void output(int[,] a)
        {
            int i, j;
            for (i = 0; i < a.GetLength(0); i++)
            {
                for (j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0},", a[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static void output(Boolean[,] a)
        {
            int i, j;
            for (i = 0; i < a.GetLength(0); i++)
            {
                for (j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == true)
                        Console.Write("1,");
                    else
                        Console.Write("0,");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static void output(double[,] a)
        {
            int i, j;
            for (i = 0; i < a.GetLength(0); i++)
            {
                for (j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0},", a[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static Raster ReadFile(string FileName)
        {
            Raster raster = new Raster();
            StreamReader streamread = new StreamReader(FileName, System.Text.ASCIIEncoding.Default);
            int i, j;
            string dataline;
            string[] data;
            dataline = streamread.ReadLine();
            data = dataline.Split(' ');
            raster.Column = Convert.ToInt16(data[9]);

            dataline = streamread.ReadLine();
            data = dataline.Split(' ');
            raster.Line = Convert.ToInt16(data[9]);

            dataline = streamread.ReadLine();
            data = dataline.Split(' ');
            raster.XCorner = Convert.ToDouble(data[5]);

            dataline = streamread.ReadLine();
            data = dataline.Split(' ');
            raster.YCorner = Convert.ToDouble(data[5]);

            dataline = streamread.ReadLine();
            data = dataline.Split(' ');
            raster.CellSize = Convert.ToInt16(data[6]);

            dataline = streamread.ReadLine();
            data = dataline.Split(' ');
            raster.NoData = Convert.ToDouble(data[2]);

            raster.dem = new double[raster.Line, raster.Column];
            i = 0;
            for (i = 0; i < raster.Line; i++)
            {
                dataline = streamread.ReadLine();
                data = dataline.Split(' ');
                for (j = 0; j < raster.Column; j++)
                {
                    raster.dem[i, j] = Convert.ToDouble(data[j]);
                }
            }
            streamread.Close();
            return raster;
        }
        static void WriteFile(string FileName,Raster raster)
        {
            StreamWriter streamwrite = new StreamWriter(FileName);
            int i, j;
            streamwrite.WriteLine("ncols         " + raster.Column.ToString());
            streamwrite.WriteLine("nrows         " + raster.Line.ToString());
            streamwrite.WriteLine("xllcorner     " + raster.XCorner.ToString());
            streamwrite.WriteLine("yllcorner     " + raster.YCorner.ToString());
            streamwrite.WriteLine("cellsize      " + raster.CellSize.ToString());
            streamwrite.WriteLine("NODATA_value  " + raster.NoData.ToString());
            for (i = 0; i < raster.Line; i++)
            {
                for (j = 0; j < raster.Column; j++)
                {
                    streamwrite.Write(raster.dem[i, j].ToString()+' ');
                }
                streamwrite.WriteLine();
            }
            streamwrite.Close();
        }
    }
}