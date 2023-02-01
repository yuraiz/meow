public static class Lab1
{
    public static double[,]? InverseOfMatrix(double[,] inversed, double[] replacedColumn, int index)
    {
        // validation
        var n = inversed.GetLength(0);
        var argumentsAreValid = inversed.GetLength(1) == n && replacedColumn.Length == n && index < n;
        if (!argumentsAreValid)
        {
            throw new ArgumentException();
        }

        // first step
        var l = MultiplyMatrixByColumn(inversed, replacedColumn);

        var li = l[index];
        if (li == 0)
        {
            return null;
        }

        // second step
        l[index] = -1;

        // third step
        var coef = -1 / li;
        for (var i = 0; i < l.Length; i++)
        {
            l[i] *= coef;
        }

        // fourth step not needed

        // fifth step

        var result = new double[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                var indexPartMul = inversed[index, j] * l[i];
                var diagonalPartMul = (index != i ? inversed[i, j] : 0);
                result[i, j] = indexPartMul + diagonalPartMul;
            }
        }

        return result;
    }

    private static double[] MultiplyMatrixByColumn(double[,] matrix, double[] column)
    {
        var n = column.Length;
        var result = new double[n];

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                result[i] += matrix[i, j] * column[j];
            }
        }

        return result;
    }
}