using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.IO.Pipes;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Tests
{
    class Program
    {

        public static List<Zayavka> arr = new List<Zayavka>();
        static void Main(string[] args)
        {
            Program c = new Program();

            c.Menu();


        }
        public void Menu()
        {

            Console.WriteLine("Команды:\n Нажмите 1, чтобы создать запись\n Нажмите 2, чтобы редактировать запись\n Нажмите 3 для удаления\n 4 - все записи\n 5 - все записи кратко\n 6 - Exit");
            int a = Convert.ToInt32(Console.ReadLine());
            Program c = new Program();
            while (a != 6)
            {
                if (a == 1)
                {
                    c.Create();
                }
                else if (a == 2)
                {
                    c.Edit();
                }
                else if (a == 3)
                {
                    c.Delete();
                }
                else if (a == 4)
                {
                    c.AllNotes();
                }
                else if (a == 5)
                {
                    c.ShortNotes();
                }

                Console.WriteLine("Команды:\n Нажмите 1, чтобы создать запись\n Нажмите 2, чтобы редактировать запись\n Нажмите 3 для удаления\n 4 - все записи\n 5 - все записи кратко\n 6 - Exit");
                a = Convert.ToInt32(Console.ReadLine());

            }


        }
        public void Create()
        {

            Console.Write("Фамилия: ");
            string surname = Console.ReadLine();
            Console.Write("Имя: ");
            string name = Console.ReadLine();
            Console.Write("Страна: ");
            string country = Console.ReadLine();
            Console.Write("Введите номер телефона: ");

            
            bool checkSymbol = false;

            string phoneNum = Console.ReadLine().Trim();
            while (checkSymbol == false)
            {
                bool isNum = true;
                for (int i = 0; i < phoneNum.Length; i++)
                {
                    if (Char.IsNumber(phoneNum, i) == false)                                    //Проверяет каждый символ на цифру
                    {
                        
                        isNum = false;
                        
                    }
                   
                }
                if (isNum == true)
                {
                    checkSymbol = true;
                }
                else
                {
                    Console.WriteLine("В строке есть ненужные символы, попробуйте ввести еще раз");
                    phoneNum = Console.ReadLine().Trim();
                }
                
            }

            Zayavka zayavka = new Zayavka(surname, name, country, phoneNum);
            arr.Add(zayavka);

            Console.WriteLine("Запись создана");
            
            MoreInfo();




        }
        public void MoreInfo()
        {
            Console.WriteLine("Хотите добавить больше информации?(да/нет)");
            int last = arr.Count-1;                                                                     //Позволяет обратиться к только созданному элементу(индекс)
            bool checkanswer = false;
            while (checkanswer == false)
            {
                string answer = Console.ReadLine();
                if (answer.ToLower() == "да")
                {
                    Console.WriteLine("Выберите пункты, которые вы хотите добавить и напишите их в именительном падеже через запятую:\nОтчество\nДата рождения\nОрганизация\nДолжность\nПрочее");
                    string moreInfo = Console.ReadLine();
                    moreInfo = moreInfo.ToLower().Trim();
                    string[] result = moreInfo.Split(",");

                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = result[i].Trim();
                    }
                    foreach (var item in result)
                    {
                        if (item == "отчество")
                        {
                            EditLastname(last);
                        }
                        else if (item == "дата рождения")
                        {
                            EditDateTime(last);
                        }
                        else if (item == "организация")
                        {
                            EditOrganization(last);
                        }
                        else if (item == "должность")
                        {
                            EditStatus(last);
                        }
                        else if (item == "прочее")
                        {
                            EditOther(last);
                        }
                    }


                    checkanswer = true;
                }
                else if (answer.ToLower() == "нет")
                {

                    checkanswer = true;
                }
                else
                {
                    Console.WriteLine("Некорректный ответ");
                }
            }
        }
        public void Edit()
        {
            bool checkexist = false;
            int num = 0;
            while (checkexist == false)
            {
                Console.WriteLine("Выберите номер записи:");
                for (int i = 0; i < arr.Count; i++)
                {
                    Console.WriteLine("Номер элемента: " + i);
                }
                num = Convert.ToInt32(Console.ReadLine());
                if (num >= 0 && num < arr.Count)
                {
                    Console.WriteLine($"{arr[num]}");
                    checkexist = true;
                }
                else
                {
                    Console.WriteLine("Такого элемента нет");
                }
            }
            Console.WriteLine("Выберите пункты, который вы хотите отредактировать и напишите их в именительном падеже через запятую:\nФамилия\nИмя\nОтчество\nНомер телефона\nСтрана\nДата рождения\nОрганизация\nДолжность\nПрочее");
            string moreInfo = Console.ReadLine();
            moreInfo = moreInfo.ToLower().Trim();
            string[] result = moreInfo.Split(",");
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = result[i].Trim();
            }
            foreach (var item in result)
            {
                
                if (item == "отчество")
                {
                    EditLastname(num);
                }
                else if (item == "номер телефона")
                {
                    EditPhoneNumber(num);
                }
                else if (item == "дата рождения")
                {
                    EditDateTime(-1);
                }
                else if (item == "организация")
                {
                    EditOrganization(num);
                }
                else if (item == "должность")
                {
                    EditStatus(-1);
                }
                else if (item == "прочее")
                {
                    EditOther(num);
                }
                else if (item == "имя")
                {
                    EditName(num);
                }
                else if (item == "фамилия")
                {
                    EditSurname(num);
                }
                else if (item == "страна")
                {
                    EditCountry(num);
                }
            }





        }
        public void EditSurname(int num)
        {
            Console.Write("Введите фамилию: ");
            string value = Console.ReadLine();
            arr[num].Surname = value;
        }
        public void EditName(int num)
        {
            Console.Write("Введите имя: ");
            string value = Console.ReadLine();
            arr[num].Name = value;
        }
        public void EditLastname(int num)
        {
            Console.Write("Введите отчество: ");
            string value = Console.ReadLine();
            arr[num].Lastname = value;
        }
        public void EditPhoneNumber(int num)
        {
            Console.Write("Введите номер телефона: ");
            bool isNum = true;
            bool checkSymbol = false;

            string phoneNum = Console.ReadLine().Trim();
            while (checkSymbol == false)
            {

                for (int i = 0; i < phoneNum.Length; i++)
                {
                    if (Char.IsNumber(phoneNum, i) == false)
                    {

                        isNum = false;

                    }

                }
                if (isNum == true)
                {
                    checkSymbol = true;
                }
                else
                {
                    Console.WriteLine("В строке есть ненужные символы, попробуйте ввести еще раз");
                    phoneNum = Console.ReadLine().Trim();
                }

            }
            arr[num].PhoneNumber = phoneNum;
        }
        public void EditCountry(int num)
        {
            Console.Write("Введите страну: ");
            string value = Console.ReadLine();
            arr[num].Country = value;
        }
        public void EditDateTime(int num)
        {
            Console.Write("Введите дату рождения: ");
            int year = 1999;
            int month = 8;
            int day = 11;
            bool checkyear = false;
            bool checkmonth = false;
            bool checkday = false;
            while (checkyear == false)
            {
                Console.Write("Введите год: ");
                year = Convert.ToInt32(Console.ReadLine());
                if (year > 2020 || year < 1800)
                {
                    Console.WriteLine("Вы либо из будущего, либо из прошлого!");
                }
                else if (year < 2020 && year > 1800)
                {
                    checkyear = true;
                }
                else
                {
                    Console.WriteLine("Год введен некорректно");
                }
            }
            while (checkmonth == false)
            {
                Console.Write("Введите месяц: ");
                month = Convert.ToInt32(Console.ReadLine());
                if (month > 13 || month < 0)
                {
                    Console.WriteLine("Такого месяца нет");
                }
                else if (month < 13 && month > 0)
                {

                    checkmonth = true;
                }
                else
                {
                    Console.WriteLine("Месяц введен некорректно");
                }
            }
            while (checkday == false)
            {
                Console.Write("Введите день: ");
                day = Convert.ToInt32(Console.ReadLine());
                if (day < 32 && day > 0)
                {
                    switch (month)
                    {
                        case 1:
                            if (day <= 31 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 2:
                            if (day <= 28 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 3:
                            if (day <= 31 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 4:
                            if (day <= 30 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 5:
                            if (day <= 31 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 6:
                            if (day <= 30 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 7:
                            if (day <= 31 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 8:
                            if (day <= 31 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 9:
                            if (day <= 30 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 10:
                            if (day <= 31 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 11:
                            if (day <= 30 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                        case 12:
                            if (day <= 31 && day >= 1)
                            {
                                checkday = true;
                            }
                            else
                            {
                                Console.WriteLine("День введен некорректно");
                            }
                            break;
                    }
                }

                else
                {
                    Console.WriteLine("День введен некорректно");
                }

            }
            DateTime dateTime = new DateTime(year, month, day);
            arr[num].DateTime = dateTime;
        }
        public void EditOrganization(int num)
        {
            Console.Write("Введите организацию: ");
            string value = Console.ReadLine();
            arr[num].Organization = value;
        }
        public void EditStatus(int num)
        {
            Console.Write("Введите должность: ");
            string value = Console.ReadLine();
            arr[num].Status = value;
        }
        public void EditOther(int num)
        {
            Console.Write("Введите прочее: ");
            string value = Console.ReadLine();
            arr[num].Other = value;
        }
        public void Delete()
        {
            bool checkexist = false;
            int num = 0;
            while (checkexist == false)
            {
                Console.WriteLine("Выберите номер элемента, который хотите удалить:");
                for (int i = 0; i < arr.Count; i++)
                {
                    Console.WriteLine("Номер элемента: " + i);
                }
                num = Convert.ToInt32(Console.ReadLine());
                if (num >= 0 && num < arr.Count)
                {
                    Console.WriteLine($"Краткая информация: {arr[num]}");
                    checkexist = true;
                }
                else
                {
                    Console.WriteLine("Такого элемента нет");
                }
            }
            arr.RemoveAt(num);
            Console.WriteLine("Элемент удален");

        }
        public void AllNotes()
        {
            bool checkexist = false;
            int num = 0;
            while (checkexist == false)
            {
                Console.WriteLine("Выберите номер записи:");
                for (int i = 0; i < arr.Count; i++)
                {
                    Console.WriteLine("Номер элемента: " + i);
                }
                num = Convert.ToInt32(Console.ReadLine());
                if (num >= 0 && num < arr.Count)
                {
                    Console.WriteLine($"{arr[num]}");
                    checkexist = true;
                }
                else
                {
                    Console.WriteLine("Такого элемента нет");
                }
            }
            Console.WriteLine($"Фамилия: {arr[num].Surname}\nИмя: {arr[num].Name}\nОтчество: {arr[num].Lastname}\nНомер телефона: {arr[num].PhoneNumber}\nСтрана: {arr[num].Country}\nДата рождения: {arr[num].DateTime}\nОрганизация: {arr[num].Organization}\nДолжность: {arr[num].Status}\nПрочее: {arr[num].Other}\n");
        }

        public void ShortNotes()
        {
            for (int i = 0; i < arr.Count; i++)
            {
                Console.WriteLine($"Фамилия: {arr[i].Surname}\nИмя: {arr[i].Name}\nНомер телефона: {arr[i].PhoneNumber}\n");
            }
        }
        public class Zayavka
        {
            
            public string Surname { get; set; }
            public string Name { get; set; }
            public string Lastname { get; set; } = "Unknown";
            public string PhoneNumber { get; set; }
            public string Country { get; set; }
            public DateTime DateTime { get; set; }
            public string Organization { get; set; } = "Unknown";
            public string Status { get; set; } = "Unknown";
            public string Other { get; set; } = "Unknown";
            public override string ToString()
            {
                return Surname + " " + Name + " " + Country;
            }
            public Zayavka(string surname, string name, string country, string phone)
            {
                
                Surname = surname;
                Name = name;
                Country = country;
                PhoneNumber = phone;

            }

        }
    }
}
