# SmartDumper
This projects has class Dumper which can dump any reference type (int, float, etc) and value type (object, string, etc).

There are some examples how the Dumper works:

==============================
Object with reference to another object is logged as the following:

MyRecursiveClass value1 = new MyRecursiveClass { ID = 1 };
MyRecursiveClass value2 = new MyRecursiveClass { ID = 2 };
MyRecursiveClass value3 = new MyRecursiveClass { ID = 3, Parent = value2 };
MyRecursiveClass value4 = new MyRecursiveClass { ID = 3 };
value2.Parent = value1;
value1.Parent = value3;
value4.Parent = value4;

mySimpleClass: SmartDumper.Framework.Test.MyRecursiveClass [obj #1]
  Properties {
    ID: 1
    Parent: SmartDumper.Framework.Test.MyRecursiveClass [obj #2]
      Properties {
        ID: 3
        Parent: SmartDumper.Framework.Test.MyRecursiveClass [obj #3]
          Properties {
            ID: 2
            Parent: [see obj #1]
          }
      }
  }
  
Information about value4 object is not dumped because it is not in the object hierarchy.

==============================

Object that reference to itsleft is dumped as the following:

MyRecursiveClass value1 = new MyRecursiveClass { ID = 1 };
value1.Parent = value1;

mySimpleClass: SmartDumper.Framework.Test.MyRecursiveClass [obj #1]
  Properties {
    ID: 1
    Parent: [see obj #1]
  }
==============================
The Dumper even dumps information about exception thrown when it access some properties. The result may look like the following:

myDictionary: System.Collections.Generic.Dictionary`2[[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] [obj #1]
IDictionary[4] {[
    Key: 1
    Value: SmartDumper.Framework.Test.MyRecursiveClass [obj #1]
      Properties {
        ID: 11111
        Parent: null
      }
]
[
    Key: SmartDumper.Framework.Test.MyClassWithException [obj #2]
      Properties {
        ID: 1234567890
        Name: System.Exception: Internal exception in Name property
      }
    Value: SmartDumper.Framework.Test.MyRecursiveClass [obj #3]
      Properties {
        ID: 22222
        Parent: null
      }
]
[
    Key: 3
    Value: SmartDumper.Framework.Test.MyClassWithException [obj #4]
      Properties {
        ID: 22222
        Name: System.Exception: Internal exception in Name property
      }
]
[
    Key: 4
    Value: null
]
}

