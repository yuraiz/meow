namespace meow.src;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public static class Lab3
{
    public sealed class TaskIncompatibleException : Exception { }

    public static Vector<double>? SimplexMethod(Vector<double> targetFunction, Matrix<double> table, Vector<double> b)
    {
        // first step
        var negativeIndexes = b.EnumerateIndexed().Where((entry) => entry.Item2 < 0.0).Select((entry) => entry.Item1);
        foreach (var index in negativeIndexes)
        {
            table.SetRow(index, table.Row(index) * -1.0);
            b[index] *= -1;
        }

        // second step
        var n = table.ColumnCount;
        var m = table.RowCount;

        var fictiveTargetFunction = DenseVector.OfEnumerable(Enumerable.Repeat(0.0, n).Concat(Enumerable.Repeat(-1.0, m)));

        var fictiveTable = DenseMatrix.OfColumns(table.EnumerateColumns().Concat(DenseMatrix.CreateIdentity(m).EnumerateColumns()));

        var fictiveBasis = Enumerable.Range(n, m).ToArray();

        var fictiveInitialValues = DenseVector.OfEnumerable(Enumerable.Repeat(0.0, n).Concat(b.Enumerate()));

        // third step
        var initialValues = Lab2.SimplexPhase2(fictiveTargetFunction, fictiveTable, fictiveBasis, fictiveInitialValues);
        if (initialValues == null)
        {
            return null;
        }

        if (initialValues.Skip(table.ColumnCount).Any(el => el != 0))
        {
            throw new TaskIncompatibleException();
        }

        initialValues = DenseVector.OfEnumerable(initialValues.Take(table.ColumnCount));

        (var basisIndexes, table) = ClearBasisIndexes(fictiveBasis, table, fictiveTable);

        return Lab2.SimplexPhase2(targetFunction, table, basisIndexes, initialValues);
    }

    private static (int[], Matrix<double>) ClearBasisIndexes(int[] basisIndexes, Matrix<double> table, Matrix<double> fictiveTable)
    {
        // var n = table.ColumnCount;
        var invFictiveTable = new Lazy<Matrix<double>>(() => fictiveTable.ByBasis(basisIndexes).Inverse());
        for (var i = 0; i < basisIndexes.Length; i++)
        {
            if (basisIndexes[i] >= table.ColumnCount)
            {
                foreach (var (replacementIndex, column) in table.EnumerateColumnsIndexed())
                {
                    var j = (invFictiveTable.Value * column)[i];
                    if (j != 0)
                    {
                        basisIndexes[i] = replacementIndex;
                    }
                }
                if (basisIndexes[i] >= table.ColumnCount)
                {
                    table = table.RemoveRow(i);
                    var list = basisIndexes.ToList();
                    list.RemoveAt(i);
                    list.ForEach((index) =>
                        {
                            if (index > i) index -= 1;
                        }
                    );
                    basisIndexes = list.ToArray();
                }
            }
        }

        return (basisIndexes, table);
    }
}