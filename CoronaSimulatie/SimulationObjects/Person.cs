using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public enum HeathStatus
    {
        Healthy,
        Ill,
        Recovered
    }

    public class Person
    {
        protected float x, y;

        protected Tile tile;
        protected Random random;

        float direction;
        int sicksteps;

        HeathStatus status;

        public Person(float x, float y, Random random)
        {
            this.x = x;
            this.y = y;
            this.random = random;
            sicksteps = 0;
            status = HeathStatus.Healthy;
        }

        public virtual void Move()
        {
            if (tile == null)
                return;

            direction += (float)((random.NextDouble() - 0.5f) * Math.PI * 0.5);
            float distance = (float)random.NextDouble() * 1f;

            x += (float)(distance * Math.Cos(direction));
            y += (float)(distance * Math.Sin(direction));


            //x += ((float)random.NextDouble() - 0.5f) * 1;
            //y += ((float)random.NextDouble() - 0.5f) * 1;

            tile.Move(this);
        }

        public void Sickness()
        {
            if (status == HeathStatus.Recovered)
                return;
            if (status == HeathStatus.Healthy)
            {
                List<List<Person>> neighbours = tile.IllNeighbours();

                foreach (List<Person> l in neighbours)
                {
                    foreach (Person p in l)
                    {
                        if (p.Status == HeathStatus.Ill)
                        {
                            float distance = (p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y);
                            int c = random.Next(0, 100);
                            if (c == 0 && distance < 225)
                            {
                                Status = HeathStatus.Ill;
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                sicksteps++;
                int days = random.Next(20, 220);
                if (sicksteps >= days)
                    Status = HeathStatus.Recovered;
            }
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public virtual Tile Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        public virtual HeathStatus Status
        {
            get { return status; }
            set {
                switch (status)
                {
                    case HeathStatus.Healthy:
                        SaveData.Healthy--;
                        break;
                    case HeathStatus.Ill:
                        SaveData.Ill--;
                        break;
                    case HeathStatus.Recovered:
                        SaveData.Recovered--;
                        break;
                }

                status = value;

                switch (status)
                {
                    case HeathStatus.Healthy:
                        SaveData.Healthy++;
                        break;
                    case HeathStatus.Ill:
                        SaveData.Ill++;
                        break;
                    case HeathStatus.Recovered:
                        SaveData.Recovered++;
                        break;
                }


                tile.UpdateStatus(this);
            }
        }
    }
}
