using ImageProcessing;
using System.Runtime.Versioning;

namespace ImageProcessingTests;

[TestClass, SupportedOSPlatform("windows")]
public class OtsuTests
{
    private static readonly int _length = 1024 * 1024;

    [TestMethod]
    public void TestDeclarative()
    {
        var input = GenerateArray(_length);
        var correct = new byte[_length];
        var output = new byte[_length];
        Array.Copy(input, correct, _length);
        Array.Copy(input, output, _length);
        Imperative.BinarizeOtsu(correct);
        Declarative.BinarizeOtsu(output);
        CollectionAssert.AreEqual(correct, output);
    }

    [TestMethod]
    public void TestFunctional()
    {
        var input = GenerateArray(_length);
        var correct = new byte[_length];
        var output = new byte[_length];
        Array.Copy(input, correct, _length);
        Array.Copy(input, output, _length);
        Imperative.BinarizeOtsu(correct);
        output = Functional.BinarizeOtsu(output);
        CollectionAssert.AreEqual(correct, output);
    }

    [TestMethod]
    public void TestOOP()
    {
        var input = GenerateArray(_length);
        var correct = new byte[_length];
        var output = new byte[_length];
        Array.Copy(input, correct, _length);
        Array.Copy(input, output, _length);
        Imperative.BinarizeOtsu(correct);
        var oopInstance = new OOP(output);
        oopInstance.BinarizeOtsu();
        CollectionAssert.AreEqual(correct, output);
    }

    private static byte[] GenerateArray(int length)
    {
        var array = new byte[length];
        var random = new Random();
        for (var i = 0; i < length; i++)
        {
            array[i] = (byte)random.Next(0, 256);
        }
        return array;
    }
}