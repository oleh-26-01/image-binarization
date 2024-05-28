using System.Runtime.Versioning;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using CSharp;

namespace Common.Benchmarks;

[SupportedOSPlatform("windows")]
[MemoryDiagnoser]
public class Otsu
{
    private const int Width = 1024;
    private const int Height = 1024;
    private const int ImagesPerIteration = 100; 
    private readonly string _imagePath = @"C:\#Coding\C#\ImageBinarizationBenchmarks\ImageBinarizationBenchmarks\TestData\all\img";

    [Benchmark]
    public void TestImperative()
    {
        BinarizeBatch(new CSharp.Algorithms.Imperative.Otsu());
    }

    [Benchmark]
    public void TestDeclarative()
    {
        BinarizeBatch(new CSharp.Algorithms.Declarative.Otsu());
    }


    [Benchmark]
    public void TestFunctional()
    {
        var imageFiles = Directory.GetFiles(_imagePath, "*.jpg").Take(ImagesPerIteration).ToArray(); // Load 100 images

        foreach (var file in imageFiles)
        {
            var image = new Image(file);
            Functional.Otsu.Binarize(image.GrayPixels);

        }
    }


    [Benchmark]
    public void TestOOP()
    {
        BinarizeBatch(new CSharp.Algorithms.OOP.Otsu());
    }

    private void BinarizeBatch(IBinarizationAlgorithm algorithm)
    {
        var imageFiles = Directory.GetFiles(_imagePath, "*.jpg").Take(ImagesPerIteration).ToArray(); // Load 100 images

        foreach (var file in imageFiles)
        {
            var image = new Image(file);
            algorithm.Binarize(image.GrayPixels);
        }
    }
}