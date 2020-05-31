using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
namespace PracticeTask8
{
    public static class TestGenerator
    {
        static int[,] matrix = null;
        static Random random = new Random();
        static string[] Convert_To_Strings(string separator)
        {
            string[] lines = null;
            if (matrix != null)
            {
                lines = new string[matrix.GetLength(0)];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        lines[i] += matrix[i, j].ToString();
                        if (j < matrix.GetLength(1) - 1) lines[i] += separator;
                    }
                }
            }
            return lines;
        }
        static void In_File(string name, string separator)
        {
            string[] lines = Convert_To_Strings(separator);
            File.WriteAllLines(name, lines);
        }
        static int[,] Generate_Matrix_Cases(int case_value, [Optional] int rows, [Optional] int columns)
        {
            switch(case_value)
            {
                case 0:
                    {
                        matrix = new int[rows, columns];
                        for(int i = 0; i < rows; i++)
                        {
                            for(int j = i; j < columns; j++)
                            {
                                if (i != j)
                                {
                                    matrix[i, j] = random.Next(2);
                                    matrix[j, i] = matrix[i, j];
                                }
                                else matrix[i, j] = 0;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        matrix = new int[rows, columns];
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                matrix[i, j] = random.Next(-100, 100);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        matrix = new int[rows, columns];
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                matrix[i, j] = 0;
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        matrix = new int[rows, columns];
                        int index = random.Next(rows);
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                if (i == index || i == j) matrix[i, j] = 0;
                                else matrix[i, j] = random.Next(2);
                                matrix[j, i] = matrix[i, j];
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        matrix = new int[8, 8] {    { 0, 1, 0, 0, 0, 0, 0, 0 },
                                                    { 1, 0, 1, 1, 0, 0, 0, 0 },
                                                    { 0, 1, 0, 0, 0, 0, 0, 1 },
                                                    { 0, 1, 0, 0, 0, 0, 1, 0 },
                                                    { 0, 0, 0, 0, 0, 1, 0, 0 },
                                                    { 0, 0, 0, 0, 1, 0, 0, 0 },
                                                    { 0, 0, 0, 1, 0, 0, 0, 1 },
                                                    { 0, 0, 1, 0, 0, 0, 1, 0 },
                        };
                        break;
                    }
                case 5:
                    {
                        matrix = new int[20, 20]
                        {
                            { 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            { 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            { 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0}, };
                        break;
                    }
                case 6:
                    {
                        matrix = new int[1, 1] { { 1 } };
                        break;
                    }
                case 7:
                    {
                        matrix = new int[1, 1] { { 0 } };
                        break;
                    }
                case 8:
                    {
                        matrix = new int[2, 2] { { 1, 0 }, { 0, 1 } };
                        break;
                    }
                case 9:
                    {
                        matrix = new int[2, 2] { { 0, 1 }, { 0, 0 } };
                        break;
                    }
                case 10:
                    {
                        matrix = new int[8, 8]
                        {
                           {0, 1, 0, 0, 0, 0, 0, 0},
                           {1, 0, 1, 0, 0, 0, 0, 0},
                           {0, 1, 0, 0, 0, 0, 0, 0},
                           {0, 0, 0, 0, 0, 0, 1, 0},
                           {0, 0, 0, 0, 0, 1, 0, 0},
                           {0, 0, 0, 0, 1, 0, 0, 0},
                           {0, 0, 0, 1, 0, 0, 0, 1},
                           {0, 0, 0, 0, 0, 0, 1, 0},
                        };
                        break;
                    }
            }
            return matrix;
        }
        static void Test_1()
        {
            matrix = Generate_Matrix_Cases(0, 5, 4);
            In_File("test1.txt", " ");
        }
        static void Test_2()
        {
            matrix = Generate_Matrix_Cases(0, 5, 5);
            In_File("test2.txt", " ");
        }
        static void Test_3()
        {
            matrix = Generate_Matrix_Cases(0);
            In_File("test3.txt", " ");
        }
        static void Test_4()
        {
            matrix = Generate_Matrix_Cases(0, 20, 20);
            In_File("test4.txt", " ");
        }
        static void Test_5()
        {
            matrix = Generate_Matrix_Cases(0, 1, 1);
            In_File("test5.txt", " ");
        }
        static void Test_6()
        {
            matrix = Generate_Matrix_Cases(0, 21, 21);
            In_File("test6.txt", " ");
        }
        static void Test_7()
        {
            matrix = Generate_Matrix_Cases(1, 20, 20);
            In_File("test7.txt", " ");
        }
        static void Test_8()
        {
            matrix = Generate_Matrix_Cases(1, 1, 1);
            In_File("test8.txt", " ");
        }
        static void Test_9()
        {
            matrix = Generate_Matrix_Cases(1, 25, 25);
            In_File("test9.txt", " ");
        }
        static void Test_10()
        {
            matrix = Generate_Matrix_Cases(1);
            In_File("test10.txt", " ");
        }
        static void Test_11()
        {
            matrix = Generate_Matrix_Cases(2, 10, 10);
            In_File("test11.txt", " ");
        }
        static void Test_12()
        {
            matrix = Generate_Matrix_Cases(3, 10, 10);
            In_File("test12.txt", " ");
        }
        static void Test_13()
        {
            matrix = Generate_Matrix_Cases(4);
            In_File("test13.txt", " ");
        }
        static void Test_14()
        {
            matrix = Generate_Matrix_Cases(5);
            In_File("test14.txt", " ");
        }
        static void Test_15()
        {
            matrix = Generate_Matrix_Cases(6);
            In_File("test15.txt", " ");
        }
        static void Test_16()
        {
            matrix = Generate_Matrix_Cases(7);
            In_File("test16.txt", " ");
        }
        static void Test_17()
        {
            matrix = Generate_Matrix_Cases(8);
            In_File("test17.txt", " ");
        }
        static void Test_18()
        {
            matrix = Generate_Matrix_Cases(9);
            In_File("test18.txt", " ");
        }
        static void Test_19()
        {
            matrix = Generate_Matrix_Cases(10);
            In_File("test19.txt", " ");
        }
        static void Test_20()
        {
            matrix = Generate_Matrix_Cases(0, 10, 10);
            In_File("test20.txt", ", ");
        }
        static void Test_21()
        {
            matrix = Generate_Matrix_Cases(0, 10, 10);
            In_File("test21.txt", ",");
        }
        static void Test_22()
        {
            matrix = Generate_Matrix_Cases(0, 10, 10);
            In_File("test22.txt", "\t");
        }
        public static void Generate_Tests()
        {
            Test_1();
            Test_2();
            Test_3();
            Test_4();
            Test_5();
            Test_6();
            Test_7();
            Test_8();
            Test_9();
            Test_10();
            Test_11();
            Test_12();
            Test_13();
            Test_14();
            Test_15();
            Test_16();
            Test_17();
            Test_18();
            Test_19();
            Test_20();
            Test_21();
            Test_22();
        }
    }
}
