namespace CSharp.Algorithms.OOP;

// ReSharper disable once InconsistentNaming
public class Otsu : IBinarizationAlgorithm
{
    private byte[] Pixels { get; set; } = Array.Empty<byte>();
    private readonly int[] _histogram = new int[256];

    public override void Binarize(byte[] pixels)
    {
        Pixels = pixels;

        var threshold = ThresholdingOtsu();
        for (var i = 0; i < Pixels.Length; i++)
        {
            Pixels[i] = (byte)(Pixels[i] > threshold ? 255 : 0);
        }
    }

    private void ComputeHistogram()
    {
        for (var i = 0; i < _histogram.Length; i++) _histogram[i] = 0;
        foreach (var i in Pixels) _histogram[i]++;
    }

    private byte ThresholdingOtsu()
    {
        const int nbins = 256;

        ComputeHistogram();

        var p = new double[_histogram.Length];
        for (var i = 0; i < _histogram.Length; i++) p[i] = (double)_histogram[i] / Pixels.Length;

        var sigmaB = new double[nbins];

        for (var t = 0; t < nbins; t++)
        {
            var qL = 0.0;
            var qH = 0.0;
            for (var i = 0; i < t; i++) qL += p[i];

            for (var i = t; i < nbins; i++) qH += p[i];

            if (qL == 0 || qH == 0) continue;

            var miuL = 0.0;
            var miuH = 0.0;
            for (var i = 0; i < t; i++) miuL += i * p[i];

            for (var i = t; i < nbins; i++) miuH += i * p[i];

            miuH /= qH;
            miuL /= qL;

            sigmaB[t] = qL * qH * (miuL - miuH) * (miuL - miuH);
        }

        var threshold = 0;
        var max = sigmaB[0];
        for (var i = 1; i < sigmaB.Length; i++)
        {
            if (sigmaB[i] <= max) continue;
            max = sigmaB[i];
            threshold = i;
        }

        return (byte)threshold;
    }
}