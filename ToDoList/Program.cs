using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BL.Controller;

namespace ToDoList.CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Привет! Это ваш ежедневник");

            Console.WriteLine("Введите имя пользователя");
            var name = Console.ReadLine();

            var userController = new UserController(name);
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
                Console.WriteLine("Что бы выхотели сделать?");
                Console.WriteLine("F - Показать ваш список задач");
                Console.WriteLine("A - Ввести задачу");
                Console.WriteLine("E - Отметить выполненную задачу");
                Console.WriteLine("Z - Статистика");
                Console.WriteLine("Q - выход");
                
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
            
            return new ToDoList.BL.Model.Task(descr, sizeToDoList, endTask);
        }
    }
}
