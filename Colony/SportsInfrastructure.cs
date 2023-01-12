using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colony
{
    class SportsInfrastructure : Building
    {
        protected static int _sportsInfrastructureNb = 0;
        public static int BuilderNb = 2;
        public static string type = "S";
        public static int TurnNb = 1;
        public int _linesNb;
        public int _columnsNb;
        protected string _name;

        /// <summary>
        /// Constructeur qui pemet de créer une infrastructure sportive
        /// </summary>
        /// <param name="x">Abscissa of the position on the board where you want to position the top left corner of the building</param>
        /// <param name="y">Ordinate of the position on the board where you want to position the top left corner of the building</param>
        /// <param name="name">Name of the sports infrastructure</param>
        public SportsInfrastructure(int x, int y, string name) : base(x, y)
        {
            _sportsInfrastructureNb++;
            _type = type;
            _id = _type + _sportsInfrastructureNb.ToString();
            _name = name;
        }

        /// <summary>
        /// It gives us a dimension of sports infrastructure: the number of lines it takes on the game board
        /// </summary>
        public int LinesNb
        {
            get { return Building._buildingSize.FirstOrDefault(x => x.Key == "S").Value[0]; }
        }


        /// <summary>
        /// It returns a dimension of sports infrastructure: the number of columns it takes on the game board
        /// </summary>
        public int ColumnsNb
        {
            get { return Building._buildingSize.FirstOrDefault(x => x.Key == "S").Value[1]; }
        }


        public override string ToString()
        {
            return base.ToString() + "C'est une infrastructure sportive \n";
        }
    }
}
