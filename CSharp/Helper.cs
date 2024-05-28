namespace CSharp;

public static class Helper
{
    // Extension method to map a function to each element of an array
    public static byte[] Map(this byte[] pixels, Func<byte, byte> mapFunction)
    {
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = mapFunction(pixels[i]);
        }
        return pixels;
    }

    public static byte[] Map2(this byte[] pixels, Func<byte, byte> mapFunction)
    {
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = mapFunction(pixels[i]);
        }
        return pixels;
    }

    public static byte[] GenerateArray(int length)
    {
        var array = new byte[length];
        var random = new Random();
        for (var i = 0; i < length; i++)
        {
            array[i] = (byte)random.Next(0, 256);
        }
        return array;
    }

    public static byte Average(this byte[] window)
    {
        var sum = window.Sum(pixel => pixel);
        return (byte)(sum / window.Length);
    }

    public static double StdDev(this byte[] window, byte mean)
    {
        var sum = window.Sum(pixel => Math.Pow(pixel - mean, 2));
        return Math.Sqrt(sum / window.Length);
    }

    // Helper methods for metric calculations
    private static double CalculatePERR(byte[] original, byte[] binarized, int width, int height)
    {
        var errorCount = 0;
        for (int i = 0; i < original.Length; i++)
        {
            if (original[i] != binarized[i])
            {
                errorCount++;
            }
        }

        return (double)errorCount / (width * height);
    }

    private static double CalculateMSE(byte[] original, byte[] binarized, int width, int height)
    {
        var sumSquaredError = 0.0;
        for (int i = 0; i < original.Length; i++)
        {
            var error = original[i] - binarized[i];
            sumSquaredError += error * error;
        }

        return sumSquaredError / (width * height);
    }

    private static double CalculateSNR(byte[] original, byte[] binarized, int width, int height)
    {
        var mse = CalculateMSE(original, binarized, width, height);
        var variance = CalculateVariance(original);

        return 10 * Math.Log10(variance / mse);
    }

    private static double CalculatePSNR(byte[] original, byte[] binarized, int width, int height)
    {
        var mse = CalculateMSE(original, binarized, width, height);
        return 10 * Math.Log10(255 * 255 / mse);
    }

    private static double CalculateVariance(byte[] data)
    {
        var mean = data.Average();
        var sumSquaredDifference = 0.0;
        foreach (var value in data)
        {
            sumSquaredDifference += Math.Pow(value - mean, 2);
        }

        return sumSquaredDifference / data.Length;
    }
}