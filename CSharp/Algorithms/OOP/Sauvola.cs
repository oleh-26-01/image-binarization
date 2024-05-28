using System;

namespace CSharp.Algorithms.OOP;

public class Sauvola : IBinarizationAlgorithm
{
    public string Name => "Sauvola";
    public string Paradigm => "OOP";

    private byte[] Pixels { get; set; } = [];
    private int Width { get; set; }
    private int Height { get; set; }

    public void Binarize(byte[] pixels, int width, int height)
    {
        Pixels = pixels;
        Width = width;
        Height = height;

        var (mean, std) = CalculateMeanAndStd();
        ApplyThreshold(mean, std);
    }

    private (float[], float[]) CalculateMeanAndStd()
    {
        const float k = 0.2f;
        const float r = 127.5f;
        int windowSize = (int)(Width * 0.02f);
        
        int[,] integralImage = CalculateIntegralImage(Pixels, Width, Height);
        int[,] squaredIntegralImage = CalculateIntegralImage(Pixels, Width, Height, pixel => pixel * pixel);

        var mean = new float[Width * Height];
        var std = new float[Width * Height];
        var half = windowSize / 2;
        var windowArea = windowSize * windowSize;

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                int x1 = Math.Max(x - half, 0);
                int y1 = Math.Max(y - half, 0);
                int x2 = Math.Min(x + half, Width - 1);
                int y2 = Math.Min(y + half, Height - 1);

                int sum = integralImage[y2, x2] - integralImage[y1, x2] - integralImage[y2, x1] + integralImage[y1, x1];
                int sumSquares = squaredIntegralImage[y2, x2] - squaredIntegralImage[y1, x2] - squaredIntegralImage[y2, x1] + squaredIntegralImage[y1, x1];

                float meanValue = (float)sum / windowArea;
                float variance = (sumSquares / windowArea) - (meanValue * meanValue);
                float stdValue = (float)Math.Sqrt(variance);

                var i = y * Width + x;
                mean[i] = meanValue;
                std[i] = stdValue;
            }
        }
        
        return (mean, std);
    }

    private void ApplyThreshold(float[] mean, float[] std)
    {
        const float k = 0.2f;
        const float r = 127.5f;

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var i = y * Width + x;
                var threshold = mean[i] * (1 + k * (std[i] / r - 1));
                Pixels[i] = Pixels[i] > threshold ? byte.MaxValue : byte.MinValue;
            }
        }
    }
    
    private static int[,] CalculateIntegralImage(byte[] pixels, int width, int height, Func<byte, int> transform = null)
    {
        var integralImage = new int[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int pixelValue = (transform != null) ? transform(pixels[y * width + x]) : pixels[y * width + x];
                integralImage[y, x] = pixelValue;
                if (x > 0) integralImage[y, x] += integralImage[y, x - 1];
                if (y > 0) integralImage[y, x] += integralImage[y - 1, x];
                if (x > 0 && y > 0) integralImage[y, x] -= integralImage[y - 1, x - 1];
            }
        }

        return integralImage;
    }
}
