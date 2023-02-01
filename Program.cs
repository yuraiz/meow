var matrix = new double[3, 3] {
    {1, 0, 5},
    {2, 1, 6},
    {3, 4, 0},
};

var inversed = new double[3, 3] {
    {-24,  20, -5},
    { 18, -15,  4},
    {  5,  -4,  1},
};

var replacedColumn = new double[3] {
    2,
    2,
    2,
};

var index = 1;

matrix.PrettyPrint("matrix A");
inversed.PrettyPrint("inversed matrix A");
replacedColumn.PrettyPrintAsColumn("replaced column");
Console.WriteLine($"index = {index} (starts with 0)");

var result = Lab1.InverseOfMatrix(inversed, replacedColumn, index);

if (result == null)
{
    Console.WriteLine("matrix can't be inversed");
}
else
{
    result.PrettyPrint("result");
}

// Actual result
// -0.85714  0.71428  0.14285
//  1.28571 -1.07142  0.28571
// -0.14285  0.28571 -0.14285