////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: DumperTest.cs
//FileType: Visual C# Source file
//Author : Nickolai Aleksandrov
//Description : Unit testing for Dumper class
////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartDumper.Framework.Test
{
    using SmartSolutions.SmartDumper;
    using System.Collections.Generic;
    using System.Text;

    [TestClass]
    public class DumperTest
    {
        [TestMethod]
        public void DumpInt()
        {
            const int value = 5;
            StringBuilder result = Dumper.Dump("testInt", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "testInt: 5";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpNull()
        {
            StringBuilder result = Dumper.Dump("testNull", null, 0);
            string strResult = result.ToString();
            const string expectedResult = "testNull: null";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpIntWithIdent()
        {
            const int value = 5;
            StringBuilder result = Dumper.Dump("testInt", value, 4);
            string strResult = result.ToString();
            const string expectedResult = "    testInt: 5";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpDateTime()
        {
            DateTime value = new DateTime(2012, 12, 05, 10, 11, 13, 50);
            StringBuilder result = Dumper.Dump("testDateTime", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "testDateTime: 5/12/2012 10:11:13 AM";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }


        [TestMethod]
        public void DumpDecimal()
        {
            const decimal value = 50;
            StringBuilder result = Dumper.Dump("testDecimal", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "testDecimal: 50";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpBool()
        {
            bool value = true;
            StringBuilder result = Dumper.Dump("testBool", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "testBool: True";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));

            value = false;
            StringBuilder result2 = Dumper.Dump("testBool", value, 0);
            string strResult2 = result2.ToString();
            const string expectedResult2 = "testBool: False";
            Assert.IsTrue(strResult2 == expectedResult2, string.Format("result == \"{0}\"", expectedResult2));
        }

        [TestMethod]
        public void DumpException()
        {
            Exception value = new ArgumentException("Test exception message");
            StringBuilder result = Dumper.Dump("exception", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "exception: System.ArgumentException: Test exception message";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpClassWithException()
        {
            MyClassWithException value = new MyClassWithException { ID = 5 };
            StringBuilder result = Dumper.Dump("myClass", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "myClass: SmartDumper.Framework.Test.MyClassWithException [obj #1]\r\n  Properties {\r\n    ID: 5\r\n    Name: System.Exception: Internal exception in Name property\r\n  }\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }


        [TestMethod]
        public void DumpSimpleStruct()
        {
            MySimpleStruct value = new MySimpleStruct { Id = 5, Name = "Tester" };
            StringBuilder result = Dumper.Dump("mySimpleStruct", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "mySimpleStruct: SmartDumper.Framework.Test.MySimpleStruct [obj #1]\r\n  Fields {\r\n    Id: 5\r\n    Name: Tester\r\n  }\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpSimpleStructWithIdent()
        {
            MySimpleStruct value = new MySimpleStruct { Id = 5, Name = "Tester" };
            StringBuilder result = Dumper.Dump("mySimpleStruct", value, 5);
            string strResult = result.ToString();
            const string expectedResult = "     mySimpleStruct: SmartDumper.Framework.Test.MySimpleStruct [obj #1]\r\n       Fields {\r\n         Id: 5\r\n         Name: Tester\r\n       }\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpSimpleClass()
        {
            MySimpletClass value = new MySimpletClass { ID1 = 1, Name1 = "Tester1", ID2 = 2, Name2 = "Tester2" };
            StringBuilder result = Dumper.Dump("mySimpleClass", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "mySimpleClass: SmartDumper.Framework.Test.MySimpletClass [obj #1]\r\n  Properties {\r\n    ID1: 1\r\n    Name1: Tester1\r\n  }\r\n  Fields {\r\n    _id1: 1\r\n    _name1: Tester1\r\n    ID2: 2\r\n    Name2: Tester2\r\n  }\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpMyRecursiveClass()
        {
            MyRecursiveClass value1 = new MyRecursiveClass { ID = 1 };
            MyRecursiveClass value2 = new MyRecursiveClass { ID = 2 };
            MyRecursiveClass value3 = new MyRecursiveClass { ID = 3, Parent = value2 };
            MyRecursiveClass value4 = new MyRecursiveClass { ID = 4 };
            value2.Parent = value1;
            value1.Parent = value3;
            value4.Parent = value4;

            StringBuilder result = Dumper.Dump("mySimpleClass", value1, 0);
            string strResult = result.ToString();
            const string expectedResult = "mySimpleClass: SmartDumper.Framework.Test.MyRecursiveClass [obj #1]\r\n  Properties {\r\n    ID: 1\r\n    Parent: SmartDumper.Framework.Test.MyRecursiveClass [obj #2]\r\n      Properties {\r\n        ID: 3\r\n        Parent: SmartDumper.Framework.Test.MyRecursiveClass [obj #3]\r\n          Properties {\r\n            ID: 2\r\n            Parent: [see obj #1]\r\n          }\r\n      }\r\n  }\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpSelfMyRecursiveClass()
        {
            MyRecursiveClass value1 = new MyRecursiveClass { ID = 1 };
            value1.Parent = value1;

            StringBuilder result = Dumper.Dump("mySimpleClass", value1, 0);
            string strResult = result.ToString();
            const string expectedResult = "mySimpleClass: SmartDumper.Framework.Test.MyRecursiveClass [obj #1]\r\n  Properties {\r\n    ID: 1\r\n    Parent: [see obj #1]\r\n  }\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpList()
        {
            List<MyRecursiveClass> items = new List<MyRecursiveClass>
                {
                    new MyRecursiveClass {ID = 1},
                    new MyRecursiveClass {ID = 2},
                    new MyRecursiveClass {ID = 3},
                    new MyRecursiveClass {ID = 4}
                };

            StringBuilder result = Dumper.Dump("mySimpleClass", items, 0);
            string strResult = result.ToString();
            const string expectedResult = "mySimpleClass: System.Collections.Generic.List`1[[SmartDumper.Framework.Test.MyRecursiveClass, DumperTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]] [obj #1]\r\nIEnumerable[] {\r\n    item[0]: SmartDumper.Framework.Test.MyRecursiveClass [obj #1]\r\n      Properties {\r\n        ID: 1\r\n        Parent: null\r\n      }\r\n\r\n    item[1]: SmartDumper.Framework.Test.MyRecursiveClass [obj #2]\r\n      Properties {\r\n        ID: 2\r\n        Parent: null\r\n      }\r\n\r\n    item[2]: SmartDumper.Framework.Test.MyRecursiveClass [obj #3]\r\n      Properties {\r\n        ID: 3\r\n        Parent: null\r\n      }\r\n\r\n    item[3]: SmartDumper.Framework.Test.MyRecursiveClass [obj #4]\r\n      Properties {\r\n        ID: 4\r\n        Parent: null\r\n      }\r\n}\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpArray()
        {
            List<MyRecursiveClass> items = new List<MyRecursiveClass>
                {
                    new MyRecursiveClass {ID = 1},
                    new MyRecursiveClass {ID = 2},
                    new MyRecursiveClass {ID = 3},
                    new MyRecursiveClass {ID = 4}
                };

            StringBuilder result = Dumper.Dump("mySimpleClass", items.ToArray(), 0);
            string strResult = result.ToString();
            const string expectedResult = "mySimpleClass: SmartDumper.Framework.Test.MyRecursiveClass[] [obj #1]\r\n  Properties {\r\n    Length: 4\r\n    LongLength: 4\r\n    Rank: 1\r\n    SyncRoot: [see obj #1]\r\n    IsReadOnly: False\r\n    IsFixedSize: True\r\n    IsSynchronized: False\r\n  }\r\n  IEnumerable[] {\r\n    item[0]: SmartDumper.Framework.Test.MyRecursiveClass [obj #2]\r\n      Properties {\r\n        ID: 1\r\n        Parent: null\r\n      }\r\n\r\n    item[1]: SmartDumper.Framework.Test.MyRecursiveClass [obj #3]\r\n      Properties {\r\n        ID: 2\r\n        Parent: null\r\n      }\r\n\r\n    item[2]: SmartDumper.Framework.Test.MyRecursiveClass [obj #4]\r\n      Properties {\r\n        ID: 3\r\n        Parent: null\r\n      }\r\n\r\n    item[3]: SmartDumper.Framework.Test.MyRecursiveClass [obj #5]\r\n      Properties {\r\n        ID: 4\r\n        Parent: null\r\n      }\r\n  }\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        [TestMethod]
        public void DumpDictionary()
        {
            Dictionary<object, object> items = new Dictionary<object, object>();
            items.Add(1, new MyRecursiveClass { ID = 11111 });
            items.Add(new MyClassWithException { ID = 1234567890 }, new MyRecursiveClass { ID = 22222 });
            items.Add(3, new MyClassWithException { ID = 22222 });
            items.Add(4, null);
            StringBuilder result = Dumper.Dump("myDictionary", items, 0);
            string strResult = result.ToString();
            const string expectedResult = "myDictionary: System.Collections.Generic.Dictionary`2[[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] [obj #1]\r\nIDictionary[4] {[\r\n    Key: 1\r\n    Value: SmartDumper.Framework.Test.MyRecursiveClass [obj #1]\r\n      Properties {\r\n        ID: 11111\r\n        Parent: null\r\n      }\r\n]\r\n[\r\n    Key: SmartDumper.Framework.Test.MyClassWithException [obj #2]\r\n      Properties {\r\n        ID: 1234567890\r\n        Name: System.Exception: Internal exception in Name property\r\n      }\r\n    Value: SmartDumper.Framework.Test.MyRecursiveClass [obj #3]\r\n      Properties {\r\n        ID: 22222\r\n        Parent: null\r\n      }\r\n]\r\n[\r\n    Key: 3\r\n    Value: SmartDumper.Framework.Test.MyClassWithException [obj #4]\r\n      Properties {\r\n        ID: 22222\r\n        Name: System.Exception: Internal exception in Name property\r\n      }\r\n]\r\n[\r\n    Key: 4\r\n    Value: null\r\n]\r\n}\r\n";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }

        private enum TestEnum1
        {
            One,
            Two,
            Three
        };

        [TestMethod]
        public void DumpEnum()
        {
            const TestEnum1 value = TestEnum1.Two;
            StringBuilder result = Dumper.Dump("testEnum1", value, 0);
            string strResult = result.ToString();
            const string expectedResult = "testEnum1: Two";
            Assert.IsTrue(strResult == expectedResult, string.Format("result == \"{0}\"", expectedResult));
        }
    }

    public struct MySimpleStruct
    {
        public int Id;
        public string Name;
    }

    public class MySimpletClass
    {
        public int _id1;

        public int ID1
        {
            get { return _id1; }
            set { _id1 = value; }
        }

        public string _name1;

        public string Name1
        {
            get { return _name1; }
            set { _name1 = value; }
        }

        public int ID2;
        public string Name2;
    }

    public class MyRecursiveClass
    {
        public int ID { get; set; }
        public MyRecursiveClass Parent { get; set; }
    }

    public class MyClassWithException
    {
        public int ID { get; set; }
        public int Name
        {
            get { throw new Exception("Internal exception in Name property"); }
        }
    }
}

