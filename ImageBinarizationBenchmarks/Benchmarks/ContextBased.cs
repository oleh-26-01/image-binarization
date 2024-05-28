using System.Runtime.Versioning;
using BenchmarkDotNet.Attributes;
using System.Drawing;

namespace Common.Benchmarks;

[SupportedOSPlatform("windows")]
[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 5, invocationCount: 128)]
public class ContextBased
{
    private byte[] _image1Data;
    private byte[] _image2Data;
    private byte[] _image3Data;
    private int _index;

    [GlobalSetup]
    public void Setup()
    {
        using var image1 = new Bitmap(@"C:\\#Coding\\C#\\ImageBinarizationBenchmarks\\ImageBinarizationBenchmarks\\TestData\\image1.jpg");
        using var image2 = new Bitmap(@"C:\\#Coding\\C#\\ImageBinarizationBenchmarks\\ImageBinarizationBenchmarks\\TestData\\image1-1.jpg");
        using var image3 = new Bitmap(@"C:\\#Coding\\C#\\ImageBinarizationBenchmarks\\ImageBinarizationBenchmarks\\TestData\\image1-2.jpg");
        _image1Data = ImageToByteArray(image1);
        _image2Data = ImageToByteArray(image2);
        _image3Data = ImageToByteArray(image3);

        _index = 0;
    }
    
    public enum ImageSource
    {
        Image1,
        Image2,
        Image3
    }

    public int Width => 1216;
    public int Height => 1944;

    [ParamsSource(nameof(ImageSources))]
    public ImageSource CurrentImage { get; set; }

    public IEnumerable<ImageSource> ImageSources => Enum.GetValues(typeof(ImageSource)).Cast<ImageSource>();

    private byte[] _testResult;

    [IterationSetup]
    public void IterationSetup()
    {
        _testResult = CurrentImage switch
        {
            ImageSource.Image1 => new byte[_image1Data.Length],
            ImageSource.Image2 => new byte[_image2Data.Length],
            ImageSource.Image3 => new byte[_image3Data.Length],
            _ => _testResult
        };
    }

    public static void CopyImage(ImageSource source, byte[] sourceData, byte[] destinationData)
    {
        switch (source)
        {
            case ImageSource.Image1:
                Array.Copy(sourceData, destinationData, sourceData.Length);
                break;
            case ImageSource.Image2:
                Array.Copy(sourceData, destinationData, sourceData.Length);
                break;
            case ImageSource.Image3:
                Array.Copy(sourceData, destinationData, sourceData.Length);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [Benchmark]
    public void TestImperativeOtsu()
    {
        var algorithm = new CSharp.Algorithms.Imperative.Otsu();
        CopyImage(CurrentImage, _image1Data, _testResult);
        algorithm.Binarize(_testResult);
    }

    [Benchmark]
    public void TestDeclarativeOtsu()
    {
        var algorithm = new CSharp.Algorithms.Declarative.Otsu();
        CopyImage(CurrentImage, _image1Data, _testResult);
        algorithm.Binarize(_testResult);
    }

    [Benchmark]
    public void TestFunctionalOtsu()
    {
        _testResult = Functional.Otsu.Binarize(_testResult);
    }

    [Benchmark]
    public void TestOOPOtsu()
    {
        var algorithm = new CSharp.Algorithms.OOP.Otsu();
        CopyImage(CurrentImage, _image1Data, _testResult);
        algorithm.Binarize(_testResult);
    }

    [Benchmark]
    public void TestImperativeSauvola()
    {
        var algorithm = new CSharp.Algorithms.Imperative.Sauvola();
        CopyImage(CurrentImage, _image1Data, _testResult);
        algorithm.Binarize(_testResult, Width, Height);
    }

    [Benchmark]
    public void TestDeclarativeSauvola()
    {
        var algorithm = new CSharp.Algorithms.Declarative.Sauvola();
        CopyImage(CurrentImage, _image1Data, _testResult);
        algorithm.Binarize(_testResult, Width, Height);
    }

    [Benchmark]
    public void TestFunctionalSauvola()
    {
        _testResult = Functional.Sauvola.Binarize(_testResult, Width, Height);
    }

    [Benchmark]
    public void TestOOPSauvola()
    {
        var algorithm = new CSharp.Algorithms.OOP.Sauvola();
        CopyImage(CurrentImage, _image1Data, _testResult);
        algorithm.Binarize(_testResult, Width, Height);
    }

    private static byte[] ImageToByteArray(Bitmap image)
    {
        var data = new byte[image.Width * image.Height];
        int index = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var pixel = image.GetPixel(x, y);
                data[index++] = (byte)((pixel.R + pixel.G + pixel.B) / 3);
            }
        }
        return data;
    }
}