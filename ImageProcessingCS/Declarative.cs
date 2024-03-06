using System.Runtime.Versioning;
using ImageProcessingCS;

namespace ImageProcessing;

[SupportedOSPlatform("windows")]
public class Declarative : ISolution
{
    public static byte[] Binarize(byte[] pixels, byte threshold = 128)
    {
        return pixels.Map(pixel => pixel > threshold ? (byte)255 : (byte)0);
    }

    public static byte[] BinarizeOtsu(byte[] pixels)
    {
        return Binarize(pixels, ThresholdingOtsu(pixels));
    }

    private static byte ThresholdingOtsu(byte[] pixels)
    {
        var histogram = Histogram(pixels);

        var totalPixels = pixels.Length;
        var threshold = Enumerable.Range(0, 256)
            .Select(t => CalculateSigmaB(histogram, totalPixels, t))
            .MaxBy(sigmaB => sigmaB).threshold;

        return (byte)threshold;
    }

    private static Dictionary<int, int> Histogram(byte[] pixels)
    {
        return pixels.GroupBy(i => i)
            .ToDictionary(g => (int)g.Key, g => g.Count());
    }

    private static (double sigmaB, int threshold) CalculateSigmaB(Dictionary<int, int> histogram, int totalPixels, int t)
    {
        var qL = histogram.Where(kvp => kvp.Key < t).Sum(kvp => (double)kvp.Value / totalPixels);
        var qH = histogram.Where(kvp => kvp.Key >= t).Sum(kvp => (double)kvp.Value / totalPixels);

        if (qL == 0 || qH == 0) return (0, t);
        var miuL = histogram.Where(kvp => kvp.Key < t).Sum(kvp => kvp.Key * (double)kvp.Value / totalPixels) / qL;
        var miuH = histogram.Where(kvp => kvp.Key >= t).Sum(kvp => kvp.Key * (double)kvp.Value / totalPixels) / qH;

        return (qL * qH * (miuL - miuH) * (miuL - miuH), t);
    }
}