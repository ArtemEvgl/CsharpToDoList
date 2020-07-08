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

        //public delegate void DisplayDelegateInfo (string msg);
        //private DisplayDelegateInfo delegateHandler;
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

        public void setNewUserData(DateTime birthDay)
        {
            CurrentUser.BirthDay = birthDay;
            Save($"users.xml", UserList);
        }

        public void setNewTask(Model.Task task)
        {
            CurrentUser.Tasks.Add(task);
            Save($"users.xml", UserList);
        }
        private List<User> GetUserData()
        {
            return Load<List<User>>($"users.xml") ?? new List<User>();
        }

        
        //public void RegisterDisplayDelegate(DisplayDelegateInfo dlg)
        //{
        //    delegateHandler = dlg;
        //}
        public void SetAcceptTask(int id)
        {
            Model.Task task = CurrentUser.Tasks.First(i => i.Id == id); //Балуемся с Linq
            if(task != null)
            {
                task.Accept = true;
                infoMsges("Задача успешно отмечена");
                if(task.EndTask < DateTime.Now)
                {
                    task.EndTask = DateTime.Now;
                    infoMsges("Вы просрочили задачу, нехорошо...");
                }
            } else
            {
                infoMsges("Задача не найдена в списке, попробуйте еще раз");
            }
            Save($"users.xml", UserList);
        }



    }
}
