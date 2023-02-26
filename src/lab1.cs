using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public static class Lab1
{
    public static Matrix? InverseOfMatrix(Matrix<double> inversed, Vector<double> replacedColumn, int index)
    {
        // validation
        var n = inversed.ColumnCount;
        var argumentsAreValid = inversed.RowCount == n && replacedColumn.Count == n && index < n;
        if (!argumentsAreValid)
        {
            throw new ArgumentException();
        }

        // first step
        var l = inversed * replacedColumn;

        var li = l[index];
        if (li == 0)
        {
            return null;
        }

        // second step
        l[index] = -1;

        // third step
        l *= -1 / li;

        // fourth step not needed

        // fifth step

        var result = new DenseMatrix(n);
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
}