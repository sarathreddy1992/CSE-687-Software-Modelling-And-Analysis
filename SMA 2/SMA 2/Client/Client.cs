﻿/////////////////////////////////////////////////////////////////////////////////
// Client.cs: Demonstrates a few MockClient operations                     //
// ver 1.0                                                                     //
//                                                                             //
// Platform     : Dell Inspiron, Windows 10 Pro x64, Visual Studio 2015        //
// Application  : CSE-681 - MockClient Demonstration                           //
// Author       : Sarath Patlolla, EECS Department, Syracuse University        //
//                (313)-728-8587, spatloll@syr.edu                             //
/////////////////////////////////////////////////////////////////////////////////

/* Package operations
 * 
 * This package demonstrates a few operations performed by a Mock Client 
 *  The main functionality of the mockClient is to generate a build request and send it to
 *  the repo
 *  The client has no functions to perform in the build server.
 */


using buildRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Client
{
    public class Client
    {
        public string Xml = null;
        buildRequest.buildRequest br = new buildRequest.buildRequest();
        public buildRequest.buildRequest buildrequest()
        {

            //-------------------<First build Driver>---------------------- 
            buildItems be1 = new buildItems();
            buildName b1 = new buildName();
            b1.fileName = "TestDriver.cs";
           // b1.buildName = "build1";
              be1.buildName = "build1";
            be1.addDriver(b1);
            buildName b2 = new buildName();
            buildName b3 = new buildName();
            b2.fileName = "TestedOne.cs";
            b3.fileName = "TestedTwo.cs";
            be1.addCode(b2);
            be1.addCode(b3);




            //-----------------<Build request>---------------------

           // buildRequest.buildRequest br = new buildRequest.buildRequest();
            br.author = "Sarath Patlolla";
            br.Builds.Add(be1);
          
            Xml = br.ToXml();
            Console.WriteLine("\nThe build Request Generated by the Client is {0}", Xml);
            return br;




        }
    }



    //------------------------------------<Test Stub>----------------------------------------------------
#if (testClient)

    class testMockClient
    {
    static void Main(string[] args)

    {
            Console.WriteLine("\nBuild Request generated by client");
            Client Client = new Client();
            Client.buildrequest();
           
    }
    }
#endif
}



