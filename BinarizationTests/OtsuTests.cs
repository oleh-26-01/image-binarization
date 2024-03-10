using CSharp.Algorithms.Imperative;
using ImageProcessing;
using System.Runtime.Versioning;
using CSharp;
using CSharp.Algorithms;

namespace ImageProcessingTests;

[TestClass, SupportedOSPlatform("windows")]
public class OtsuTests
{
    private static readonly int _length = 1024 * 1024;
    private readonly byte[] _input = Helper.GenerateArray(_length);
    private readonly byte[] _correct = new byte[_length];

    [TestInitialize]
    public void Initialize()
    {
        IBinarizationAlgorithm algorithm = new Otsu();
        Array.Copy(_input, _correct, _length);
        algorithm.Binarize(_correct);
    }

    [TestMethod]
    public void TestDeclarative()
    {
        var testResult = new byte[_length];
        Array.Copy(_input, testResult, _length);
        var algorithm = new CSharp.Algorithms.Declarative.Otsu();
        algorithm.Binarize(testResult);
        CollectionAssert.AreEqual(_correct, testResult);
    }

    [TestMethod]
    public void TestFunctional()
    {
        var testResult = new byte[_length];
        Array.Copy(_input, testResult, _length);
        testResult = Functional.BinarizeOtsu(testResult);
        CollectionAssert.AreEqual(_correct, testResult);
    }

    [TestMethod]
    public void TestOOP()
    {
        var testResult = new byte[_length];
        Array.Copy(_input, testResult, _length);
        var algorithm = new CSharp.Algorithms.OOP.Otsu();
        algorithm.Binarize(testResult);
        CollectionAssert.AreEqual(_correct, testResult);
    }
}