using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;


namespace serial
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Character RobbieLens = new Character { FirstName = "Robbie", LastName = "Lens", Description= "Photographe"};
            File.WriteAllText("robbie.json", JsonSerializer.Serialize(RobbieLens));
            
        }
    }
    public class Character
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public Actor PlayedBy { get; set; }
    }
    public class Actor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public bool IsAlive { get; set; }
    }
}
