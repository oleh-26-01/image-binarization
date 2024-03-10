using System.Runtime.Versioning;
using BenchmarkDotNet.Running;
using Common.Benchmarks;
using CSharp;

namespace Common;

[SupportedOSPlatform("windows")]
public class Program
{
    public static void Task1()
    {
        IBinarizationAlgorithm algorithm = new CSharp.Algorithms.Imperative.Otsu();

        var folderPath = @"C:\#Coding\C#\ImageBinarizationBenchmarks\ImageBinarizationBenchmarks\TestData";
        var outputPath = $@"{folderPath}\binarized";
        // binarize all images in the folder with algorithm
        var files = Directory.GetFiles(folderPath, "*.jpg");
        var count = 0;
        var total = files.Length;
        foreach (var file in files)
        {
            var image = new Image(file);
            algorithm.Binarize(image.GrayPixels);
            image.Save($@"{outputPath}\{Path.GetFileName(file)}", true);
            image.SaveBmp($@"{outputPath}\{Path.GetFileName(file).Replace("jpg", "bmp")}");
            count++;
            Console.WriteLine($"Image {count}/{total} binarized successfully.");
        }
    }

    public static void Main()
    {
        Console.Write(string.Join("\n",
            "1. Task1",
            "2. Benchmark",
            "Choose option: "));
        var option = Convert.ToByte(Console.ReadLine());
        switch (option)
        {
            case 1:
                Task1();
                break;
            case 2:
                BenchmarkRunner.Run<Otsu>();
                break;
        }
    }
}