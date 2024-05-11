Міністерство освіти і науки України
Національний технічний університет України «Київський політехнічний
інститут імені Ігоря Сікорського»
Факультет інформатики та обчислювальної техніки

Кафедра інформатики та програмної інженерії


Звіт

з лабораторної роботи № 2
з дисципліни
«Алгоритми та структури даних. Частина 2. Структури даних»

«Метод декомпозиції. Пошук інверсій»





 
Виконав(ла) 


Перевірив


ІП-33 Цапурда Є. Д.
(шифр, прізвище, ім'я, по батькові)
	
Соколовський В. В
(прізвище, ім'я, по батькові)

 



Київ 2024
Мета:
Дослідити як працює метод декомпозиції, та навчитися шукати інверсії за допомогою нього.
Постановка задачі:
За допомогою методу декомпозиції  розробити алгоритм, який буде розв’язувати наступну задачу.  
Вхідні дані. Матриця D натуральних чисел розмірності u*m, де u — ці кількість користувачів, m — кількість фільмів.  Кожний елемент матриці  D[i,  j] вказує на позицію фільму  j  в списку вподобань користувача  i. Іншим вхідним елементом є  x  — номер користувача, з яким будуть порівнюватись всі інші користувачі. 
Вихідні дані. Список з впорядкованих за зростанням другого елементу пар (i, c), де i — номер користувача, c — число, яке вказує на степінь схожості вподобань користувачів x та c (кількість інверсій).
Псевдокод алгоритму: 
SortAndCountInv(A) 
n ← length(A) 
if n=1  
then return (A, 0) 
else    (L, x) ← SortAndCountInv(перша половина A) 
(R, y) ← SortAndCountInv(друга половина A) 
(A, z) ← MergeAndCountSplitInv(A, L, R) 
return (A, x+y+z)

MergeAndCountSplitInv(A, L, R)
n1 ← length(L) 
n2 ← length(R) 
L[n1+1] ← ∞ 
R[n2+1] ← ∞ 
i ← 0 
j ← 0
c ← 0    
for k ← 0 to length(A) 
if L[i] ≤ R[j] 
then A[k] ← L[i] i ← i + 1 
else A[k] ← R[j] 
j ← j + 1 
c ← c + (n2 – i) 
return (A, c)
Розрахунок складності та аналіз алгоритму:
T(n) = {█(Θ(1)              якщо n≤c,@aT(n/b)+D(n)+C(n)  в протилежному випадку.)┤ 
c - деяка заздалегідь відома стала.
a – кількість підзадач, об’єм кожної з яких дорівнює 1/b від об’єму висхідної задачі.
	Розділення. Під час розділення визначається, де знаходиться середина підмасиву. Ця операція триває фіксований час, тому D(n) = Θ(1). 
	Рекурсивний розв’язок. Рекурсивно розв’язуються дві підзадачі, об’єм кожної з яких складає n/2. Час розв’язку цих підзадач становить 2T(n/2). 
	Комбінування. Процедура Merge для n-елементного масиву виконується протягом часу Θ(n), тому C(n) = Θ(n).
T(n)={█(Θ(1)                     якщо n=1@2T(n/2)+ Θ(n)  якщо n>1)┤
T(n)={█( c                        якщо n=1@2T(n/2)+ cn  якщо n>1)┤

 
Рис. 1. Побудова дерева рекурсій для рівняння T(n) = 2T(n/2) + cn
Маючи lgn + 1 рівнів, кожний з яких виконується протягом часу cn, отримуємо загальний час
cn(lgn + 1) = cnlgn + cn
Нехтуючи членами менших порядків та константою c, в результаті отримуємо 
Θ(nlgn)








Властивість	Сортування злиттям 
Стійкість	Стійкий 
«Природність» поведінки	Не адаптивний
Базуються на порівняннях 	Використовує рекурсивний алгоритм для порівняння «лівої» та «правої» частини масиву 
Необхідність в додатковій пам’яті (об’єм)	Додаткова пам'ять потрібна, її кількість дорівнює довжині вхідного масиву
Необхідність в знаннях про структури даних	Вимагає знань про організацію даних в масивах та про рекурсію 

Код програми (C#): 
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

Робота програми:

Вхідні дані:
  
Рис. 2. Файл вхідних даних

Консоль:
 
Рис. 3. Вікно консолі після запуску програми та введення даних

Вихідні дані:
 
Рис. 4. Файл з назвою «input_5_10_output.txt»







Покроковий приклад роботи програми:

Вхідні дані:

№	Порядок у користувача (Х) = 1	Порядок у користувача 2 	Порядок у 3, відносно Х
0	3	4	5
1	1	5	3
2	2	3	4
3	4	2	2
4	5	1	1

	Етап розділення головного масиву:
 

	Етап розділення лівої частини:
 
 




Оброблено всі ліві підмасиви

 

Оброблено правий підмаисв останнього не одиничного лівого підмасиву

 

	Етап порівняння та обʼєднання останніх одиничних підмасивів:

 

	Обробка правого підмасиву від масива [5, 3, 4]
 

	Сортування та підрахунок інверсій в підмасиві [5, 3, 4]
 

	Обробка правого підмасиву від оригінального масиву
 

Так як два масиви одиничні, переходимо від разу до їх сортування.
 

	Маємо два відсортованих підмасиви, переходимо до їх обʼєднання: сортування та підрахунку інверсій.
 

Програма відсортувала масив та порахувала кількість інверсій в ньому. 

	Сумуємо кількість інверсій з відсортованих до цього лівого масиву та правого
 
 
Рис. 5. Другий масив має 9 інверсій

 
Рис. 6. Кількість інверсій в заданому масиві

Висновок:
Під час виконання роботи ми дослідили алгоритм сортування merge sort та за його допомогою обрахували кількість інверсій в заданому масиві. Ми ознайомилися з методом декомпозицій та впевнилися в його ефективності, обрахувавши його складність. Створена програма працює коректно та відповідає меті завдання.

![image](https://github.com/R0BIK/InversionSimilarityFinder/assets/99051328/23ff8bdc-68dd-41e9-9447-13ec6a94bfb0)
