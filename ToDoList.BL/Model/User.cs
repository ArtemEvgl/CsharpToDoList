using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.BL.Model
{
    [Serializable]
    public class User
    {
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        
        public List<Task> Tasks { get; set; }

        public int Age { get
            {
                int age = DateTime.Now.Year - BirthDay.Year;
                if (BirthDay > DateTime.Now.AddYears(-age)) age--;
                return age;
            } }

        public User ()
        {           
        }
        public User(string name, DateTime birthDay)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Имя не может быть пустым", nameof(name));
            }
            if(birthDay < DateTime.Parse("01.01.1900") && birthDay >= DateTime.Now)
            {
                throw new ArgumentNullException("Невозможная дата рождения", nameof(DateTime));
            }
            Name = name;
            BirthDay = birthDay;
            Tasks = new List<Task>();
        }

        public User(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Имя не может быть пустым", nameof(name));
            }
            Name = name;
            Tasks = new List<Task>();
        }
        public override string ToString()
        {
            return $"{Name} {Age}";
        }

    }
}
