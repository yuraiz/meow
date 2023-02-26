namespace meow.src;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public static class Lab2
{
    private static Matrix<double> ByBasis(this Matrix<double> matrix, int[] basisIndexes)
        => Matrix.Build.DenseOfColumns(
            basisIndexes.Select(index => matrix.Column(index))
        );

    private static Vector<double> ByBasis(this Vector<double> vector, int[] basisIndexes)
        => Vector.Build.DenseOfEnumerable(
            basisIndexes.Select(index => vector[index])
        );

    public static Vector<double>? SimplexPhase2(Vector<double> targetFunction, Matrix<double> table, int[] basis, Vector<double> initialValues)
    {
        var inversedBasisMatrix = table.ByBasis(basis).Inverse();

        while (true)
        {
            var basisTargetFunction = targetFunction.ByBasis(basis);

            var u = inversedBasisMatrix.LeftMultiply(basisTargetFunction);

            var triangle = table.LeftMultiply(u) - targetFunction;

            var nonBasisItemsLessThanZero = triangle.EnumerateIndexed()
                                .SkipWhile((item, index) => basis.Contains(index) || item.Item2 >= 0.0);

            if (nonBasisItemsLessThanZero.Any())
            {
                var j0 = nonBasisItemsLessThanZero.First().Item1;

                var minTetaIndex = SimplexPhase2IterMainPart(inversedBasisMatrix, j0, table, basis, initialValues);

                if (minTetaIndex == null)
                {
                    return null;
                }

                inversedBasisMatrix = Lab1.InverseOfMatrix(inversedBasisMatrix, table.Column(j0), minTetaIndex.Value);

                if (inversedBasisMatrix == null)
                {
                    throw new ArgumentException("Can't use simplex algoritm task for this arguments");
                }
            }
            else
            {
                return initialValues;
            }
        }
    }

    private static int? SimplexPhase2IterMainPart(Matrix<double> inversedBasisMatrix, int j0, Matrix<double> table, int[] basis, Vector<double> initialValues)
    {
        var z = inversedBasisMatrix * table.Column(j0);

        var tetas = DenseVector.OfEnumerable(z.MapIndexed((i, value) =>
            value <= 0 ? double.PositiveInfinity : initialValues[basis[i]] / value
        ));

        var minTetaIndex = tetas.MinimumIndex();
        var minTeta = tetas[minTetaIndex];

        if (double.IsInfinity(minTeta))
        {
            return null;
        }

        foreach (var i in Enumerable.Range(0, basis.Length))
        {
            initialValues[basis[i]] -= minTeta * z[i];
        }

        initialValues[basis[minTetaIndex]] = 0.0;
        basis[minTetaIndex] = j0;
        initialValues[j0] = minTeta;

        return minTetaIndex;
    }
}
