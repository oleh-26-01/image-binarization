using System.Diagnostics;
using System.Runtime.Versioning;
using ImageProcessing;

namespace ImageBinarizationBenchmarks;

[SupportedOSPlatform("windows")]
public class Program
{
    private static void Task1()
    {
        var folderPath = @"C:\#Coding\C#\ImageBinarizationBenchmarks\ImageBinarizationBenchmarks\TestData";
        var imageName = "image3.jpg";
        var inputPath = $@"{folderPath}\{imageName}";
        // var outputPath = $@"{folderPath}\{imageName.Replace(".jpg", "_binarized.jpg")}";
        var outputPathImperative = $@"{folderPath}\{imageName.Replace(".jpg", "_binarized_imperative.jpg")}";
        var outputPathDeclarative = $@"{folderPath}\{imageName.Replace(".jpg", "_binarized_declarative.jpg")}";
        var outputPathFunctional = $@"{folderPath}\{imageName.Replace(".jpg", "_binarized_functional.jpg")}";
        var outputPathOOP = $@"{folderPath}\{imageName.Replace(".jpg", "_binarized_oop.jpg")}";

        var imageImperative = new Image(inputPath);
        var imageDeclarative = new Image(inputPath);
        var imageFunctional = new Image(inputPath);
        var imageOOP = new Image(inputPath);

        Imperative.BinarizeOtsu(imageImperative.GrayPixels);
        Declarative.BinarizeOtsu(imageDeclarative.GrayPixels);
        imageFunctional.GrayPixels = Functional.BinarizeOtsu(imageFunctional.GrayPixels);
        var oopInstance = new OOP(imageOOP.GrayPixels);
        oopInstance.BinarizeOtsu();

        imageImperative.Save(outputPathImperative, true);
        imageDeclarative.Save(outputPathDeclarative, true);
        imageFunctional.Save(outputPathFunctional, true);
        imageOOP.Save(outputPathOOP, true);

        Console.WriteLine("Image binarized successfully.");
    }

    public static void Task2()
    {
        var folderPath = @"C:\#Coding\C#\ImageBinarizationBenchmarks\ImageBinarizationBenchmarks\TestData";
        var outputPath = $@"{folderPath}\binarized";
        // binarize all images in the folder with imperative Otsu
        var files = Directory.GetFiles(folderPath, "*.jpg");
        var count = 0;
        var total = files.Length;
        foreach (var file in files)
        {
            var image = new Image(file);
            Imperative.BinarizeOtsu(image.GrayPixels);
            image.Save($@"{outputPath}\{Path.GetFileName(file)}", true);
            image.SaveBmp($@"{outputPath}\{Path.GetFileName(file).Replace("jpg", "bmp")}");
            count++;
            Console.WriteLine($"Image {count}/{total} binarized successfully.");
        }
    }

    public static void Task3()
    {
        // benchmark SaveBmp
        int testCount = 100;
        long time = 0;

        var imagePath = @"C:\#Coding\C#\ImageBinarizationBenchmarks\ImageBinarizationBenchmarks\TestData\image3.jpg";

        var image = new Image(imagePath);
        var outputPath = @"C:\#Coding\C#\ImageBinarizationBenchmarks\ImageBinarizationBenchmarks\TestData\image3.bmp";

        for (var i = 0; i < testCount; i++)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            image.SaveBmp(outputPath);
            stopwatch.Stop();
            time += stopwatch.ElapsedMilliseconds;
        }

        Console.WriteLine($"Average time: {time / testCount} ms");
    }

    public static void Main()
    {
        // Task1();
        Task2();
        // Task3();
    }
}