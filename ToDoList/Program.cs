using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BL.Controller;
using ToDoList.BL.Model;

namespace ToDoList.CMD
{
    class Program
    {
        //TODO: Проверка возраста, проверка даты планирования окончания и просроченности задачи
        static void Main(string[] args)
        {
            Console.WriteLine("Привет! Это ваш ежедневник");

            Console.WriteLine("Введите имя пользователя");
            var name = Console.ReadLine();

            var userController = new UserController(name);
            userController.RegisterDisplayDelegate(new UserController.DisplayDelegateInfo(DisplayMsg));
            if (userController.IsNewUser)
            {
                Console.Write("Введите дату рождения: ");
                var bithDate = ParseDate("дату рождения");
                userController.setNewUserData(bithDate);
                Console.WriteLine($"Вы успешно зарегистрированы {userController.CurrentUser.ToString()}");
            } else
            {
                Console.WriteLine("Добро пожаловать");
            }
            while(true)
            {
                Console.WriteLine("\n*****************");
                Console.WriteLine("Что бы выхотели сделать?");
                Console.WriteLine("F - Показать ваш список задач");
                Console.WriteLine("A - Ввести задачу");
                Console.WriteLine("E - Отметить выполненную задачу");
                Console.WriteLine("P - Редактировать задачу");
                Console.WriteLine("Z - Статистика");
                Console.WriteLine("Q - выход");
                Console.WriteLine("*****************\n");

                var key = Console.ReadKey();
                Console.WriteLine();
                switch(key.Key)
                {
                    case ConsoleKey.F:
                        foreach(ToDoList.BL.Model.Task task in userController.CurrentUser.Tasks)
                        {
                            Console.WriteLine(task.ToString());
                        }
                        break;
                    case ConsoleKey.A:
                        ToDoList.BL.Model.Task newTask = EnterTask(userController.CurrentUser.Tasks.Count);
                        userController.setNewTask(newTask);
                        break;
                    case ConsoleKey.E:
                        foreach(ToDoList.BL.Model.Task task in userController.CurrentUser.Tasks)
                        {
                            if (task.Accept != true) Console.WriteLine(task.ToString());                            
                        }
                        Console.WriteLine("Введите Id задачи, которую хотите закрыть");
                        Int32.TryParse(Console.ReadLine(), out int id);
                        userController.SetAcceptTask(id);
                        break;
                    case ConsoleKey.Z:
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
            Console.ReadLine();
        }

        private static DateTime ParseDate(string question)
        {
            DateTime birthDate;
            while (true)
            {
                Console.WriteLine($"Введите {question} (dd.MM.yyyy)");
                if (DateTime.TryParse(Console.ReadLine(), out birthDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Неверный формат {question}");
                }

            }

            return birthDate;
        }

        private static ToDoList.BL.Model.Task EnterTask(int sizeToDoList)
        {
            Console.Write("Введите описание задачи: ");
            string descr = Console.ReadLine();
            DateTime endTask = ParseDate("планируемую дату окончания задачи");
            
            return new ToDoList.BL.Model.Task(descr, ++sizeToDoList, endTask);
        }

        public static void DisplayMsg(string msg)
        {
            Console.WriteLine("\n*****************");
            Console.WriteLine($"=> {msg}");
            Console.WriteLine("*****************\n");
        }
    }
}
