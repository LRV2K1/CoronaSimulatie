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
        Random random;

        DataWriter dataWriter;

        public Program()
        {
            dataWriter = new DataWriter();

            random = new Random();

            people = new List<Person>();
            people.Add(new Person(10, 10, random));


            word = new WorldGrid(Globals.gridsize,Globals.tilesize,people);
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
                //Console.ReadLine();
            }
        }
    }
}
