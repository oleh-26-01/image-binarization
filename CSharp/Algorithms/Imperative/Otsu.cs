using System.Runtime.Versioning;

namespace CSharp.Algorithms.Imperative;

[SupportedOSPlatform("windows")]
public class Otsu : IBinarizationAlgorithm
{
    public override void Binarize(byte[] pixels)
    {
        const int nbins = 256;
        var histogram = Histogram(pixels);

        var p = new double[histogram.Length];
        for (var i = 0; i < histogram.Length; i++) p[i] = (double)histogram[i] / pixels.Length;

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

        var tempThreshold = 0;
        var max = sigmaB[0];
        for (var i = 1; i < sigmaB.Length; i++)
        {
            if (!(sigmaB[i] > max)) continue;
            max = sigmaB[i];
            tempThreshold = i;
        }

        byte threshold = (byte)tempThreshold;

        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = (byte)(pixels[i] > threshold ? 255 : 0);
        }
    }

    private static int[] Histogram(byte[] pixels)
    {
        var histogram = new int[256];
        foreach (var i in pixels) histogram[i]++;
        return histogram;
    }
}