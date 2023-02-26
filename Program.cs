using MathNet.Numerics.LinearAlgebra.Double;

var matrix = DenseMatrix.OfArray(new[,] {
    {1.0, 0.0, 5.0},
    {2.0, 1.0, 6.0},
    {3.0, 4.0, 0.0},
});

var inversed = DenseMatrix.OfArray(new[,] {
    {-24.0,  20.0, -5.0},
    { 18.0, -15.0,  4.0},
    {  5.0,  -4.0,  1.0},
});

var replacedColumn = DenseVector.OfArray(new[] {
    2.0,
    2.0,
    2.0,
});

var index = 1;

Console.WriteLine($"matrix A = {matrix}");
Console.WriteLine($"inversed matrix A = {inversed}");
Console.WriteLine($"replaced column = {replacedColumn}");
Console.WriteLine($"index = {index} (starts with 0)");
Console.WriteLine();

var result = Lab1.InverseOfMatrix(inversed, replacedColumn, index);

if (result == null)
{
    Console.WriteLine("matrix can't be inversed");
}
else
{
    Console.WriteLine($"result = {result}");
}

// Actual result
// -0.85714  0.71428  0.14285
//  1.28571 -1.07142  0.28571
// -0.14285  0.28571 -0.14285