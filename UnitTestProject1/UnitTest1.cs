using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticeTask8;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Form1 form = new Form1();
            TestGenerator.Generate_Tests();
            for (int i = 1; i <= 22; i++)
            {
                form.Convert_From_File($"test{i}.txt");
            }
            
        }
    }
}
