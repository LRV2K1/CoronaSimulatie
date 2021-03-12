using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoronaSimulatie.SimulationObjects;

namespace CoronaSimulatie
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        WorldGrid word;
        List<Person> people;

        public Program()
        {
            people = new List<Person>();
            people.Add(new Person(10, 10));


            word = new WorldGrid(100,100,100,100,people);
        }

        public void Run()
        {
            for (int i = 0; i < 100; i++)
            {
                foreach(Person p in people)
                {
                    p.Move();
                }
                foreach(Person p in people)
                {
                    p.Sickness();
                }
                Console.ReadLine();
            }
        }
    }
}
