using System;

namespace NumberSystem
{
    public static class Number
    {
        private static String _number; //Введённое пользователем число
        private static String _intNumber; //Целая часть числа
        private static String _floatNumber; //Дробная часть числа
        private static int schis; //Система счисления введённого числа
        private static int schis2; //Система счисления для перевода
        private const int MinSchis = 2; //Минимальное значение системы счисления
        private const int MaxSchis = 36; //Максимальное значение системы счисления

        //Функция перевода символьного значения в целочисленное
        private static int CharToInt(char simvol) //simvol - символьное значение числа
        {
            if (simvol >= '0' && simvol <= '9') return simvol - '0';
            else
            {
                if (simvol >= 'A' && simvol <= 'Z') return simvol - 'A' + 10;
                else return 100000;
            }
        }
        //Функция перевода целочисленного значения в символьное
        private static char IntToChar(int numbers) //numbers - целочисленное значение числа
        {
            if (numbers <= 9) return (char)(numbers + '0');
            else return (char)(numbers + 55);
        }

        //Функция проверки коррекnности введённого числа
        private static bool IsNumberCorrect()
        {
            int length = _number.Length; //Длина числа
            int countPoint = 0; //Количество точек в числе

            if (_number[0] == '.' || _number[length - 1] == '.') return false;

            for (int i = 0; i < length; i++)
            {
                if (countPoint > 1) return false;
                if (_number[i] == '.')
                {
                    countPoint++;
                    continue;
                }
                if (CharToInt(_number[i]) >= schis) return false;
            }

            return true;
        }

        //Функция проверки корректности введенных систем счисления.
        private static bool IsSchisCorrect()
        {
            if ((schis > MaxSchis || schis < MinSchis) || (schis2 > MaxSchis || schis2 < MinSchis))
            {
                return false;
            }
            return true;
        }

        //Функция перевода целой части числа в десятичную систему счисления
        private static int TranslateIntegerToDecimal()
        {
            int decimalNumber = 0, //Хранит число в десятичной системе счисления
                index = 1; //Хранит степень для возведения каждой цифры числа
            int count = _intNumber.Length; //Хранит длину целочисленной части числа
            for (int i = count - 1; i >= 0; i--)
            {
                int bufNumber = CharToInt(_intNumber[i]);
                decimalNumber += (bufNumber * index);
                index *= schis;
            }
            return decimalNumber;
        }

        //Функция перевода дробной части числа в десятичную систему счисления
        private static double TranslateFloatToDecimal()
        {
            double floatNumber = 0.0; //Хранит число в десятичной системе счисления
            double index = -1; //Хранит степень для возведения каждой цифры числа
            int count = _floatNumber.Length; //Хранит длину целочисленной части числа
            double bufSchis = schis; //Хранит систему счисления, в которую нужно перевести число.
            for (int i = 0; i < count; i++)
            {
                double bufNumber = CharToInt(_floatNumber[i]);
                floatNumber = floatNumber + (bufNumber * Math.Pow(bufSchis, index));
                index--;
            }
            return floatNumber;
        }

