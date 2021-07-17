using System;
using System.Diagnostics;
using System.IO;

namespace Tests.TestUtils
{
    public class LogExecutionTimer
    {
        public void LogExecutionTime(string executingFunction, Action function)
        {
            var startTime = DateTime.Now.ToString("[yyyy-mm-dd hh-mm-ss]");
            using var sr = File.CreateText(Path.Combine("Assets/", "Logs/",
                $"{startTime} Execution time of {executingFunction}.txt"));
            // path contains illegal chars


            var stopWatch = new Stopwatch();
            stopWatch.Start();
            function();
            stopWatch.Stop();

            sr.Write(
                $"Execution time of {executingFunction}: \n      {stopWatch.ElapsedMilliseconds} ms \n      {stopWatch.ElapsedMilliseconds / 1000} s");
        }
    }
}