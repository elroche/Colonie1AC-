using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colony
{
    abstract class Settler
    {
        protected static int _settlersNb = 0;
        protected string _id;
        public static int Energy = 50;
        public static int Hunger = 30;
        protected int _energyState;
        protected int _hungerState;
        public bool IsInActivity { get; set; }
        protected bool _available;
        public int NbTunrBeforeAvailable { get; set; }
        protected int _decreasingEnergy = 1;
        protected int _decreasingHunger = 1;
        protected int _timeToEat = 3;
        protected int _timeToSleep = 5;
        protected int _x, _y;
        public int[] _itinerary = { 0, 0 };
        protected Building[] _buildings = new Building[2];
        public string SettlerType { get; set; }


        /// <summary>
        /// Builder that allows you to create a colonist, who is immediately available and has a maximum level of hunger and energy
        /// </summary>
        public Settler()
        {
            NbTunrBeforeAvailable = 0;
            IsInActivity = false;
            _settlersNb++;
            _energyState = Energy;
            _hungerState = Hunger;
            _x = 0;
            _y = 0;
            //_available = true;
        }

        /// <summary>
        /// Allows to recover or modify the energy level of the colonist
        /// </summary>
        public int EnergyState
        {
            get { return _energyState; }
            set { _energyState = value; }
        }


        /// <summary>
        /// Allows you to recover or modify the colon's hunger level
        /// </summary>
        public int HungerState
        {
            get { return _hungerState; }
            set { _hungerState = value; }
        }


        /// <summary>
        /// Allows you to retrieve the buildings assigned to the settler (Building[0] corresponds to his hotel, Building[1] to his restaurant)
        /// </summary>
        public Building[] Buildings
        {
            get { return _buildings; }
        }

        /// <summary>
        ///  de récupérer le nombre de tour que met un colon à manger
        /// </summary>
        public int TimeToEat
        {
            get { return _timeToEat; }
        }


        /// <summary>
        /// Permet de récupérer le nombre de tour que met un colon à dormir
        /// </summary>
        public int TimeToSleep
        {
            get { return _timeToSleep; }
        }

        /// <summary>
        /// Allows to retrieve or modify the abscissa of the colon position
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Allows you to retrieve or modify the ordinate of the colon's position
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value;  }
        }

        /// <summary>
        /// Returns and changes a settler's availability
        /// </summary>
        public bool Available
        {
            get
            {
                if (NbTunrBeforeAvailable == 0 && !IsInActivity)
                {
                    _available = true;
                }
                else
                {
                    _available = false;
                }
                return _available;
            }
            set { _available = value; }
        }

        /// <summary>
        /// Allows the colon to play (i.e. to make him move, to decrease his level of hunger and energy...)
        /// </summary>
        /// <param name="GameBoardSettler">Look in the Settlers </param>
        /// <param name="turnNb">look at the turn where we are in the game</param>
        public virtual void Play(List<Settler>[,] GameBoardSettler, int turnNb)
        {
            if (Math.Abs(_itinerary[0]) + Math.Abs(_itinerary[1]) != 0) 
            {
                GameBoardSettler[_x, _y].Remove(this);
                this.Move();
                GameBoardSettler[_x, _y].Add(this);
            }
            
            _energyState -= _decreasingEnergy;
            _hungerState -= _decreasingHunger;

            if (_energyState <= 0)
            {
                _energyState = 0;
                if (!IsInActivity)
                {
                    this.CalculatingItinerary(_buildings[0].X, _buildings[0].Y);
                    NbTunrBeforeAvailable = Math.Abs(_itinerary[0]) + Math.Abs(_itinerary[1]) + 2 + turnNb;
                    Console.WriteLine("nb tour " + NbTunrBeforeAvailable);
                    IsInActivity = true;
                }
                else
                {
                    Console.WriteLine("tour " + NbTunrBeforeAvailable);
                    if (NbTunrBeforeAvailable == turnNb)
                    {
                        _energyState = Energy;
                        IsInActivity = false;
                        NbTunrBeforeAvailable = 0;
                    }
                }
            }
            if (_hungerState <= 0)
            {
                _hungerState = 0;
                if (!IsInActivity)
                {
                    this.CalculatingItinerary(_buildings[1].X, _buildings[1].Y);
                    NbTunrBeforeAvailable = Math.Abs(_itinerary[0]) + Math.Abs(_itinerary[1]) + 2 + turnNb;
                    Console.WriteLine("nb tour " + NbTunrBeforeAvailable);
                    IsInActivity = true;
                }
                else
                {
                    Console.WriteLine("tour " + NbTunrBeforeAvailable);
                    if (NbTunrBeforeAvailable == turnNb)
                    {
                        Console.WriteLine("test");
                        _hungerState = Hunger;
                        IsInActivity = false;
                        NbTunrBeforeAvailable = 0;
                    }
                }
            }

        }

        /// <summary>
        /// Calculate the distance between the destination and the setller location
        /// </summary>
        /// <param name="xDestination">Abscisse</param>
        /// <param name="yDestination"></param>
        public void CalculatingItinerary(int xDestination, int yDestination)
        {
            _itinerary[0] =  _x - xDestination;
            _itinerary[1] =  _y - yDestination;
        }

        /// <summary>
        /// Allows a settler to move, depending on the itinerary they have (_itinerary[0] for the abscissa, and _itinerary[1] for the ordinate)
        /// </summary>
        public void Move()
        {
            if (_itinerary[0] != 0)
            {
                _x = _itinerary[0] > 0 ? _x - 1 : _x + 1;
                _itinerary[0] = _itinerary[0] > 0 ? _itinerary[0] - 1 : _itinerary[0] + 1;
            }
            else if (_itinerary[1] != 0)
            {
                _y = _itinerary[1] > 0 ? _y - 1 : _y + 1;
                _itinerary[1] = _itinerary[1] > 0 ? _itinerary[1] - 1 : _itinerary[1] + 1;
            }
        }

        /// <summary>
        ///This method makes it possible to know if the colon must go to eat
        /// </summary>
        /// <returns>Returns true if hungry, false otherwise</returns>
        public bool IsHungry()
        {
            bool isHungry = false;
            if (_hungerState == 0)
                isHungry = true;
            return isHungry;
        }

        /// <summary>
        /// This method allows you to know if the colon should go to sleep
        /// </summary>
        /// <returns>Returns true if sleepy, false otherwise</returns>
        public bool IsSleepy()
        {
            bool isSleepy = false;
            if (_energyState == 0)
                isSleepy = true;
            return isSleepy;
        }


        public override string ToString()
        {
            return _id + "\nNiveau d'énergie : " + _energyState + "\nNiveau de faim : " + _hungerState
                + "\nCoodronnées : " + _x + " , " + _y + "\nDisponibilité : " + _available + "\n";
        }

    }
}