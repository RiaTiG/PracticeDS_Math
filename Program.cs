using System;
using System.IO;
using System.Text;

class Program
{
    static StringBuilder builder = new StringBuilder(); // создаем новый экземпляр класса, StringBuilder - предоставляет изменяемую последовательность символов
    static string posX = " X";
    static string negX = " ¬X";
    static string multi = " *";
    static string symbol = " V";

    static void FillIn(int[,] arr)
    {
        int length = arr.GetUpperBound(1) + 1;  // кол-во столбцов в массиве (кол-во Х)

        for (int i = 0; i < arr.GetUpperBound(0) + 1; i++) // строки
        {
            int period = length / (int)Math.Pow(2, i + 1);  // период постановки 0 и 1
            int temp = 0;      // временная переменная подсчета кол-ва 0 и 1 перд которыми нужно сменить направление 
            bool reverse = false;
            for (int j = 0; j < arr.GetUpperBound(1) + 1; j++)  //столбцы
            {
                if (reverse) //понижаем счетчик периода и заполняем 1, пока он не станет = 0  (22 строка)
                {
                    arr[i, j] = 1;
                    temp--;
                    if (temp <= 0)
                        reverse = !reverse;
                }
                else     // увеличиваем счетчик периода и заполняем 0, пока он не станет = периоду (29 строка)
                {
                    arr[i, j] = 0;
                    temp++;
                    if (temp >= period)
                        reverse = !reverse;
                }
                //Console.Write(arr[i, j] + " ");
            }
            //Console.WriteLine("//");
        }
    }
    static void WriteData()   //Запись в файл
    {
        File.WriteAllText("N.txt", builder.ToString());
    }
    static int Num(int n)
    {
        return (int)Math.Log2(n);    // вычисление log2 (нахождение степени 2-ки)
    }
    static void CheckLast()  // рекурсивный цикл который проверяет последние элементы
    {
        if (builder[builder.Length - 1] == '*' || builder[builder.Length - 1] == 'V')
        {
            builder.Remove(builder.Length - 1, 1);
            CheckLast();
        }
    }
    static void Calculate(string str)
    {
        double temp = Math.Log2(str.Length);            // дробная переменная temp = степени 2-ки 
        if (temp - Math.Truncate(temp) != 0)            // разность temp и целого значения числа 
            return;                                     //если не равно 0 , то выход
        int[,] Array2 = new int[Num(str.Length), str.Length];    // двумерный массив = [степень 2-ки, длина строки]

        FillIn(Array2);  // заполняем массив

        for (int i = 0; i < Array2.GetUpperBound(1) + 1; i++) // проходим по столбцам 
        {
            int indexX = 1;
            if (str[i] != '1') continue;  // проверка если элемент не равен 1, то пропускаем

            for (int j = 0; j < Array2.GetUpperBound(0) + 1; j++)  //проходими по строчкам
            {
                if (j != Array2.GetUpperBound(0))  // проверяем если текущая поз. не равна макс по строчкам, то...
                    builder.Append(Array2[j, i] == 0 ? negX + indexX + multi : posX + indexX + multi);
                else
                    builder.Append(Array2[j, i] == 0 ? negX + indexX : posX + indexX);
                //Console.Write(Array2[j, i] + " ");
                indexX++;
            }
            if (i != Array2.GetUpperBound(1)) // если текущ поз не равна последнему столбцу, то мы...
                builder.Append(symbol);

            //Console.WriteLine("//");
        }
        CheckLast();  // проверяем на лишние элементы в конце
        builder.AppendLine();  // переход новую строку
    }

    static void Main(string[] args)
    {
        string[] Array = File.ReadAllLines("F.txt");    // чтение всех элементов файла и добавление в массив строк
        for (int i = 0; i < Array.Length; i++)
        {
            Calculate(Array[i]);
        }
        WriteData();
    }
}