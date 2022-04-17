using System;
using System.Diagnostics;
using System.IO;

namespace Tests.UnitTests.TestUtils
{
    public class LogExecutionTimer
    {
        public void LogExecutionTime(string executingFunction, Action function)
        {
            var startTime = DateTime.Now.ToString("[yyyy-mm-dd hh-mm-ss]");
            using var sr = File.CreateText(Path.Combine("Assets/", "Logs/",
                $"{startTime} Execution time of {executingFunction}.txt"));

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            function();
            stopWatch.Stop();
            if (stopWatch.ElapsedMilliseconds != 0)
                sr.Write(
                    $"Execution time of {executingFunction}: \n      {stopWatch.ElapsedMilliseconds.ToString()} ms \n      {(stopWatch.ElapsedMilliseconds / 1000).ToString()} s \n      {(stopWatch.ElapsedMilliseconds / 1000 / 60).ToString()} min");
        }
    }
}