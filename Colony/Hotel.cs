using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colony
{
    class Hotel : Building
    {
        protected static int _hotelNb = 0;
        private Settler[] _settlers;
        public static string type = "H";
        public static int BuilderNb = 2;
        public static int TurnNb = 2;
        public int _linesNb; 
        public int _columnsNb;

        /// <summary>
        /// Builder who wants to create a hotel, with 5 places available for 
        /// </summary>
        /// <param name="x">Abscisse dans le village de l'angle en haut à gauche de l'hotel</param>
        /// <param name="y">Ordinate dans le village de l'angle en haut à gauche de l'hotel</param>
        public Hotel(int x, int y) : base(x, y)
        {
            _totalPlace = 5;
            _nbPlaces = _totalPlace;
            _hotelNb++;
            _type = type;
            _settlers = new Settler[5];
            _x = x;
            _y = y;

        }

        /// <summary>
        /// Allows you to recover the colonists assigned to the hotel
        /// </summary>
        /// <returns></returns>
        public Settler[] GetSettlers() //TODO j'ai pas l'impression qu'elle soit utile, ça tourne quand même sans, a supprimer apres qu'Alex ait validée
        {
            return _settlers;
        }

        /// <summary>
        /// It returns a dimension of the hotel: the number of lines it takes up on the game board
        /// </summary>
        public int LinesNb //TODO J'ai pas l'impression qu'elle soit utile, a supprimer apres accord d'Alex
        {
            get {return Building._buildingSize.FirstOrDefault(x => x.Key == "H").Value[0];}
        }

        /// <summary>
        /// It returns a dimension of the hotel: the number of columns it takes up on the game board
        /// </summary>
        public int ColumnsNb
        {
            get { return Building._buildingSize.FirstOrDefault(x => x.Key == "H").Value[1]; }
        }


        public override string ToString()
        {
            return base.ToString() + "C'est un hotel\n";
        }
    }
}
