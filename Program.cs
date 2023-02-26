using MathNet.Numerics.LinearAlgebra.Double;
using meow.src;

var targetFunction = DenseVector.OfArray(new[] {
    1.0, 1.0, 0.0, 0.0, 0.0
});

var table = DenseMatrix.OfArray(new[,] {
    {-1.0, 1.0, 1.0, 0.0, 0.0},
    {1.0, 0.0, 0.0, 1.0, 0.0},
    {0.0, 1.0, 0.0, 0.0, 1.0},
});

// Starts with 0
var basisIndexes = new[] { 2, 3, 4 };

var initialValues = DenseVector.OfArray(new[] {
    0.0, 0.0, 1.0, 3.0, 2.0
});

var expectedResult = DenseVector.OfArray(new[] {
    3.0, 2.0, 2.0, 0.0, 0.0
});

var result = Lab2.SimplexPhase2(targetFunction, table, basisIndexes, initialValues);

if (!expectedResult.Equals(result))
{
    throw new Exception("Unexpected result");
}