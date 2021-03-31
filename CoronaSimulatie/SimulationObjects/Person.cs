using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public enum HealthStatus
    {
        Susceptible,
        Exposed,
        Infectious,
        Recovered
    }

    public class Person
    {
        //position
        protected float x, y;
        //target position
        float tx, ty;

        protected Tile tile;
        protected Random random;
        protected App app;

        float direction;
        int sicksteps;
        int waittimer;

        HealthStatus healthStatus;

        public Person(float x, float y, Random random)
        {
            this.x = x;
            this.y = y;
            tx = x;
            ty = y;
            this.random = random;
            sicksteps = 0;
            healthStatus = HealthStatus.Susceptible;
            app = null;
        }

        public virtual void Move()
        {
            if (tile == null)
                return;

            //close to the target.
            if (Math.Abs(tx - x) < 15000f * Globals.timestep && Math.Abs(ty - y) < 15000 * Globals.timestep)
                GetTarget();

            if (waittimer > 0)
            {
                waittimer--;
                return;
            }

            //direction += (float)((random.NextDouble() - 0.5f) * Math.PI * 0.5);
            //float distance = (float)random.NextDouble() * 3000f * Globals.timestep;

            //average speed 3 km/h
            x += (float)(30000f * Globals.timestep * Math.Cos(direction));
            y += (float)(30000f * Globals.timestep * Math.Sin(direction));


            //x += ((float)random.NextDouble() - 0.5f) * 1;
            //y += ((float)random.NextDouble() - 0.5f) * 1;

            tile.Move(this);
        }

        public void GetTarget()
        {
            x = tx;
            y = ty;

            //hotspots
            int c = random.Next(0, 5);
            if (c == 0)
            {
                c = random.Next(0, 5);
                switch (c)
                {
                    case 0:
                        tx = Globals.worldsize / 4;
                        ty = Globals.worldsize / 4;
                        break;
                    case 1:
                        tx = 3*(Globals.worldsize / 4);
                        ty = Globals.worldsize / 4;
                        break;
                    case 3:
                        tx = 3 * (Globals.worldsize / 4);
                        ty = 3 * (Globals.worldsize / 4);
                        break;
                    case 4:
                        tx = Globals.worldsize / 4;
                        ty = 3 * (Globals.worldsize / 4);
                        break;
                }
                tx += random.Next(-25, 26);
                ty += random.Next(-25, 26);
            }
            else
            {
                tx = random.Next(0, Globals.worldsize);
                ty = random.Next(0, Globals.worldsize);
            }


            if (tx - x != 0)
            {
                direction = (float)Math.Atan((ty - y) / (tx - x));
                if (tx - x < 0)
                {
                    direction += (float)Math.PI;
                }
            }
            else
            {
                direction = (float)Math.PI * 0.5f * Math.Sign(ty - y);
            }

            waittimer = random.Next(0, (int)(3f/Globals.timestep)); //wait between 0 to 3 hour.
        }

        public void Sickness()
        {
            if (app != null && healthStatus != HealthStatus.Recovered)
            {
                app.Run();
            }

            switch (healthStatus)
            {
                case HealthStatus.Susceptible:
                    Susceptible();
                    break;
                case HealthStatus.Exposed:
                    Exposed();
                    break;
                case HealthStatus.Infectious:
                    Infectious();
                    break;
                case HealthStatus.Recovered:
                    Recovered();
                    break;
            }
        }

        private void Susceptible()
        {
            List<List<Person>> neighbours = tile.InfectiousNeighbours();
            foreach (List<Person> l in neighbours)
            {
                foreach (Person p in l)
                {

                    float distance = (p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y);
                    int c = random.Next(0, (int)(Globals.C / Globals.timestep));   // staying close to someone roughly 0.5 hours to get sick.
                    if (c == 0 && distance < 225)
                    {
                        HealthStatus = HealthStatus.Exposed;
                        sicksteps = 0;
                        return;
                    }

                }
            }
        }

        private void Exposed()
        {
            sicksteps++;
            if (sicksteps >= Globals.infectiondays / Globals.timestep)
            {
                sicksteps = 0;
                HealthStatus = HealthStatus.Infectious;
            }
        }

        private void Infectious()
        {
            sicksteps++;
            if (sicksteps >= Globals.a_contingiousnessdays / Globals.timestep)
            {
                sicksteps = 0;
                HealthStatus = HealthStatus.Recovered;
            }
        }

        //get a test.
        public void CheckSickness()
        {
            switch (healthStatus)
            {
                case HealthStatus.Susceptible:
                    break;
                case HealthStatus.Exposed:
                    HealthStatus = HealthStatus.Recovered;
                    break;
                case HealthStatus.Infectious:
                    HealthStatus = HealthStatus.Recovered;
                    break;
                case HealthStatus.Recovered:
                    break;
            }
        }

        private void Recovered()
        {
            return;
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

        public virtual App App
        {
            get { return app; }
            set { 
                app = value;
                app.Owner = this;
            }
        }

        public virtual HealthStatus HealthStatus
        {
            get { return healthStatus; }
            set {
                switch (healthStatus)
                {
                    case HealthStatus.Susceptible:
                        SaveData.Susceptible--;
                        break;
                    case HealthStatus.Exposed:
                        SaveData.Exposed--;
                        break;
                    case HealthStatus.Infectious:
                        SaveData.Infectious--;
                        break;
                    case HealthStatus.Recovered:
                        SaveData.Recovered--;
                        break;
                }

                healthStatus = value;

                switch (healthStatus)
                {
                    case HealthStatus.Susceptible:
                        SaveData.Susceptible++;
                        break;
                    case HealthStatus.Exposed:
                        SaveData.Exposed++;
                        break;
                    case HealthStatus.Infectious:
                        SaveData.Infectious++;
                        break;
                    case HealthStatus.Recovered:
                        SaveData.Recovered++;
                        if (app != null)
                            app.SentMessage();
                        break;
                }

                tile.UpdateStatus(this);
            }
        }
    }
}