        //Функция перевода буквы в число и числа в букву (значение числа записано в системе счисления > 9)
        private static char TranslateToMoreSystems(char number) //number - значение числа в системе счисления > 9
        {
            int[] arrOfNumbers = new int[] //Массив чисел в системе счисления > 9
            { 
                10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22,
                23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35
            };
            char[] arrOfSimvols = new char[] //Массив букв, соответсвующих значениям чисел из предыдущего массива
            { 
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            bool isCheckSimvol = false; //Проверяет, является ли введённое значение буквой
            int indexNecessaryElement = 0; //Индекс необходиого значения из массивов
            for (int i = 0; i < arrOfNumbers.Length; i++)
            {
                if (IntToChar(arrOfNumbers[i]) == number)
                {
                    indexNecessaryElement = i;
                    isCheckSimvol = false;
                    break;
                }

                if (arrOfSimvols[i] == number)
                {
                    indexNecessaryElement = i;
                    isCheckSimvol = true;
                    break;
                }
            }
            if (isCheckSimvol == false) return arrOfSimvols[indexNecessaryElement];
            else return IntToChar(arrOfNumbers[indexNecessaryElement]);
        }

        //Функция перевода дробной части числа в систему счисления _schis2
        private static char[] TranslateFractionalPart()
        {
            int digit = 0, //Хранит цифры из дробной части
                index = 0,
                size = 50;
            char[] arrNumbers = new char[size]; //Массив цифр из дробной части в системе счисления _schis2
            double floatNumbers = TranslateFloatToDecimal(); //Перевод дробной части в десятичную систему счисления
            int sizeFloatNumbers = 0; //Хранит длину дробной части в десятичной системе счисления
            double bufNumber = floatNumbers;
            
            //Вычисление размера дробной части в десятичной системе счисления
            while (bufNumber % 10 != 0)
            {
                bufNumber *= 10;
                sizeFloatNumbers++;
            }

            sizeFloatNumbers--;

            //Перевод дробной части в систему счисления _schis2
            while (index < 10)
            {
                floatNumbers *= schis2;
                digit = (int)floatNumbers;
                arrNumbers[index] = IntToChar(digit);
                floatNumbers -= digit;
                index++;
            }
            arrNumbers[index] = '\0';
            return arrNumbers;
        }

        //Функция перевода числа в систему счисления _schis2
        public static char[] TranslateToSystemNumbers(string stringNumbers, int firstSchis, int secondSchis)
        {
            //stringNumbers - введённое число 
            //firstSchis - система счисления введённого числа
            //secondSchis - система счисления для перевода числа
            _number = stringNumbers;
            schis = firstSchis;
            schis2 = secondSchis;
            char[] nullArrNumbers = { '\0' }; //Хранит пустой массив символов. Возвращается в случае некорректности записи числа, несоответсвии числа с системой счисления
            if (IsSchisCorrect() == false)
            {
                Console.WriteLine("Система счисления не может быть меньше 2 или больше 36!");
                return nullArrNumbers;
            }
            if (IsNumberCorrect() == false)//Проверка на корректность записи введённого числа
            { 
                Console.WriteLine("Число некорректно!");
                return nullArrNumbers;
            }
            else
            {
                int j = 0;
                bool checkPoint = false; //Проверяет, есть ли в числе дробная часть

                //Разбиваем число на целую и дробную части
                while (j != stringNumbers.Length)
                {
                    if (stringNumbers[j] == '.')
                    {
                        checkPoint = true;
                        j++;
                        continue;
                    }
                    else
                    {
                        if (checkPoint == false)
                            _intNumber += stringNumbers[j];
                        else
                        {
                            _floatNumber += stringNumbers[j];
                        }

                        j++;
                    }
                }

                int count = 0; //Хранит количество цифр в записи числа в системе счисления _schis2
                int decNumbers = TranslateIntegerToDecimal(); //Записываем число в десятичной системе счисления
                int bufDecNumbers = decNumbers; //Переменная-буфер для хранения копии десятичной записи числа, чтобы не изменять значение decNumbers

                //Вычисление количества цифр в записи числа в системе счисления _schis2
                while (bufDecNumbers > 0)
                {
                    bufDecNumbers /= schis2;
                    count++;
                }

                char[] arrNumbers = new char[count]; //Массив цифр в системе счисления _schis2
                char[] resultString = new char[100]; //Результат в виде числа в системе счисления _schis2
                int index = 0;

                //Перевод числа в систему счисления _schis2
                while (decNumbers > 0)
                {
                    arrNumbers[index] = IntToChar(decNumbers % secondSchis);
                    if (CharToInt(arrNumbers[index]) > 9) arrNumbers[index] = TranslateToMoreSystems(arrNumbers[index]);
                    decNumbers /= schis2;
                    index++;
                }

                index = 0;

                //Запись числа в системе счисления _schis2 в корректный вид
                for (int i = 0; i < count; i++)
                {
                    resultString[i] = arrNumbers[count - 1 - i];
                    index++;
                }

                if (checkPoint == true)
                {
                    char[] floatNumbers = new char[50]; //Массив цифр из дробной части в системе счисления _schis2
                    int sizeFloatNumbers = 0; //Хранит размер дробной части
                    floatNumbers = TranslateFractionalPart(); //Перевод дробной части в систему счисления _schis2

                    resultString[index] = '.';

                    //Вычисление размера дробной части
                    while (floatNumbers[sizeFloatNumbers] != '\0') sizeFloatNumbers++;

                    int ind = 0;

                    //Присоединение дробной части в системе счисления _schis2 к результату из целой части
                    for (int i = index + 1; i < sizeFloatNumbers + index + 1; i++)
                    {
                        resultString[i] = floatNumbers[ind];
                        ind++;
                    }
                }
                return resultString;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите систему счисления числа: ");
            int firstSchis = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите число: ");
            string stringNumbers = Console.ReadLine();

            Console.Write("Введите систему числения, в которую нужно перевести число: ");
            int secondSchis = Convert.ToInt32(Console.ReadLine());

            char[] result = Number.TranslateToSystemNumbers(stringNumbers, firstSchis, secondSchis);

            Console.WriteLine(result);
        }
    }
}
