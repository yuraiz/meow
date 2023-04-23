namespace meow.src;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public static class Lab4
{
    public static Vector<double> DualSimplex(
        Vector<double> targetFunction,
        Matrix<double> table,
        Vector<double> b,
        int[] basisIndexes
    )
    {
        var inversedBasisMatrix = table.ByBasis(basisIndexes).Inverse();
        var additionalPossiblePlan = targetFunction.ByBasis(basisIndexes) * inversedBasisMatrix;

        while (true)
        {
            var kappaBasis = inversedBasisMatrix * b;

            var kappa = DenseVector.OfEnumerable(Enumerable.Repeat(0.0, targetFunction.Count));

            for (int i = 0; i < kappaBasis.Count; i++)
            {
                var basisIndex = basisIndexes[i];
                kappa[basisIndex] = kappaBasis[i];
            }

            if (kappaBasis.All(k => k >= 0.0))
            {
                return kappa;
            }
            else
            {
                DualSimplexIter(targetFunction, additionalPossiblePlan, table, inversedBasisMatrix, kappa, basisIndexes);
                inversedBasisMatrix = table.ByBasis(basisIndexes).Inverse();
            }
        }
    }

    private static void DualSimplexIter(
        Vector<double> targetFuncion,
        Vector<double> additionalPossiblePlan,
        Matrix<double> table,
        Matrix<double> inversedBasisMatrix,
        Vector<double> kappa,
        int[] basisIndexes
    )
    {
        var selectedKappaIndex = kappa.EnumerateIndexed().First(entry => entry.Item2 < 0.0).Item1;

        var indexOfBasisIndex = Array.IndexOf(basisIndexes, selectedKappaIndex);

        var deltaY = inversedBasisMatrix.Row(indexOfBasisIndex);

        var nonBasisIndexes = Enumerable.Range(0, table.ColumnCount).Where(index => !basisIndexes.Contains(index)).ToArray();

        var mu = nonBasisIndexes.Select(index => deltaY * table.Column(index)).ToArray();

        if (mu.All(m => m >= 0.0))
        {
            throw new Lab3.TaskNotFeasibleException();
        }

        var (index, b) = mu.Select((element, index) =>
        {
            var result = double.PositiveInfinity;
            if (element < 0.0)
            {
                result = (targetFuncion[index] - table.Column(index) * additionalPossiblePlan) / element;
            }
            return (index, result);
        }).MinBy(pair => pair.result);

        basisIndexes[indexOfBasisIndex] = index;

        additionalPossiblePlan += b * deltaY;
    }
}