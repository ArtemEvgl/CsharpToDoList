using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToDoList.BL.Model;

namespace ToDoList.BL.Controller
{
    public class UserController : ControllerBase
    {
       
        public event Action<string> infoMsges;
        
        
        public List<User> UserList { get; }

        public User CurrentUser { get; }
        public bool IsNewUser { get; } = false;

        public UserController (string userName)
        {
            if(string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("Имя пользователя не может быть пустым", nameof(userName));
            }
            UserList = GetUserData();

            CurrentUser = UserList.SingleOrDefault(u => u.Name == userName);
            if(CurrentUser == null)
            {
                CurrentUser = new User(userName);
                UserList.Add(CurrentUser);
                IsNewUser = true;
                //Save("user.xml",UserList);
            }
        }

        public void SetNewUserData(DateTime birthDay)
        {
            CurrentUser.BirthDay = birthDay;
            Save($"users.xml", UserList);
        }

        public void SetNewTask(DateTime dateTime, string descr)
        {
            if (dateTime <= DateTime.Now) throw new ArgumentNullException("Ошибка в дате запланированной задачи, попробуйте еще раз",nameof(dateTime));
            int sizeTaskList = CurrentUser.Tasks.Count;
            CurrentUser.Tasks.Add(new Model.Task(descr, ++sizeTaskList, dateTime));
            Save($"users.xml", UserList);
            infoMsges("Задача успешно добавлена");
        }
        private List<User> GetUserData()
        {
            return Load<List<User>>($"users.xml") ?? new List<User>();
        }

        

        public void SetAcceptTask(int id)
        {
            Model.Task task = CurrentUser.Tasks.FirstOrDefault(i => i.Id == id); //Балуемся с Linq
            if (task != null)
            {
                task.Accept = true;
                task.OnTime = true;
                infoMsges("Задача успешно отмечена");
                if (task.EndTask < DateTime.Now)
                {
                    infoMsges("Вы просрочили задачу, нехорошо...");
                    task.OnTime = false;
                }
                else infoMsges("Задача выполнена в срок, отлично!");
            }
            else
            {
                infoMsges("Задача не найдена в списке, попробуйте еще раз");
                return;
            }           
            task.EndTask = DateTime.Now;
            Save($"users.xml", UserList);
        }

        public void EditTask(int id, DateTime endDateTime, string descr)
        {
            if (endDateTime <= DateTime.Now) throw new ArgumentNullException("Ошибка в дате запланированной задачи, попробуйте еще раз", nameof(endDateTime));
            if (CurrentUser.Tasks.First(i => i.Id == id) is ToDoList.BL.Model.Task task)
            {
                task.EndTask = endDateTime;
                task.Description = descr;
                Save($"users.xml", UserList);
                Console.WriteLine("Задача успешно отредактирована");
            }
            else throw new ArgumentNullException("Вы ошиблись при вводе номера задачи, попробуйте еще раз.");
            
        }

        public void GetStats()
        {
            List<Model.Task> tasks = CurrentUser.Tasks;
            double acceptTasks = tasks.Where(t => t.Accept == true).ToList().Count;
            double percentAcceptTasks = acceptTasks / (double)tasks.Count * 100;
            double percentAcceptTasksOnTime = tasks.Where(t => t.OnTime == true).ToList().Count / acceptTasks * 100;
            infoMsges(String.Format("Процент выполненных задач:{0:f3}", percentAcceptTasks));
            infoMsges(String.Format("Процент выполненных задач в срок: {0:f3}", percentAcceptTasksOnTime));
        }
    }
}
