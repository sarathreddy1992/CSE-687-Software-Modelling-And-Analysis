/////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Title:: Prototype to demonstarate a build server                                                   //
//  Author:: Sarath Patlolla                                                                           //
//           (CSE 681, spatloll@syr.edu)                                                               //
//   Platform :: Dell inspiron                                                                         //
//   version :: v.1.0                                                                                  //
//   Date::  09/12/17                                                                                  //                                                                                                  //
//                                                                                                     //
//                                                                                                     //
//                                                                                                     //
//////////////////////////////////////////////////////////////////////////////////////////////////////////







using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Execution;
using System.IO;

namespace BuildServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Build server Prototype");
            Console.WriteLine("**********************************************");
            string projectFile = @"../../../../testFile/testFile/testFile.csproj";
            
            string logFile = @"../../../../testFile/testFile/testFile.text";
            try
            {
                Console.WriteLine("Building test file");
            Console.WriteLine("{0}", projectFile);
           
                FileLogger fileLogger = new FileLogger();
                fileLogger.Parameters = @"logFile=" + logFile;
                Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();
                BuildRequestData BuildRequest = new BuildRequestData(projectFile, GlobalProperty, null, new string[] { "Build" }, null);
                BuildParameters bp = new BuildParameters();
                bp.Loggers = new List<Microsoft.Build.Framework.ILogger> { fileLogger }.AsEnumerable();
                BuildResult buildResult = BuildManager.DefaultBuildManager.Build(bp, BuildRequest);

                Console.WriteLine("\nBuild Successful\n\n");
                string text = File.ReadAllText(logFile);
                Console.WriteLine("\nBuild Messages:\n{0}", text);
                Console.WriteLine("\nPlease check log:");
                Console.WriteLine("\t\t\t {0}", logFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException message:\n {0}\n\n", ex.Message);
                Environment.Exit(0);
            }
        }
      
    }
}