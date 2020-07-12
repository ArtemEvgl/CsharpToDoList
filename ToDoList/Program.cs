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
        //TODO: Замечен небольшой баг при редактикровании и сделать Статистику. Далее с тестами. База.
        static void Main(string[] args)
        {
            Console.WriteLine("Привет! Это ваш ежедневник");

            Console.WriteLine("Введите имя пользователя");
            var name = Console.ReadLine();

            var userController = new UserController(name);            
            userController.infoMsges += (msg) => DisplayMsg(msg);
             
            while (userController.IsNewUser)
            {
                Console.Write("Введите дату рождения: ");
                var bithDate = ParseDate("дату рождения");
                try
                {
                    userController.SetNewUserData(bithDate);
                } catch (ArgumentNullException)
                {
                    Console.WriteLine("Ошибка при вводе даты рождения, попробуйте еще раз");
                    continue;
                }
                Console.WriteLine($"Вы успешно зарегистрированы {userController.CurrentUser.ToString()}");
                break;
            }
            Console.WriteLine("Добро пожаловать");
            
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
                        userController.CurrentUser.Tasks.ForEach(t => Console.WriteLine(t.ToString()));
                        break;
                    case ConsoleKey.A:
                        var (endTask, descr) = EnterTask();
                        try
                        {
                            userController.SetNewTask(endTask, descr);
                        } catch(ArgumentNullException e)
                        {
                            Console.WriteLine($"Ошибка в {e.Message}");
                        }
                        break;
                    case ConsoleKey.E:
                        if(userController.CurrentUser.Tasks.Where(t => t.Accept == false).ToList().Count == 0)
                        {
                            Console.WriteLine("У вас нет открытых задач");
                            break;
                        }
                        userController.CurrentUser.Tasks.Where(t => t.Accept == false).ToList().ForEach(t=> Console.WriteLine(t.ToString()));                                                
                        Console.WriteLine("Введите Id задачи, которую хотите закрыть");
                        Int32.TryParse(Console.ReadLine(), out int acceptId);
                        userController.SetAcceptTask(acceptId);
                        break;
                    case ConsoleKey.P:
                        while (true)
                        {
                            userController.CurrentUser.Tasks.ForEach(t => Console.WriteLine(t.ToString()));
                            Console.WriteLine("Введите Id задачи, которую хотите откредактировать");
                            Int32.TryParse(Console.ReadLine(), out int editId);
                            var (endEditTask, editDescr) = EnterTask();
                            try
                            {
                                userController.EditTask(editId, endEditTask, editDescr);
                            } catch (ArgumentNullException ex)
                            {
                                Console.WriteLine($"Ошибка в {ex.Message}");
                                continue;
                            }
                            break;
                        }
                        break;
                    case ConsoleKey.Z:
                        userController.GetStats();
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }            
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

        
        private static (DateTime date, string descr) EnterTask()
        {
            Console.Write("Введите описание задачи: ");
            string descr = Console.ReadLine();
            DateTime endTask = ParseDate("планируемую дату окончания задачи");
            return (endTask, descr);
        }

        private static void DisplayMsg(string msg)
        {
            Console.WriteLine("\n*****************");
            Console.WriteLine($"=> {msg}");
            Console.WriteLine("*****************\n");
        }

        
    }
}
