using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colony
{
    class Restaurant : Building
    {

        protected static int _restaurantNb = 0;
        private Settler[] _settlers;
        public static string type = "R";
        public static int BuilderNb = 2; 
        public static int TurnNb = 2;
        public int _linesNb;
        public int _columnsNb;

        /// <summary>
        /// Constructeur qui permet de créer un restaurant 
        /// </summary>
        /// <param name="x">Abscissa of the top left corner of the restaurant from where it is positioned on the platform</param>
        /// <param name="y">Ordinate  of the top left corner of the restaurant from where it is positioned on the platform</param>
        public Restaurant(int x, int y) : base(x, y)
        {
            _totalPlace = 10;
            _nbPlaces = _totalPlace;
            _linesNb = 3;
            _columnsNb = 5;
            _restaurantNb++;
            _type = type;
            _id = _type + _restaurantNb.ToString();
            _settlers = new Settler[10]; 
        }

        /// <summary>
        /// It gives us a dimension of the restaurant: the number of lines it takes up on the game board
        /// </summary>
        public int LinesNb
        {
            get { return Building._buildingSize.FirstOrDefault(x => x.Key == "R").Value[0]; }
        }

        /// <summary>
        /// It returns a dimension of the restaurant: the number of columns it takes up on the game board
        /// </summary>
        public int ColumnsNb
        {
            get { return Building._buildingSize.FirstOrDefault(x => x.Key == "R").Value[1]; }
        }

        public override string ToString()
        {
            return base.ToString() + "C'est un restaurant\n";
        }
    }
}
