using MathNet.Numerics.LinearAlgebra.Double;
using meow.src;

// Example task
var targetFunction = DenseVector.OfArray(new[] {
    1.0, 0.0, 0.0
});

var table = DenseMatrix.OfArray(new[,] {
    {1.0, 1.0, 1.0},
    {2.0, 2.0, 2.0},
});

var b = new DenseVector(2);

var expectedResult = new DenseVector(3);

if (!expectedResult.Equals(Lab3.SimplexMethod(targetFunction, table, b)))
{
    throw new Exception("Unexpected result");
}

// Unrestriced task
targetFunction = DenseVector.OfArray(new[] {
    1.0, 0.0, 0.0
});

table = DenseMatrix.OfArray(new[,] {
    {0.0, 1.0, 1.0},
});

b = DenseVector.OfArray(new[] {
    0.0
});

if (Lab3.SimplexMethod(targetFunction, table, b) != null)
{
    throw new Exception("Unexpected result");
}

// Task with incompatible system
targetFunction = DenseVector.OfArray(new[] {
    1.0, 0.0, 0.0
});

table = DenseMatrix.OfArray(new[,] {
    {1.0, 1.0, 1.0},
});

b = DenseVector.OfArray(new[] { -1.0 });

try
{
    Lab3.SimplexMethod(targetFunction, table, b);

    throw new Exception("Unexpected result");
}
catch (Lab3.TaskIncompatibleException) { }
