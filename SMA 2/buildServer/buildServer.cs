/////////////////////////////////////////////////////////////////////////////////
// buildRequest.cs: Demonstrates a few buildRequest operations                 //
// ver 1.0                                                                     //
//                                                                             //
// Platform     : Dell Inspiron, Windows 10 Pro x64, Visual Studio 2015        //
// Application  : CSE-681 - MockClient Demonstration                           //
// Author       : Sarath Patlolla, EECS Department, Syracuse University        //
//                (313)-728-8587, spatloll@syr.edu                             //                            
/////////////////////////////////////////////////////////////////////////////////

/* Based on build requests and code sent from the Repository, the Build Server builds test 
 * libraries for submission to the Test Harness. On completion, if successful,
 *  the build server submits test libraries and test requests to the Test Harness, 
 *  and sends build logs to the Repository.
 *  The test request is then generated in the build server and along with the test 
 *  request the dll files are sent to the test harness for testing of the build files
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Federation;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Utilities;
using buildRequest;
using testRequest;



namespace Federation
{

    public class buildServer
    {
        RepoMock repo = null;
        public string xml = null;
        public List<string> files { get; set; } = new List<string>();
       

        Dictionary<string, string> dictionary = new Dictionary<string, string>();


        //-----------------------<Function to obtain the (relative) path of the files  to build in the repo>---------------------------------
        public List<string> getBuilderFiles()
        {
            List<string> file = new List<string>();
            string[] files = Directory.GetFiles("../../../buildServer/builderStorage", "*.cs");
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFullPath(files[i]);
            }
            file.AddRange(files);
            return file;
        }



        //---------------------------<Function to perform a build operation on the files that have been obtained from the repository>---------------------------------
        public void buildFile(List<string> Files, Dictionary<string, string> dict)
        {

            foreach (string file in Files)
            {
                Process p = new Process();
                

                // Replacing the cs extension with the dll after the build is succesfull

                string dll = file;
                dll = dll.Replace(".cs", ".dll");
                try
                {
                    string temp = null;
                    if (file.Contains("Driver"))
                        temp = file + "   " + dict[file];

                    else
                        temp = file;

                    var frameworkPath = RuntimeEnvironment.GetRuntimeDirectory();
                    var cscPath = Path.Combine(frameworkPath, "csc.exe");
                    // specifies the start of the command
                    p.StartInfo.FileName = cscPath;
                    p.StartInfo.Arguments = "/target:library " + temp;


                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.WorkingDirectory = "../../../buildServer/builderStorage";
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.Start();
                   string output = p.StandardOutput.ReadToEnd();
                    string error = p.StandardError.ReadToEnd();
                    if (output != null)
                    {
                        string[] tempFiles = Directory.GetFiles("../../../buildServer/builderStorage/", "*.dll");
                        for (int i = 0; i < tempFiles.Length; ++i)
                        {
                            tempFiles[i] = Path.GetFileName(tempFiles[i]);
                        }
                        if (tempFiles.Contains(dll))
                        {
                            Console.WriteLine("\n");
                            Console.WriteLine("BUILD SUCCESSFULL : " + temp);
                            Console.WriteLine("\n");
                        }
                        else
                        {
                            Console.WriteLine("BUILD NOT SUCCESSFULL:" + temp);
                            Console.WriteLine(output);
                        }
                    }
                    //if (error != null)
                    //{
                    //    Console.WriteLine(error);
                    //}
                    //p.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        /*----< private helper function for RepoMock.getFiles >--------*/

        private void getFilesHelper(string path, string pattern)
        {
            string[] tempFiles = Directory.GetFiles(path, pattern);
            for (int i = 0; i < tempFiles.Length; ++i)
            {
                tempFiles[i] = Path.GetFullPath(tempFiles[i]);
            }
            files.AddRange(tempFiles);

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                getFilesHelper(dir, pattern);
            }
        }

        //----------------------------------------------<Function to get the associated Dll files in the build server>-----------------------
        //public void getFiles(string pattern)
        //{
        //    files.Clear();
        //    getFilesHelper(storagePath, pattern);
        //}

        //---------------------------------------<Function to process the build request>-------------------------------------

        public Dictionary<string,string> processBuild(string path,RepoMock rm)
        {
            Console.WriteLine("processing build request");
            Console.WriteLine("-------------------------------------");
            
            string xmlstring = File.ReadAllText(path);
            buildRequest.buildRequest newRequest = xmlstring.FromXml<buildRequest.buildRequest>();

            repo = rm;

            foreach (buildItems item in newRequest.Builds)
            {
                string s1 = "";

                List<string> l1 = new List<string>();

                foreach (buildName b1 in item.driver)
                {
                    l1.Add(b1.fileName);
                    Console.WriteLine("\nrequesting" + b1.fileName);
                    repo.processfilerequest(b1.fileName);
                }

                foreach (buildName c in item.sourcefiles)
                {
                    s1 = s1 + c.fileName + "  ";
                    Console.WriteLine("\nrequesting" + c.fileName);
                    repo.processfilerequest(c.fileName);

                }

                foreach (string str in l1)
                {
                    dictionary.Add(str, s1);
                }

            }

            return dictionary;

        }

        //--------------------------------------<function to process the dllrequest for the given file name>------------------
        public void processdtestrequest(string name)
        {
            string destSpec = Path.Combine("../../../buildServer/builderStorage", name);
            try
            {
                string file = Path.GetFileName(destSpec);
                string sendLoc = Path.Combine("../../../DllLoaderDemo/testHarnessStorage",file);
                File.Copy(destSpec, sendLoc, true);
                Console.WriteLine("File sent to test harness");
                
            }
            catch(Exception e)
            {
                Console.WriteLine("{0}", e.Message);
            }          
        }
        //----------------------<Functionto parse tehe test request for the test harness>-------------------------------
        public void parseTestRequest(TestRequest tr, List<string> names)
        {
            List<string> file = new List<string>();
            foreach (string s in names)
            {
                string fileName = Path.GetFileName(s);
                file.Add(fileName);
            }
            names = file;
            Console.WriteLine("\n\nParsing test Message");


            foreach (TestElement item in tr.tests)
            {
                foreach (TestName b in item.testDriver)
                {
                    if (names.Contains(b.testName))
                    {
                        Console.WriteLine("test driver found" + b.testName);
                    }
                    else
                        Console.WriteLine("Test Driver not found");
                }
                foreach (TestName b in item.testCodes)
                {
                    if (names.Contains(b.testName))
                    {
                        Console.WriteLine("source driver found" + b.testName); ;
                    }
                    else
                        Console.WriteLine("Source Driver not found");

                }
            }
        }

        //-----------------------------<Function to process the directory contets in a file>-------------------------------------
        public List<string> processdirectory(List<string> file)
        {
            List<string> files = new List<string>();

            foreach (string f in file)
            {
                string fileName = Path.GetFileName(f);
                files.Add(fileName);
            }
            return files;
        }

        //-------------------------------------<Function to generate a test Request>--------------------------------------
        public TestRequest generateTestRequest()

        {
            TestElement te1 = new TestElement();
            TestName t1 = new TestName();
            t1.testName = "TestDriver.dll";
            te1.testName = "Test1";
            te1.addDriver(t1);
            TestName t2 = new TestName();
            TestName t3 = new TestName();
            t2.testName = "TestedOne.dll";
            t3.testName = "TestedTwo.dll";
            te1.addCode(t2);
            te1.addCode(t3);


            TestRequest tr = new TestRequest();
            tr.author = "Sarath Patlolla";
            tr.tests.Add(te1);

             xml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", xml);

            

            //Console.Write("\n  deserializing xml string results in type: {0}\n", typeName);
            return tr;

        }
     
    }

    //-----------------------------------------------<Test Stub>-------------------------------------------------
#if (test_buildServer)
    public class testBuildServer
    {
        public static void Main(string[] srgs)
        {

        }


    }
#endif


    }
