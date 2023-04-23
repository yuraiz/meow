namespace meow.src;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public static class Lab5
{
    public sealed class TransportationPlan
    {
        public Matrix<double> matrix;
        public List<(int, int)> indexes = new();

        public TransportationPlan(
            Vector<double> mines,
            Vector<double> factories
        )
        {
            matrix = new DenseMatrix(mines.Count, factories.Count);

            var mIndex = 0;
            var fIndex = 0;

            while (mIndex < mines.Count)
            {
                indexes.Add((mIndex, fIndex));

                var tranfered = Math.Min(mines[mIndex], factories[fIndex]);

                mines[mIndex] -= tranfered;
                factories[fIndex] -= tranfered;
                matrix[mIndex, fIndex] = tranfered;

                if (mines[mIndex] == 0)
                {
                    mIndex += 1;
                }
                else
                {
                    fIndex += 1;
                }
            }
        }
        public Matrix<double> OptimizeFor(Matrix<double> prices)
        {
            var indexToMakeBasis = IndexToMakeBasis(prices);

            while (indexToMakeBasis is not null)
            {
                Console.WriteLine(SummaryFor(prices));
                MakeIndexBasis(indexToMakeBasis.Value);
                indexToMakeBasis = IndexToMakeBasis(prices);
            }

            Console.WriteLine(SummaryFor(prices));

            return matrix;
        }

        public double SummaryFor(Matrix<double> prices)
        {
            return matrix
                .EnumerateIndexed()
                .Aggregate(0.0,
                    (sum, value) => sum += prices[value.Item1, value.Item2] * value.Item3
                );
        }

        public void MakeIndexBasis((int, int) index)
        {
            var indexes = this.indexes.Prepend(index).ToList();

            var rowsToRemove = new List<int>();
            var columnsToRemove = new List<int>();

            var count = 0;

            while (count != indexes.Count)
            {
                count = indexes.Count;

                for (var i = 0; i < matrix.RowCount; i++)
                {
                    if (indexes.Count(value => value.Item1 == i) == 1)
                    {
                        indexes.RemoveAll(value => value.Item1 == i);
                    }
                }

                for (var j = 0; j < matrix.RowCount; j++)
                {
                    if (indexes.Count(value => value.Item2 == j) == 1)
                    {
                        indexes.RemoveAll(value => value.Item2 == j);
                    }
                }
            }

            var indexToRemove = indexes.Where((_, index) => int.IsOddInteger(index)).MinBy(val => matrix[val.Item1, val.Item2]);

            var price = matrix[indexToRemove.Item1, indexToRemove.Item2];

            foreach (var (i, j) in indexes.Where((_, index) => int.IsOddInteger(index)))
            {
                matrix[i, j] -= price;
            }

            foreach (var (i, j) in indexes.Where((_, index) => int.IsEvenInteger(index)))
            {
                matrix[i, j] += price;
            }

            this.indexes.Remove(indexToRemove);
            this.indexes.Add(index);
        }

        private (int, int)? IndexToMakeBasis(Matrix<double> prices)
        {
            var uPotentials = new double[prices.RowCount];
            var vPotentials = new double[prices.ColumnCount];
            Array.Fill(uPotentials, double.PositiveInfinity);
            Array.Fill(vPotentials, double.PositiveInfinity);
            uPotentials[indexes.First().Item1] = 0.0;

            var unsolved = new Queue<(int, int)>(indexes);

            while (unsolved.TryDequeue(out (int, int) index))
            {
                var (i, j) = index;

                var sum = prices[i, j];

                if (double.IsFinite(uPotentials[i]))
                {
                    vPotentials[j] = sum - uPotentials[i];
                }
                else if (double.IsFinite(vPotentials[j]))
                {
                    uPotentials[i] = sum - vPotentials[j];
                }
                else
                {
                    unsolved.Enqueue(index);
                }
            }

            try
            {
                var (ires, jres, price) = prices.EnumerateIndexed().Last(input =>
                {
                    var (i, j, price) = input;
                    return uPotentials[i] + vPotentials[j] > price;
                });
                return (ires, jres);
            }
            catch
            {
                return null;
            }
        }


        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();
            builder.AppendLine("TransportationPlan");
            builder.AppendLine("Matrix:");
            builder.AppendLine(matrix.ToMatrixString());
            builder.AppendLine("Indexes:");
            builder.AppendJoin(", ", indexes);
            builder.AppendLine();
            return builder.ToString();
        }
    }

    public static Matrix<double> SolveTransportation(
        Vector<double> mines,
        Vector<double> factories,
        Matrix<double> prices
    )
    {
        return new TransportationPlan(mines, factories).OptimizeFor(prices);
    }
}