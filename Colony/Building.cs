using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colony
{
    abstract class Building
    {
        protected int _totalPlace;
        protected int _nbPlaces;
        protected int _x, _y;
        protected string _id;
        protected string _type;
        private List<Settler> _settlers;
        protected string _name;
        public static Dictionary<string, int[]> _buildingSize = new Dictionary<string, int[]> { { "H", new int[2] { 3, 3 } }, { "R", new int[2]{ 3, 5 } }, { "S", new int[2]{ 5, 5 } } };

        /// <summary>
        /// This constructor allows you to create a building on the chosen coordinates
        /// </summary>
        /// <param name="x">Abscissa of the position of the top left corner of the building on the game board</param>
        /// <param name="y">Ordinate of the position of the top left corner of the building on the game board</param>
        public Building(int x, int y)
        {
            _x = x;
            _y = y;
            _settlers = new List<Settler>();
        }


        public string Id//TODO a supprimer après l'accord d'Alex
        {
            get { return _id; }
        }

        /// <summary>
        /// Returns the name of the building, which allows them to be distinguished from each other
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns the dictionary that contains the type of buildings and their dimensions
        /// </summary>
        public Dictionary<string, int[]> BuildingSize
        {
            get { return _buildingSize; }
        }

        /// <summary>
        /// Returns the number of lines of the building entered in parameter
        /// </summary>
        /// <param name="type">indicates the type of building for which we want to know the number of lines</param>
        /// <returns></returns>
        public static int GetLinesNb(string type)
        {
            return Building._buildingSize.FirstOrDefault(x => x.Key == type).Value[0];
        }

        /// <summary>
        /// Returns the number of columns of the building entered in parameter
        /// </summary>
        /// <param name="type">indicates the type of building for which we want to know the number of columns</param>
        /// <returns></returns>
        public static int GetColumnsNb(string type)
        {
            return Building._buildingSize.FirstOrDefault(x => x.Key == type).Value[1];
        }

        /// <summary>
        /// Returns the number of places there are for settlers in the building
        /// </summary>
        public int NbPlaces//TODO à supprimer après l'accord d'Alex
        {
            get { return _nbPlaces; }
        }

        /// <summary>
        /// Returns the abscissa of the position of the top left corner of the building
        /// </summary>
        public int X
        {
            get { return _x; }
        }

        /// <summary>
        /// Returns the ordinate of the position of the top left corner of the building
        /// </summary>
        public int Y
        {
            get { return _y; }
        }

        /// <summary>
        /// returns or saves the total number of spaces in the building
        /// </summary>
        public int TotalPlace
        {
            get { return _totalPlace; }
            set { _totalPlace = value; }
        }

        /// <summary>
        /// returns the list of settlers assigned to this building
        /// </summary>
        public List<Settler> Settlers
        {
            get { return _settlers; }
        }

        /// <summary>
        /// Returns the type of building ("H" for hotel, "R" for restaurant and "S" for sports infrastructure)
        /// </summary>
        public string Type
        {
            get { return _type; }
        }


        /// <summary>
        /// Say if the building have free place
        /// </summary>
        /// <returns>True if the building have free place and false if they are not</returns>
        public bool haveFreePlace()
        {
            int i = 0;
            bool freePlace = false;

            if (_settlers.Count() != 0)
            {
                while (i < _settlers.Count() || !freePlace)
                {
                    if (_settlers[i] != null)
                    {
                        freePlace = true;
                    }
                    i++;
                } 
            }
            else
            {
                freePlace = true;
            }
            return freePlace;
        }


        public override string ToString()
        {
            return "\ncolonnes \nIl a comme coodronnées : " + _x + " , " + _y
                + "\n Son nom est : " + _name + "\n";
        }
    }
}
