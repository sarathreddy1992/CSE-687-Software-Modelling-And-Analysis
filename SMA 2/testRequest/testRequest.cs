
/////////////////////////////////////////////////////////////////////////////////
// testRequest.cs: Demonstrates   testdRequest operations                      //
// ver 1.0                                                                     //
//                                                                             //
// Platform     : Dell Inspiron, Windows 10 Pro x64, Visual Studio 2015        //
// Application  : CSE-681 - MockClient Demonstration                           //
// Author       : Sarath Patlolla, EECS Department, Syracuse University        //
//                (313)-728-8587, spatloll@syr.edu                             //
// Source       : Dr Fawcett Test Request Package                              //
/////////////////////////////////////////////////////////////////////////////////

/*Package operations
 * ================================
 * Demonstrates serializing and deserializing complex data structures used
* in TestHarnes.
* 
* This demo serializes and deserializes TestRequest and TestResults instances.
* It then Creates and parses a TestRequest Message and a TestResults Message,
* retrieving copies of the original data structures.
* 
* The purpose of this demo is to show that using a single message class with
* an XML body
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utilities;


namespace testRequest
{
    public class TestName
    {
        public string testName { get; set; }
    }

    public class TestElement  /* information about a single test */
    {
        public string testName { get; set; }
        public List<TestName> testDriver { get; set; } = new List<TestName>();
        public List<TestName> testCodes { get; set; } = new List<TestName>();

        public TestElement() { }
        public TestElement(string name)
        {
            testName = name;
        }
        public void addDriver(TestName name)
        {
            testDriver.Add(name);
        }
        public void addCode(TestName name)
        {
            testCodes.Add(name);
        }
    }

        ///////////////////////////////////////////////////////////////////
        //Build Request class
        public class TestRequest  /* a container for one or more TestElements */
        {
            public string author { get; set; }
            public List<TestElement> tests { get; set; } = new List<TestElement>();

            public TestRequest() { }
            public TestRequest(string auth)
            {
                author = auth;
            }
        }
            public class TestResult  /* information about processing of a single test */
            {
                public string testName { get; set; }
                public bool passed { get; set; }
                public string log { get; set; }

                public TestResult() { }
                public TestResult(string name, bool status)
                {
                    testName = name;
                    passed = status;
                }
                //<--------used for logging logitem----------->
                public void addLog(string logItem)
                {
                    log += logItem;
                }
            }

                //<------------------------------------------<TEST STUB>----------------------------------------------------->

#if (TEST_THMESSAGES)

  class TestTHMessages
  {
    static void Main(string[] args)
    {
      //"Testing THMessage Class".title('=');
      Console.WriteLine();

      ///////////////////////////////////////////////////////////////
      // Serialize and Deserialize TestRequest data structure

      //"Testing Serialization of TestRequest data structure".title();

      TestElement te1 = new TestElement();
      TestName t1 = new TestName();
            t1.testName = "TestDriver.dll";
            te1.testName = "Test1";
            te1.addDriver(t1);
            TestName t2 = new TestName();
            TestName t3 = new TestName();
            t2.testName = "TestedOne.dll";
            t2.testName = "TestedTwo.dll";
            te1.addCode(t2);
            te1.addCode(t3);
           
     
      TestRequest tr = new TestRequest();
      tr.author = "Sarath Patlolla";
      tr.tests.Add(te1);
      
      string trXml = tr.ToXml();
      Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);

      "Testing Deserialization of TestRequest from XML".title();

      TestRequest newRequest = trXml.FromXml<TestRequest>();
      string typeName = newRequest.GetType().Name;
           
      Console.Write("\n  deserializing xml string results in type: {0}\n", typeName);
      Console.Write(newRequest);
      Console.WriteLine();

    }
  }
#endif
}

