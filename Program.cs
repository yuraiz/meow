using MathNet.Numerics.LinearAlgebra.Double;
using meow.src;

{
    Console.WriteLine("Lab 4");
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
}

{
    // Check for lab 5

    var mines = DenseVector.OfArray(new[] {
        20.0, 30.0, 50.0,
    });

    var factories = DenseVector.OfArray(new[] {
        25.0, 25.0, 25.0, 25.0
    });

    var matrix = DenseMatrix.OfArray(new[,] {
        {20.0,   0.0,   0.0,   0.0},
        {5.0,  25.0,   0.0,   0.0},
        {0.0,   0.0,  25.0,  25.0}
    });

    var indexes = new List<(int, int)> {
        (0, 0), (1, 0), (1, 1), (2, 1), (2, 2), (2, 3)
    };

    var plan = new Lab5.TransportationPlan(mines, factories);

    if (!plan.matrix.Equals(matrix) || !plan.indexes.SequenceEqual(indexes))
    {
        throw new Exception();
    }
}

{
    Console.WriteLine("Lab 5");

    var mines = DenseVector.OfArray(new[] {
        100.0, 300.0, 300.0,
    });

    var factories = DenseVector.OfArray(new[] {
        300.0, 200.0, 200.0
    });

    var prices = DenseMatrix.OfArray(new[,] {
        {8.0, 4.0, 1.0},
        {8.0, 4.0, 3.0},
        {8.0, 7.0, 5.0},
    });

    var result = Lab5.SolveTransportation(mines, factories, prices);

    Console.WriteLine(result);
}

{
    Console.WriteLine("Lab 6");
    var targetFunction = DenseVector.OfArray(new[] {
        -8.0, -6.0, -4.0, -6.0,
    });

    var table = DenseMatrix.OfArray(new[,] {
        {1.0, 0.0, 2.0, 1.0},
        {0.0, 1.0, -1.0, 2.0},
    });

    var matrixD = DenseMatrix.OfArray(new[,] {
        {2.0, 1.0, 1.0, 0.0},
        {1.0, 1.0, 0.0, 0.0},
        {1.0, 0.0, 1.0, 0.0},
        {0.0, 0.0, 0.0, 0.0},
    });

    var startPlan = DenseVector.OfArray(new[] {
        2.0, 3.0, 0.0, 0.0,
    });

    var basisIndexes = new[] {
        0, 1
    };

    var basisIndexesStarred = new[] {
        0, 1
    };

    var result = Lab6.SolveQuadratic(targetFunction, table, matrixD, startPlan, basisIndexes, basisIndexesStarred);

    Console.WriteLine(result);
}