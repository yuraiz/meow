namespace meow.src;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public static class Lab6
{
    public static Matrix<double> ByBasisSquare(this Matrix<double> matrix, int[] basisIndexes)
        => matrix.ByBasis(basisIndexes)
              .Transpose()
              .ByBasis(basisIndexes)
              .Transpose();

    public static Vector<double> SolveQuadratic(
        Vector<double> targetFunction,
        Matrix<double> table,
        Matrix<double> matrixD,
        Vector<double> startPlan,
        int[] basisIndexes,
        int[] basisIndexesStarred)
    {
        while (true)
        {
            var inversedBasisMatrix = table.ByBasis(basisIndexes).Inverse();
            var cx = targetFunction + startPlan * matrixD;
            var cb = cx.ByBasis(basisIndexes);
            var ux = -cb * inversedBasisMatrix;
            var dx = ux * table + cx;

            if (dx.All(val => val >= 0))
            {
                return startPlan;
            }

            var (j0, _) = dx.Find(val => val < 0);

            var matrixH = MakeMatrixH(table, matrixD, basisIndexesStarred);
            var l = MakeL(targetFunction, table, matrixD, matrixH, basisIndexesStarred, j0);

            var delta = l * matrixD * l;

            var tetaJ0 = double.Abs(dx[j0]) / delta;
            var otherTetas = l.Select((l, index) =>
            {
                if (l >= 0)
                {
                    return (index, double.PositiveInfinity);
                }
                else
                {
                    return (index, targetFunction[index] / l);
                }
            });

            var (jStarred, teta0) = otherTetas.Append((j0, tetaJ0)).MinBy(t => t.Item2);

            startPlan += teta0 * l;

            if (double.IsInfinity(teta0))
            {
                return startPlan;
            }

            UpdateIndexes(j0, jStarred, table, ref basisIndexes, ref basisIndexesStarred);
        }
    }

    private static Matrix<double> MakeMatrixH(Matrix<double> table, Matrix<double> matrixD, int[] basisIndexesStarred)
    {
        var topLeft = matrixD.ByBasisSquare(basisIndexesStarred);
        var bottomLeft = table.ByBasis(basisIndexesStarred);
        var topRight = table.ByBasis(basisIndexesStarred).Transpose();
        var bottomRight = new DenseMatrix(topRight.ColumnCount);

        var top = topLeft.Append(topRight);
        var bottom = bottomLeft.Append(bottomRight);

        var matrixH = DenseMatrix.OfRows(
            top.EnumerateRows().Concat(bottom.EnumerateRows())
        );

        return matrixH;
    }

    private static Vector<double> MakeL(Vector<double> targetFunction, Matrix<double> table, Matrix<double> matrixD, Matrix<double> matrixH, int[] basisIndexesStarred, int j0)
    {

        var topB = matrixD.Column(j0).ByBasis(basisIndexesStarred);
        var bottomB = table.Column(j0);

        var bStarred = DenseVector.OfEnumerable(topB.Concat(bottomB));
        var inveredH = matrixH.Inverse();

        var xWaved = -inveredH * bStarred;

        var notBasisIndexesStarred = Enumerable.Range(0, targetFunction.Count).Except(basisIndexesStarred).ToArray();

        foreach (var index in notBasisIndexesStarred)
        {
            xWaved[index] = 0;
        }
        xWaved[j0] = 1;

        var l = DenseVector.OfEnumerable(xWaved.Take(matrixD.ColumnCount));

        return l;
    }


    private static void UpdateIndexes(int j0, int jStarred, Matrix<double> table, ref int[] basisIndexes, ref int[] basisIndexesStarred)
    {
        if (j0 == jStarred)
        {
            basisIndexesStarred = basisIndexesStarred.Append(jStarred).ToArray();
        }
        else if (basisIndexesStarred.Except(basisIndexes).Contains(jStarred))
        {
            basisIndexesStarred = basisIndexesStarred.Except(new[] { jStarred }).ToArray();
        }
        else if (basisIndexes.Contains(jStarred))
        {
            var s = Array.IndexOf(basisIndexes, jStarred);

            try
            {
                var invesedTable = table.ByBasis(basisIndexes).Inverse();
                var jPlus = basisIndexesStarred.Except(basisIndexes).First(index =>
                {
                    var res = invesedTable * table.Column(index);
                    return res[0] != 0;
                });

                basisIndexes[s] = jPlus;
                basisIndexesStarred = basisIndexesStarred.Except(new[] { jStarred }).ToArray();
            }
            catch
            {
                basisIndexes[s] = jStarred;
                s = Array.IndexOf(basisIndexesStarred, j0);
                basisIndexesStarred[s] = jStarred;
            }
            throw new NotImplementedException();
        }
        else
        {
            throw new System.Diagnostics.UnreachableException();
        }
    }
}