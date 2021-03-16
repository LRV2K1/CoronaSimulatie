using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public class Globals
    {
        /// <summary>
        /// The number of hours in one timestep.
        /// </summary>
        public static float timestep { get; } = 1f/60f;

        //world parameters
        public static int tilesize { get; } = 20;
        public static int gridsize { get; } = 50;
        public static int worldsize { get; } = tilesize * gridsize;

        //population parameters
        public static int totalpopulation { get; } = 1000;
        public static int illpopulation { get; } = (int)(totalpopulation * 0.01f + 0.5f);
    }
}
