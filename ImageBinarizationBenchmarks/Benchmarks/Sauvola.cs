﻿using System.Runtime.Versioning;
using BenchmarkDotNet.Attributes;
using CSharp;
using Functional;

namespace Common.Benchmarks;

[SupportedOSPlatform("windows")]
[MemoryDiagnoser]
public class Sauvola
{
    private const int Width = 1024;
    private const int Height = 1024;
    private const int Length = Width * Height;
    private readonly byte[] _input = new byte[Length];
    private byte[] _testResult = new byte[Length];

    [Benchmark]
    public void TestImperative()
    {
        var algorithm = new CSharp.Algorithms.Imperative.Otsu();
        Array.Copy(_input, _testResult, _input.Length);
        algorithm.Binarize(_testResult, Width, Height);
    }

    [Benchmark]
    public void TestDeclarative()
    {
        var algorithm = new CSharp.Algorithms.Declarative.Otsu();
        Array.Copy(_input, _testResult, _input.Length);
        algorithm.Binarize(_testResult, Width, Height);
    }

    [Benchmark]
    public void TestFunctional()
    {
        _testResult = Functional.Otsu.Binarize(_input);
    }

    [Benchmark]
    public void TestOOP()
    {
        var algorithm = new CSharp.Algorithms.OOP.Otsu();
        Array.Copy(_input, _testResult, _input.Length);
        algorithm.Binarize(_testResult, Width, Height);
    }
}