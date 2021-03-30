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
        DataWriter rdata;

        public Program()
        {
            rdata = new DataWriter(0);
        }

        public void Setup(int k)
        {
            dataWriter = new DataWriter(k, rdata);

            random = new Random();

            people = new List<Person>();

            SaveData.Susceptible = Globals.totalpopulation;
            SaveData.Exposed = 0;
            SaveData.Infectious = 0;
            SaveData.Recovered = 0;

            for (int i = 0; i < Globals.totalpopulation; i++)
            {
                people.Add(new Person(random.Next(0, Globals.worldsize), random.Next(0, Globals.worldsize), random));
            }

            word = new WorldGrid(Globals.gridsize, Globals.tilesize, people);

            for (int i = 0; i < Globals.illpopulation; i++)
            {
                people[i].HealthStatus = HealthStatus.Infectious;
            }

            dataWriter.Write();

            step = 0;
            hours = 0;
        }

        public void Run()
        {
            for (int i = 0; i < 100; i++)
            {
                Setup(i);
                //for (int i = 0; i < 129600; i++)
                while (SaveData.Infectious > 0 || SaveData.Exposed > 0)
                {
                    step++;
                    foreach (Person p in people)
                    {
                        p.Move();
                    }
                    foreach (Person p in people)
                    {
                        p.Sickness();
                    }
                    if ((float)step * Globals.timestep > hours)
                    {
                        hours++;
                        dataWriter.Write();
                    }
                }
                dataWriter.End();
            }
        }
    }
}
