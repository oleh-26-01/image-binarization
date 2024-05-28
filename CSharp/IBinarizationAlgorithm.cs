namespace CSharp;

public interface IBinarizationAlgorithm
{
    string Name { get; } // Name of the algorithm
    string Paradigm { get; } // Imperative, Declarative, Functional, OOP

    public void Binarize(byte[] pixels, int width = -1, int height = -1);
}