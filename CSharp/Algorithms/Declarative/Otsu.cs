namespace CSharp.Algorithms.Declarative;

public class Otsu : IBinarizationAlgorithm
{
    public string Name => "Otsu";
    public string Paradigm => "Declarative";

    public void Binarize(byte[] pixels, int width = -1, int height = -1)
    {
        var threshold = ThresholdingOtsu(pixels);
        const byte upValue = 255;
        const byte downValue = 0;
        pixels.Map(p => p > threshold ? upValue : downValue);
    }

    private static byte ThresholdingOtsu(byte[] pixels)
    {
        var histogram = Histogram(pixels);

        var totalPixels = pixels.Length;
        var threshold = Enumerable.Range(0, 256)
            .Select(t => CalculateSigmaB(histogram, totalPixels, t))
            .MaxBy(t => t.sigmaB).threshold;

        return (byte)threshold;
    }

    private static (double sigmaB, int threshold) CalculateSigmaB(Dictionary<int, int> histogram, int totalPixels, int t)
    {
        var p = histogram.ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value / totalPixels);

        var qL = p.Where(kvp => kvp.Key < t).Sum(kvp => kvp.Value);
        var qH = p.Where(kvp => kvp.Key >= t).Sum(kvp => kvp.Value);

        if (qL == 0 || qH == 0) return (0, t);
        var miuL = p.Where(kvp => kvp.Key < t).Sum(kvp => kvp.Key * kvp.Value) / qL;
        var miuH = p.Where(kvp => kvp.Key >= t).Sum(kvp => kvp.Key * kvp.Value) / qH;

        return (qL * qH * (miuL - miuH) * (miuL - miuH), t);
    }

    private static Dictionary<int, int> Histogram(byte[] pixels)
    {
        return pixels.GroupBy(i => i)
            .ToDictionary(g => (int)g.Key, g => g.Count());
    }
}