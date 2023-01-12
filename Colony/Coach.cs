using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colony
{
    class Coach : Settler
    {
        private static int _coachNb = 0;
        private static string _type;


        /// <summary>
        /// Constructor that allows you to create a coach
        /// </summary>
        public Coach() : base() 
        {
            _type = "C";
            SettlerType = _type;
            _coachNb++;
            _id = _type + _coachNb.ToString();
            _decreasingHunger = 3;
            _decreasingEnergy = 3;
        }


        /// <summary>
        /// Returns the type of the coach
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Allows the coach to train an athlete and therefore to be unavailable during this period
        /// </summary>
        /// <param name="turnNb">Turn where he will have finished training</param>
        public void Lead(int turnNb)
        {
            if (!IsInActivity)
            {
                NbTunrBeforeAvailable = 4 + turnNb;
                IsInActivity = true;
            }

            if (NbTunrBeforeAvailable == turnNb)
            {
                IsInActivity = false;
                NbTunrBeforeAvailable = 0;
            }
        }
    }
}
