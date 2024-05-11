using System.Reflection;

class InversionSimilarityFinder
{
    static private int x;
    static private int films;
    static private int users;
    static private List<int> listX;
    static private List<(int, int)> Result = new List<(int, int)>();
    
    static void Main()
    {
        try
        { 
            Console.WriteLine("Введіть назву файлу у форматі 'file_name.txt'");

            string fileName = Console.ReadLine();

            Console.WriteLine("Введіть X");

            x = int.Parse(Console.ReadLine());

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            string programDirectory = Path.GetDirectoryName(assemblyLocation);

            string filePath = Path.Combine(programDirectory, fileName);

            string[] lines = File.ReadAllLines(filePath);

            string firstLine = lines[0];
            int[] numbers = firstLine.Split(' ').Select(int.Parse).ToArray();

            users = numbers[0];
            films = numbers[1];
            listX = lines[x].Split(' ').Select(int.Parse).ToList();


            for (int i = 1; i <= users; ++i)
            {
                if (i == x)
                    continue;
                Result.Add((i, SortAndCountInv(listForCount(lines[i].Split(' ').Select(int.Parse).ToList())).Item2));
            }

            Result.Sort((z, y) =>
            {
                int SortRes = z.Item2.CompareTo(y.Item2);
                if (SortRes == 0)
                    return z.Item1.CompareTo(y.Item1);
                return SortRes;
            });
            
            string fileNameOutput = fileName.Replace(".txt", "_output.txt");
            string filePathOutput = Path.Combine(programDirectory, fileNameOutput);
            
            using (StreamWriter writer = new StreamWriter(filePathOutput))
            {
                writer.WriteLine(x);
                for (int i = 0; i < users - 1; ++i)
                {
                    writer.WriteLine($"{Result[i].Item1} {Result[i].Item2}");
                }
            }
        }
        catch
        {
            Console.WriteLine("Помилка");
        }
    }
    
    static List<int> listForCount(List<int> list)
    {
        List<int> newList = list.GetRange(1, films);

        for (int i = 1; i <= films; ++i)
            newList[listX[i] - 1] = list[i];

        return newList;
    }

    static (List<int>, int) SortAndCountInv(List<int> list)
    {
        if (list.Count == 1)
            return (list, 0);
        var halfs = HalfMass(list);
        var left = SortAndCountInv(halfs.Item1);
        var right = SortAndCountInv(halfs.Item2);
        var listA = MergeAndCountSplitInv(list, left.Item1, right.Item1);

        return (list, left.Item2 + right.Item2 + listA.Item2);
    }

    static (List<int>, int) MergeAndCountSplitInv(List<int> A, List<int> L, List<int> R)
    {
        int n1 = L.Count;
        int n2 = R.Count;
        
        L.Add(Int32.MaxValue);
        R.Add(Int32.MaxValue);

        int i = 0;
        int j = 0;
        int c = 0;

        for (int k = 0; k < A.Count; ++k)
        {
            if (L[i] <= R[j])
            {
                A[k] = L[i];
                ++i;
            }
            else
            {
                A[k] = R[j];
                ++j;
                c += n1 - i;
            }
        }
        
        return (A, c);
    }

    static (List<int>, List<int>) HalfMass(List<int> mass)
    {
        int afterMid = mass.Count() / 2;
        int mid = mass.Count() - afterMid;

        List<int> half1 = mass.GetRange(0, mid);
        List<int> half2 = mass.GetRange(mid, afterMid);

        return (half1, half2);
    }
}