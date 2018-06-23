/////////////////////////////////////////////////////////////////////
// RepoMock.cs - Demonstrate a few mock repo operations            //
//                                                                 //
// Author: Jim Fawcett, CST 4-187, jfawcett@twcny.rr.com           // 
// Co Author: Sarath Patlolla, EECS department, Syracuse University//
//(315)-728-8587, spatloll@syr.edu                                 //
// Application: Project Template                                   //
// Environment: C# console                                         //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * ===================
 * TBD
 * 
 * Required Files:
 * ---------------
 * TBD
 * 
 * Maintenance History:
 * --------------------
 * ver 1.0 : 07 Sep 2017
 * - first release
 * 
 * Functionalities added
 * ------------------------------------------
 * added  save file process request  and parsing to build server
 *  functionality to the repository
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Utilities;
using buildRequest;


namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // RepoMock class
    // - begins to simulate basic Repo operations

    public class RepoMock
    {
        public string storagePath { get; set; } = "../../../repoMock/repoStorage";
        public string receivePath { get; set; } = "../../../buildServer/builderStorage";
        public List<string> files { get; set; } = new List<string>();

        /*----< initialize RepoMock Storage>---------------------------*/

        public RepoMock()
        {
            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);
            if (!Directory.Exists(receivePath))
                Directory.CreateDirectory(receivePath);
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
        /*----< find all the files in RepoMock.storagePath >-----------*/
        /*
        *  Finds all the files, matching pattern, in the entire 
        *  directory tree rooted at repo.storagePath.
        */
        public void getFiles(string pattern)
        {
            files.Clear();
            getFilesHelper(storagePath, pattern);
        }
        /*---< copy file to RepoMock.receivePath >---------------------*/
        /*
        *  Will overwrite file if it exists. 
        */
        public bool sendFile(string fileSpec)
        {
            try
            {
                string fileName = Path.GetFileName(fileSpec);
                string destSpec = Path.Combine(receivePath, fileName);
                File.Copy(fileSpec, destSpec, true);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("\n--{0}--", ex.Message);
                return false;
            }
        }

        //--------------------<Function to save the file in the Repo >------------

        public void savecontent( string path, string xmlcontent)
        {
            File.WriteAllText(path, xmlcontent);
         //   Console.Write("\n\n contents saved to the mock repository\n\n ");

        }

        //----------------------<Function to process directory>----------------------
        public List<string> processdircontents(List<string> direc)
        {
            List<string> file = new List<string>();

            foreach (string f in direc)
            {
                string fileName = Path.GetFileName(f);
                file.Add(fileName);
            }
            return file;
        }

        //-----------------------<function to send the repo directory to the build server on command>----------------

        public void sendFile(string command, string filepath, string recievepath)
        {
            if (command == "send" || command == "Send" || command == "SEND")
            {
                receivePath = recievepath;
                if (sendFile(filepath))
                {
                    Console.WriteLine("\n\n File sent to the build server");
                }
            }
        }


        //----------------------<Function to process the file request>-------------------
        public void processfilerequest(string s)
        {
            string fileReq = Path.Combine(storagePath, s);
            sendFile(fileReq);
            Console.Write("file sent to build server");
            Console.Write("\n--------------------------------------");

        }
        //------------------<Function to parse the build request>---------------------------------

        public void parseRequest(buildRequest.buildRequest br, List<string> names)
        {
            List<string> file = new List<string>();
            foreach (string s in names)
            {
                string fileName = Path.GetFileName(s);
                file.Add(fileName);
            }
            names = file;
            Console.WriteLine("\n\nParsing build Message");


            foreach (buildItems item in br.Builds)
            {
                foreach (buildName b in item.driver)
                {
                    if (names.Contains(b.fileName))
                    {
                        Console.WriteLine("test driver found" +b.fileName);
                    }
                    else
                        Console.WriteLine("Test Driver not found");
                }
                foreach (buildName b in item.sourcefiles)
                {
                    if (names.Contains(b.fileName))
                    {
                        Console.WriteLine("source driver found" + b.fileName); ;
                    }
                    else
                        Console.WriteLine("Source Driver not found");

                }
            }
        }






#if (TEST_REPOMOCK)

        ///////////////////////////////////////////////////////////////////
        // TestRepoMock class

        class TestRepoMock
        {
            static void Main(string[] args)
            {
                Console.Write("\n\n Mock Repo and its functionality");
                RepoMock repo = new RepoMock();
                repo.getFiles("*.*");
                foreach (string file in repo.files)
                    Console.Write("\n  \"{0}\"", file);

                string fileSpec = repo.files[1];
                string fileName = Path.GetFileName(fileSpec);
                Console.Write("\n  sending \"{0}\" to \"{1}\"", fileName, repo.receivePath);
                repo.sendFile(repo.files[1]);

                Console.Write("\n\n");
            }
        }
    }
#endif
    }

