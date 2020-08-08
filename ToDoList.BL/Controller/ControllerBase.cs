using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ToDoList.BL.Model;

namespace ToDoList.BL.Controller
{
    public class ControllerBase
    {
        protected void Save(string fileName, object item)
        {
            
            var formatter = new XmlSerializer(item.GetType());
            using(var fs = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(fs, item);
            }
        }

        protected T Load<T>(string fileName)
        {
            var formatter = new XmlSerializer(typeof(T));
            using(var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                if(fs.Length > 0 && formatter.Deserialize(fs) is T items)
                {
                    return items;
                } 
                else
                {
                    return default(T);
                }
            }

        }
    }
}
