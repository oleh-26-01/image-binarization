using System.Runtime.Versioning;
using CSharp;

namespace BinarizationTests;

[TestClass, SupportedOSPlatform("windows")]
public class OtsuTests
{
    private static readonly int _length = 1024 * 1024;
    private static readonly float _allowed = 1e-5f;
    private readonly byte[] _input = Helper.GenerateArray(_length);
    private readonly byte[] _correct = new byte[_length];

    [TestInitialize]
    public void Initialize()
    {
        IBinarizationAlgorithm algorithm = new CSharp.Algorithms.Imperative.Otsu();
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
        testResult = Functional.Otsu.Binarize(testResult);
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
        var algorithm = new CSharp.Algorithms.OOP.Otsu();
        algorithm.Binarize(testResult);
        CollectionAssert.AreEqual(_correct, testResult);
    }
}