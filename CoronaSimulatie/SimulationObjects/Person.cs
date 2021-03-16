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
        int quarentinesteps;
        int waittimer;

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
            quarentinesteps = 0;
            healthStatus = HealthStatus.Healthy;
            quarentineStatus = QuarentineStatus.Free;
        }

        public virtual void Move()
        {
            if (tile == null)
                return;

            //close to the target.
            if (Math.Abs(tx - x) < 1800f * Globals.timestep && Math.Abs(ty - y) < 1800 * Globals.timestep)
                GetTarget();

            if (waittimer > 0)
            {
                waittimer--;
                return;
            }

            //direction += (float)((random.NextDouble() - 0.5f) * Math.PI * 0.5);
            //float distance = (float)random.NextDouble() * 3000f * Globals.timestep;

            //average speed 3 km/h
            x += (float)(3000f * Globals.timestep * Math.Cos(direction));
            y += (float)(3000f * Globals.timestep * Math.Sin(direction));


            //x += ((float)random.NextDouble() - 0.5f) * 1;
            //y += ((float)random.NextDouble() - 0.5f) * 1;

            tile.Move(this);
        }

        public void GetTarget()
        {
            //tx = random.Next(0, Globals.worldsize);
            //ty = random.Next(0, Globals.worldsize);

            //if (tx - x != 0)
            //{
            //    direction = (float)Math.Atan(((float)tx - x) / ((float)ty - y));
            //    if (tx - x < 0)
            //    {
            //        direction += (float)Math.PI;
            //    }
            //}
            //else
            //{
            //    direction = (float)Math.PI * 0.5f * Math.Sign(ty - y);
            //}

            direction = (float)((random.NextDouble() - 0.5f) * Math.PI * 2f);
            float distance = (float)random.NextDouble() * 300;
            tx = x + (float)(distance * Math.Cos(direction));
            ty = y + (float)(distance * Math.Sin(direction));
            waittimer = random.Next(0, (int)(1f/Globals.timestep)); //wait between 0 to 1 hour.
            //if (tx < 0 || tx >= Globals.worldsize || ty < 0 || ty >= Globals.worldsize)
            //    //GetTarget();
            //    return;
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
                                int c = random.Next(0, (int)(0.5f/Globals.timestep));   // staying close to someone roughly 0.5 hours to get sick.
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
                    quarentinesteps++;
                    if (quarentinesteps > 336f/Globals.timestep) //come out of quarentine after 14 days, 14 days * 24 hours = 336 timesteps
                    {
                        QuarentineStatus = QuarentineStatus.Free;
                        quarentinesteps = 0;
                    }
                }
            }
            else
            {
                sicksteps++;
                int days = random.Next((int)(168f/Globals.timestep), (int)(504f / Globals.timestep));    //sick between 7 to 21 days, 7 days * 24 hours = 168, 21 days * 24 hours = 504
                if (sicksteps >= days)
                {
                    HealthStatus = HealthStatus.Recovered;
                    QuarentineStatus = QuarentineStatus.Free;
                }
                else if (sicksteps >= 72f/ Globals.timestep)   //go into quarentine after 3 days, 3 days * 24 hours = 72.
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
