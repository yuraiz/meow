public static class PrintExt
{

    public static void PrettyPrintAsColumn<T>(this T[] column, string name = "column")
    {
        Console.WriteLine($"{name} = [");
        foreach (var item in column)
        {
            Console.WriteLine($"  {item:#0.####},");
        }
        Console.WriteLine(']');
    }

    public static void PrettyPrintAsRow<T>(this T[] row, string name = "row")
    {
        Console.Write($"{name} = [");
        for (var i = 0; i < row.Length - 1; i++)
        {
            Console.Write($"{row[i]:#0.####}, ");
        }
        Console.WriteLine($"{row.Last():#0.####}]");
    }

    public static void PrettyPrint<T>(this T[,] matrix, string name = "matrix")
    {
        Console.WriteLine($"{name} = [");
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            Console.Write("  ");
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]:#0.####}, ");
            }
            Console.WriteLine();
        }
        Console.WriteLine(']');
    }
}