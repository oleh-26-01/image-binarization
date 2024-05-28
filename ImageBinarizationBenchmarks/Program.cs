using System.Diagnostics;
using System.Runtime.Versioning;
using BenchmarkDotNet.Running;
using Common.Benchmarks;
using CSharp;

namespace Common;

[SupportedOSPlatform("windows")]
public class Program
{
    public static void Sample()
    {
        IBinarizationAlgorithm algorithm = new CSharp.Algorithms.Imperative.Sauvola();

        var folderPath = @"C:\#Coding\C#\ImageBinarizationBenchmarks\ImageBinarizationBenchmarks\TestData\all\img";
        var outputPath = $@"{folderPath}\binarized\Sauvola";
        // binarize all images in the folder with algorithm
        var files = Directory.GetFiles(folderPath, "*.jpg");
        var count = 0;
        var total = files.Length;
        // start time
        var watch = Stopwatch.StartNew();
        foreach (var file in files)
        {
            var image = new Image(file);
            algorithm.Binarize(image.GrayPixels, image.Width, image.Height);
            image.Save($@"{outputPath}\{Path.GetFileName(file)}", true);
            /*image.SaveBmp(
                $@"{outputPath}\{Path.GetFileName(file)
                    .Replace(
                        ".jpg", 
                        $"_{algorithm.Name}_{algorithm.Paradigm}.bmp")}");*/
            count++;
            //Console.WriteLine($"Image {count}/{total} binarized successfully.");
            if (count % 100 == 0)
            {
                var elapsed = watch.Elapsed;
                var remaining = TimeSpan.FromTicks(elapsed.Ticks / count * (total - count));
                Console.WriteLine($"Image {count}/{total} binarized successfully. " +
                                  $"Elapsed: {elapsed}, " +
                                  $"Remaining: {remaining}");
            }
        }
    }

    public static void CalcMetrics()
    {
        // initialize two algorithms in array
        var algorithms = new IBinarizationAlgorithm[]
        {
            new CSharp.Algorithms.Imperative.Otsu(),
            new CSharp.Algorithms.Imperative.Sauvola()
        };

        var folderPath = @"C:\#Coding\C#\ImageBinarizationBenchmarks\ImageBinarizationBenchmarks\TestData\all";
        var gtFolder = "GT";
        var imagesFolder = "img";


    }

    public static void Main()
    {
        Console.Write(string.Join("\n",
            "1. Benchmark",
            "2. Sample",
            "Choose option: "));
        var option = Convert.ToByte(Console.ReadLine());
        switch (option)
        {
            case 1:
                BenchmarkRunner.Run<Otsu>();
                break;
            case 2:
                Sample();
                break;
        }
    }
}