using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using FailFastLibrary;

namespace FailFast
{
    class Program
    {
        public static List<string> FilePathsToTest { get; set; }
        public static Timer TimeToRunTests { get; set; }
        
        static void Main(string[] args)
        {
            FilePathsToTest = new List<string>();
            TimeToRunTests = new Timer();
            TimeToRunTests.Interval = 500;
            TimeToRunTests.Elapsed +=new ElapsedEventHandler(TimeToRunTests_Elapsed);
            var watcher = RunWatcher();
            Console.ReadLine();
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }

        private static void TimeToRunTests_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!FilePathsToTest.Any())
                return;
            var currentTestPaths = FilePathsToTest.Distinct().ToList();
            FilePathsToTest.Clear();
            Console.Clear();

            Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            foreach (var path in currentTestPaths)
            {
                RunTestsFromPath(path);
            }
            Console.WriteLine(Environment.NewLine +
                "Watching for additional changes. Press any key to quit.");
        }

        private static FileSystemWatcher RunWatcher()
        {
            var watcher = new FileSystemWatcher();
            watcher.Path =  Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            watcher.Filter = "*.dll";
            watcher.NotifyFilter = NotifyFilters.FileName |
                                   NotifyFilters.LastWrite;
            watcher.IncludeSubdirectories = true;
            watcher.Changed +=watcherFoundChange;
            watcher.InternalBufferSize = 5000;
            watcher.EnableRaisingEvents = true;
            
            Console.WriteLine("Waiting for changes.  Press any key to quit.");
            return watcher;
        }

        private static void watcherFoundChange(object sender, FileSystemEventArgs e)
        {
            FilePathsToTest.Add(e.FullPath);
            TimeToRunTests.Stop();
            TimeToRunTests.Start();
        }
        public static void RunTestsFromPath(string path)
        {

            var stopWatch = new System.Diagnostics.Stopwatch();

            stopWatch.Start();
            var types = FailFastRunner.FindTestClassesFromPath(path);
            stopWatch.Stop();
            if (!types.Any())
                return;

            Console.WriteLine("--------------------");
            Console.WriteLine(Path.GetFileName(path));
            runTests(types.Select(t => Activator.CreateInstance(t) as FailFastClass));
        }

        public static void runTests(IEnumerable<FailFastClass> testClasses)
        {
            var stopWatch = new System.Diagnostics.Stopwatch();

            stopWatch.Start();
            var runResult = FailFastRunner.RunTests(testClasses);
            stopWatch.Stop();
            Console.WriteLine(string.Format("Test Run Time: {0} seconds", stopWatch.ElapsedMilliseconds / 1000m));
            Console.WriteLine(runResult.RunMessage);
        }

    }
}
