// See https://aka.ms/new-console-template for more information
// would be great to have the helper functionality in other namespaces
using conlib;
using pathgen;

namespace Program;

class MainClass{
    public static void Main(string[] args){
        TestGetParamDict();
        TestPrependLines();
        TestReplaceParams();
        TestFilePathGen();
    }

    public static void TestGetParamDict(){
        string testInput = ":param a: a\n:param b: b\nc\n:param c:";
        Dictionary<string, string> expectedResult = new Dictionary<string, string>
        {
            {
                ":param a:",
                ":param a: a\n"
            },
            {
                ":param b:",
                ":param b: b\nc\n"
            },
            {
                ":param c:",
                ":param c:"
            }
        };
        // it passes, but such equality check does not work
        // i have to check if the stuff equals in count and then i have
        // to iterate over keys of one to verify key values of other, where if
        // it throws some keyerror, i can say it did not work
        Dictionary<string, string> result = ParamLoader.GetParamDict(testInput);
        if (!(result.Count == expectedResult.Count)) {
            Console.WriteLine("Result:");
            foreach (var kv in result) Console.WriteLine(
                $"{kv.Key}: {kv.Value}"
            );
            Console.WriteLine("Expected result:");
            foreach (var kv in expectedResult) Console.WriteLine(
                $"{kv.Key}: {kv.Value}"
            );
            throw new Exception("Expected result did not match the result.");
        }
    }

    public static void TestPrependLines(){
        // again here would be really gool some args tuple
        // this cant be done unless i name the tuple items
        string testIpnutText = "\na\n\n";
        string testInputPrepend = " ";
        string expectedResult = " \n a\n \n";
        var result = ParamReplacer.PrependLines(
            testIpnutText,
            testInputPrepend
        );
        if (!(result == expectedResult)) throw new Exception("Expected result did not match the result.");

    }

    public static void TestReplaceParams(){
        string testIpnutSourceText = "\na:\n\n  :param a:\n:param b: \n";
        Dictionary<string, string> testInputParamsDict = new Dictionary<string, string>{
            {
                ":param a:",
                ":param a: a\n"
            },
            {
                ":param b:",
                ":param b: b\nc\n"
            }
        };
        string expectedResult = "\na:\n\n  :param a: a\n:param b: b\nc\n";
        string result = ParamReplacer.ReplaceParams(
            testIpnutSourceText,
            testInputParamsDict
        );
        if (!(result == expectedResult)) throw new Exception("Expected result did not match the result.");

        testIpnutSourceText = ":param a:";
        testInputParamsDict = new Dictionary<string, string>{
            {
                ":param a:",
                ":param a: a\n"
            },
            {
                ":param b:",
                ":param b: b\nc\n"
            }
        };
        expectedResult = ":param a: a\n";
        result = ParamReplacer.ReplaceParams(
            testIpnutSourceText,
            testInputParamsDict
        );
        if (!(result == expectedResult)) throw new Exception("Expected result did not match the result.");
    }

    public static void TestFilePathGen() {
        FilePathGen g = new FilePathGen(@"C:\Users\adams\projects\namecon\namecon\manualtest\test_a");
        foreach (string p in g) Console.WriteLine($"filepath: {p}");
    }
}