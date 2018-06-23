﻿/////////////////////////////////////////////////////////////////////////////////
// buildRequest.cs: Demonstrates a few buildRequest operations                 //
// ver 1.0                                                                     //
//                                                                             //
// Platform     : Dell Inspiron, Windows 10 Pro x64, Visual Studio 2015        //
// Application  : CSE-681 - MockClient Demonstration                           //
// Author       : Sarath Patlolla, EECS Department, Syracuse University        //
//                (313)-728-8587, spatloll@syr.edu                             //
// Source       : Dr Fawcett Test Request Package                              //
/////////////////////////////////////////////////////////////////////////////////

/* The buildRequest package is to devlop a buildRequest which consists of some .cs 
 * files which would be send to the build server to build. and convert themto dll The build request would 
 * be generated in the client and then stored in the repository. From there it would be
 *  sent on 
 * demand to the build server. The build request is created by the user and parsed to the 
 * repository
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utilities;

namespace buildRequest
{

    public class buildName
    {
        public string fileName { get; set; }
        public string driverName { get; set; }
    }

    /*The buildItems class consist of all the different buildItems for the Xml which we would be creating
     */

    public class buildItems
    {

       public string buildName { get; set; }
        public List<buildName> driver { get; set; } = new List<buildName>();

        public List<buildName> sourcefiles { get; set; } = new List<buildName>();

        public buildItems() { }
        public buildItems(string name)
        {
            buildName = name;
        }

        public void addDriver(buildName fileName)
        {
            driver.Add(fileName);
        }
        public void addCode(buildName fileName)
        {
            sourcefiles.Add(fileName);
        }
       
    }

    ///////////////////////////////////////////////////////////////////
    //Build Request class
    public class buildRequest
    {
        public string author { get; set; }
        public List<buildItems> Builds { get; set; } = new List<buildItems>();

        public buildRequest() { }
        public buildRequest(string auth)
        {
            author = auth;
        }
       


    }


    //<------------------------------------------<TEST STUB>----------------------------------------------------->

#if(testbuildRequest)
    class testbuildRequest
    {

        static void Main(string[] args)
        {


            //-------------------<First build Driver>---------------------- 
            buildItems be1 = new buildItems();
            buildName b1 = new buildName();
            b1.fileName = "TestDriver.cs";
            be1.buildName = "build1";
          //  be1.buildName = "td1.cs";
            be1.addDriver(b1);
            buildName b2 = new buildName();
            buildName b3 = new buildName();
            b2.fileName = "TestedOne.cs";
            b3.fileName = "TestedTwo.cs";
            be1.addCode(b2);
            be1.addCode(b3);


            

            //-----------------<Build request>---------------------

            buildRequest br = new buildRequest();
            br.author = "Sarath Patlolla";
            br.Builds.Add(be1);
           
            string Xml = br.ToXml();
            Console.WriteLine("\nThe build Request Generated by the Client is {0}", Xml);


        }



    }
#endif


}
