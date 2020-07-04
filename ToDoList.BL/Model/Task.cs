﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.BL.Model
{
    [Serializable]
    public class Task
    {
        public string Description { get; set; }

        public bool Accept { get; set; } = false;

        public DateTime StartTask { get; set; } = DateTime.Now;

        public DateTime EndTask { get; set; }

        public int Id { get; set; }

        public Task (string description, DateTime startTask, DateTime endTask, int sizeListTasks)
        {            
            if(string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException("Описание задачи не может быть пустым", nameof(description));
            }
            if(startTask < DateTime.Now)
            {
                throw new ArgumentNullException("Невозможно запланировть задачу в прошлое", nameof(startTask));
            }
            if (startTask < endTask)
            {
                throw new ArgumentNullException("Конец задачи может быть раньше начала", nameof(startTask));
            }
            if(sizeListTasks < 0)
            {
                throw new ArgumentNullException("Размер списка задач не может быть отрицательным", nameof(sizeListTasks));
            }
            Description = description;
            StartTask = startTask;
            EndTask = endTask;
            Id = sizeListTasks++;
        }

        public Task(string description, DateTime startTask, int sizeListTasks)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException("Описание задачи не может быть пустым", nameof(description));
            }
            if (startTask < DateTime.Now)
            {
                throw new ArgumentNullException("Невозможно запланировть задачу в прошлое", nameof(startTask));
            }
            Description = description;
            StartTask = startTask;
            Id = sizeListTasks++;
        }

        public Task(string description, int sizeListTasks, DateTime endTask)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException("Описание задачи не может быть пустым", nameof(description));
            }
            Description = description;
            Id = 1;
            if (sizeListTasks > 0) ++Id;
            EndTask = endTask;
        }

        public Task() { }

        public override string ToString()
        {
            string status = "не выполнена";
            string endTime = "";
            if (Accept)
            {
                status = "выполнена";
                endTime = EndTask.ToString();
            }
            return $"{Id}. {Description} - {status} {endTime}";
            //todo: фвфывфыв
        }
    }
}