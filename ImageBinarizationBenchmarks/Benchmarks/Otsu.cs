using System.Runtime.Versioning;
using BenchmarkDotNet.Attributes;
using CSharp;
using ImageProcessing;

namespace Common.Benchmarks;

[SupportedOSPlatform("windows")]
[MemoryDiagnoser]
public class Otsu
{
    private readonly byte[] _input = new byte[1024 * 1024];
    private readonly byte[] _correct = new byte[1024 * 1024];
    private byte[] _testResult = new byte[1024 * 1024];

    public Otsu()
    {
        IBinarizationAlgorithm algorithm = new CSharp.Algorithms.OOP.Otsu();
        _input = Helper.GenerateArray(1024 * 1024);
        Array.Copy(_input, _correct, _input.Length);
        algorithm.Binarize(_correct);
    }

    [Benchmark]
    public void TestImperative()
    {
        var algorithm = new CSharp.Algorithms.Imperative.Otsu();
        Array.Copy(_input, _testResult, _input.Length);
        algorithm.Binarize(_testResult);
    }

    [Benchmark]
    public void TestDeclarative()
    {
        var algorithm = new CSharp.Algorithms.Declarative.Otsu();
        Array.Copy(_input, _testResult, _input.Length);
        algorithm.Binarize(_testResult);
    }

    [Benchmark]
    public void TestFunctional()
    {
        _testResult = Functional.BinarizeOtsu(_input);
    }

    [Benchmark]
    public void TestOOP()
    {
        var algorithm = new CSharp.Algorithms.OOP.Otsu();
        Array.Copy(_input, _testResult, _input.Length);
        algorithm.Binarize(_testResult);
    }
}