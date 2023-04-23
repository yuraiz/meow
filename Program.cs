using MathNet.Numerics.LinearAlgebra.Double;
using meow.src;

// Example task
var targetFunction = DenseVector.OfArray(new[] {
    -4.0, 3.0, -7.0, 0.0, 0.0,
});

var table = DenseMatrix.OfArray(new[,] {
    {-2.0, -1.0, -4.0, 1.0, 0.0},
    {-2.0, -2.0, -2.0, 0.0, 1.0},
});

var b = DenseVector.OfArray(
    new[] {
        -1.0, -1.5
    }
);

var basisIndexes = new[] { 3, 4 };

var result = Lab4.DualSimplex(targetFunction, table, b, basisIndexes);
Console.WriteLine(result);
