using System.Runtime.Versioning;
using CSharp;

namespace BinarizationTests;

[TestClass, SupportedOSPlatform("windows")]
public class SauvolaTests
{
    private static readonly int _length = 1024 * 1024;
    private static readonly int _width = 1024;
    private static readonly int _height = 1024;
    private static readonly float _allowed = 1e-5f;
    private readonly byte[] _input = Helper.GenerateArray(_length);
    private readonly byte[] _correct = new byte[_length];

    [TestInitialize]
    public void Initialize()
    {
        IBinarizationAlgorithm imperativeSauvola = new CSharp.Algorithms.Imperative.Sauvola();
        Array.Copy(_input, _correct, _length);
        imperativeSauvola.Binarize(_correct, _width, _height); 
    }

    [TestMethod]
    public void TestDeclarative()
    {
        var testResult = new byte[_length];
        Array.Copy(_input, testResult, _length);
        var algorithm = new CSharp.Algorithms.Declarative.Sauvola();
        algorithm.Binarize(testResult, _width, _height);
        CollectionAssert.AreEqual(_correct, testResult);
    }

    [TestMethod]
    public void TestFunctional()
    {
        var testResult = new byte[_length];
        Array.Copy(_input, testResult, _length);
        testResult = Functional.Sauvola.Binarize(testResult, _width, _height);
        Assert.IsTrue(DiffPercentage(_correct, testResult) <= _allowed);
    }

    public static double DiffPercentage(byte[] correct, byte[] testResult)
    {
        var diff = 0;
        for (var i = 0; i < correct.Length; i++)
        {
            if (correct[i] != testResult[i])
            {
                diff++;
            }
        }

        return (double)diff / correct.Length;
    }

    [TestMethod]
    public void TestOOP()
    {
        var testResult = new byte[_length];
        Array.Copy(_input, testResult, _length);
        var algorithm = new CSharp.Algorithms.OOP.Sauvola();
        algorithm.Binarize(testResult, _width, _height);
        CollectionAssert.AreEqual(_correct, testResult);
    }
}
