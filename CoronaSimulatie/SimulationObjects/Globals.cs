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
        public static int simulations { get; } = 100;

        //world parameters
        public static int tilesize { get; } = 20;
        public static int gridsize { get; } = 500; 
        public static int worldsize { get; } = tilesize * gridsize;

        //population parameters
        public static int totalpopulation { get; } = 1000;
        public static int illpopulation { get; } = (int)(totalpopulation * 0.01f + 0.5f);
        public static int apppopulation { get; } = (int)(totalpopulation * 0.25f + 0.5f);

        //disease model
        public static float infectiondays { get; } = 2 * 24;           //infected, not infectous
        public static float a_contingiousnessdays { get; } = 4 * 24;   //infectous, no symptomes
        public static float C { get; } = 0.5f;

        //app
        public static float L { get; } = infectiondays + a_contingiousnessdays;
        public static float K { get; } = 0.25f;

        //movement
        public static float W { get; } = 3f;
        public static int T { get; } = 5;
        public static int D { get; } = 25;
    }
}
