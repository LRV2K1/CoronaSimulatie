using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public enum HealthStatus
    {
        Healthy,
        Ill,
        Recovered
    }

    public enum QuarentineStatus
    {
        Free,
        Quarentined
    }

    public class Person
    {
        //position
        protected float x, y;
        //target position
        float tx, ty;

        protected Tile tile;
        protected Random random;

        float direction;
        int sicksteps;
        int quarentineDays;

        HealthStatus healthStatus;
        QuarentineStatus quarentineStatus;

        public Person(float x, float y, Random random)
        {
            this.x = x;
            this.y = y;
            tx = x;
            ty = y;
            this.random = random;
            sicksteps = 0;
            quarentineDays = 0;
            healthStatus = HealthStatus.Healthy;
            quarentineStatus = QuarentineStatus.Free;
        }

        public virtual void Move()
        {
            if (tile == null)
                return;

            //close to the target.
            if (Math.Abs(tx - x) < 1.0f && Math.Abs(ty - y) < 1.0f)
                GetTarget();

            //direction += (float)((random.NextDouble() - 0.5f) * Math.PI * 0.5);
            //float distance = (float)random.NextDouble() * 3000f * Globals.timestep;

            x += (float)(3000f * Globals.timestep * Math.Cos(direction));
            y += (float)(3000f * Globals.timestep * Math.Sin(direction));


            //x += ((float)random.NextDouble() - 0.5f) * 1;
            //y += ((float)random.NextDouble() - 0.5f) * 1;

            tile.Move(this);
        }

        private void GetTarget()
        {
            direction = (float)((random.NextDouble() - 0.5f) * Math.PI * 2f);
            float distance = (float)random.NextDouble() * 100;
            tx = x + (float)(distance * Math.Cos(direction));
            ty = y + (float)(distance * Math.Sin(direction));
            if (tx < 0 || tx >= Globals.worldsize || ty < 0 || ty >= Globals.worldsize)
                GetTarget();
        }

        public void Sickness()
        {
            if (healthStatus == HealthStatus.Recovered)
                return;
            if (healthStatus == HealthStatus.Healthy)
            {
                if (quarentineStatus == QuarentineStatus.Free)
                {
                    List<List<Person>> neighbours = tile.IllNeighbours();

                    foreach (List<Person> l in neighbours)
                    {
                        foreach (Person p in l)
                        {
                            if (p.HealthStatus == HealthStatus.Ill && p.quarentineStatus == QuarentineStatus.Free)
                            {
                                float distance = (p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y);
                                int c = random.Next(0, 100);
                                if (c == 0 && distance < 225)
                                {
                                    HealthStatus = HealthStatus.Ill;
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    quarentineDays++;
                    if (quarentineDays > 120)
                    {
                        QuarentineStatus = QuarentineStatus.Free;
                        quarentineDays = 0;
                    }
                }
            }
            else
            {
                sicksteps++;
                int days = random.Next(20, 220);
                if (sicksteps >= days)
                {
                    HealthStatus = HealthStatus.Recovered;
                    QuarentineStatus = QuarentineStatus.Free;
                }
                else if (sicksteps >= 30)
                    QuarentineStatus = QuarentineStatus.Quarentined;
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

        public virtual HealthStatus HealthStatus
        {
            get { return healthStatus; }
            set {
                switch (healthStatus)
                {
                    case HealthStatus.Healthy:
                        SaveData.Healthy--;
                        break;
                    case HealthStatus.Ill:
                        SaveData.Ill--;
                        break;
                    case HealthStatus.Recovered:
                        SaveData.Recovered--;
                        break;
                }

                healthStatus = value;

                switch (healthStatus)
                {
                    case HealthStatus.Healthy:
                        SaveData.Healthy++;
                        break;
                    case HealthStatus.Ill:
                        SaveData.Ill++;
                        break;
                    case HealthStatus.Recovered:
                        SaveData.Recovered++;
                        break;
                }


                tile.UpdateStatus(this);
            }
        }

        public virtual QuarentineStatus QuarentineStatus
        {
            get { return quarentineStatus; }
            set { quarentineStatus = value; }
        }
    }
}
