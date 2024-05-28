namespace CSharp.Algorithms.Declarative;

public class Sauvola : IBinarizationAlgorithm
{
    public string Name => "Sauvola";
    public string Paradigm => "Declarative";

    public void Binarize(byte[] pixels, int width, int height)
    {
        int windowSize = (int)(width * 0.02f);
        const float k = 0.2f;
        const float r = 127.5f;

        var (mean, std) = MeanStdIntegralImage(pixels, width, height, windowSize);

        var transformedPixels = pixels.Select((pixel, index) =>
                pixel > mean[index] * (1 + k * (std[index] / r - 1))
                    ? byte.MaxValue
                    : byte.MinValue)
            .ToArray();

        Array.Copy(transformedPixels, pixels, pixels.Length);
    }

    private static (float[], float[]) MeanStdIntegralImage(byte[] pixels, int width, int height, int windowSize)
    {
        int[,] integralImage = CalculateIntegralImage(pixels, width, height);
        int[,] squaredIntegralImage = CalculateIntegralImage(pixels, width, height, pixel => pixel * pixel);

        var mean = new float[width * height];
        var std = new float[width * height];
        var half = windowSize / 2;
        var windowArea = windowSize * windowSize;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                int x1 = Math.Max(x - half, 0);
                int y1 = Math.Max(y - half, 0);
                int x2 = Math.Min(x + half, width - 1);
                int y2 = Math.Min(y + half, height - 1);

                int sum = integralImage[y2, x2] - integralImage[y1, x2] - integralImage[y2, x1] + integralImage[y1, x1];
                int sumSquares = squaredIntegralImage[y2, x2] - squaredIntegralImage[y1, x2] - squaredIntegralImage[y2, x1] + squaredIntegralImage[y1, x1];

                float meanValue = (float)sum / windowArea;
                float variance = (sumSquares / windowArea) - (meanValue * meanValue);
                float stdValue = (float)Math.Sqrt(variance);

                var i = y * width + x;
                mean[i] = meanValue;
                std[i] = stdValue;
            }
        }

        return (mean, std);
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