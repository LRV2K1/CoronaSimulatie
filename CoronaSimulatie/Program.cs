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

        int step;
        int hours;

        DataWriter dataWriter;

        public Program()
        {
            dataWriter = new DataWriter();

            random = new Random();

            people = new List<Person>();
            SaveData.Healthy = Globals.totalpopulation;

            for (int i = 0; i < Globals.totalpopulation; i++)
            {
                people.Add(new Person(random.Next(0, Globals.worldsize), random.Next(0, Globals.worldsize), random));
            }

            word = new WorldGrid(Globals.gridsize,Globals.tilesize,people);

            for (int i = 0; i < Globals.illpopulation; i++)
            {
                people[i].HealthStatus = HealthStatus.Ill;
            }

            dataWriter.Write(SaveData.Healthy, SaveData.Ill, SaveData.Recovered);

            step = 0;
            hours = 0;
        }

        public void Run()
        {
            while (SaveData.Ill > 0) 
            {
                step++;
                foreach(Person p in people)
                {
                    p.Move();
                }
                foreach(Person p in people)
                {
                    p.Sickness();
                }
                if ((float)step * Globals.timestep > hours)
                {
                    hours++;
                    dataWriter.Write(SaveData.Healthy, SaveData.Ill, SaveData.Recovered);
                }
            }
        }
    }
}
