using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colony
{
    /// <summary>
    /// Structure that allows you to create buildings that are under construction while they are being created
    /// </summary>
    struct InConstructionBuilding
    {
        private string _buildingType;
        private int _x;
        private int _y;
        private List<Settler> _settlers;
        private int _creationTurn;
        private int _turnNb;
        private string _name;

        /// <summary>
        /// Builder to create a building under construction
        /// </summary>
        /// <param name="type">Specify the type of building ("S" for a sports infrastructure, "H" for a hotel and "R" for a restaurant</param>
        /// <param name="turnNb">Turn where the construction of the building began</param>
        /// <param name="x">Abscissa of the board where you want to build the angel at the top left dub building</param>
        /// <param name="y">Ordinate of the board where you want to build the angel at the top left dub building</param>
        /// <param name="builders">List of builders requisitioned to build this building</param>
        /// <param name="name">The name makes it possible to specify the type of sports infrastructure when there is one.</param>
        public InConstructionBuilding(string type,int turnNb, int x, int y, List<Settler> builders, string name)
        {

            _buildingType = type;
            _x = x;
            _y = y;
            _settlers = builders;
            _creationTurn = 0;
            _name = name;

            //Calculates the turn when the building will be finished
            foreach (Builder builder in builders)
            {
                _creationTurn = Math.Max(_creationTurn, Math.Abs(builder._itinerary[0]) + Math.Abs(builder._itinerary[1]) + turnNb );  //TODO Je crois que ça prend pas en compte le temps de construction du batiment
            }
            _turnNb = turnNb;
        }

        /// <summary>
        /// Returns the name of the building under construction
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns the type of the building under 
        /// </summary>
        public string BuildingType
        {
            get { return _buildingType; }
        }

        /// <summary>
        /// Returns the absicce of the board where the angel is built at the top left of the building under construction
        /// </summary>
        public int X
        {
            get { return _x; }
        }

        /// <summary>
        /// Returns the ordinate of the board where the angel is built at the top left of the building under construction
        /// </summary>
        public int Y
        {
            get { return _y; }
        }

        /// <summary>
        /// Returns the list of builders requisitioned to build the building under construction
        /// </summary>
        public List<Settler> Settlers
        {
            get { return _settlers; }
        }

        /// <summary>
        /// Returns the turn when the building will be 
        /// </summary>
        public int CreationTurn
        {
            get { return _creationTurn; }
        }
    }

    class Village
    {
        private List<Building> _buildings = new List<Building>();
        private List<InConstructionBuilding> _inConstruction = new List<InConstructionBuilding>();
        private int _maxNbSettlers;
        private int _lenght;
        private int _width;
        private string[,] _gameBoardBuilder;
        private List<Settler>[,] _gameBoardSettler;
        private List<Settler> _settlers = new List<Settler>();
        public Dictionary<string, bool> SportsInfrastructures { get; set; }
        public int ProfessionnelNb { get; set; }


        /// <summary>
        /// Builder which allows you to build a , which already contains a hotel, a restaurant and 3 builders
        /// </summary>
        public Village()
        {
            ProfessionnelNb = 0;
            SportsInfrastructures = new Dictionary<string, bool>();
            SportsInfrastructures.Add("Piscine olympique", false);
            SportsInfrastructures.Add("Terrain de sport collectif intérieur", false);
            SportsInfrastructures.Add("Stade", false);

            _lenght = 20;
            _width = 15;
            _gameBoardSettler = new List<Settler>[_lenght, _width];
            _gameBoardBuilder = new string[_lenght, _width];

            for (int i = 0; i < _width; i++)
            {
                for (int y = 0; y < _lenght; y++)
                {
                    GameBoardSettler[y, i] = new List<Settler>();
                }
            }

            Restaurant restaurant = new Restaurant(8, 8);
            Hotel hotel = new Hotel(0, 0);
            CreationBuilding(hotel);
            CreationBuilding(restaurant);
            Builder s1 = new Builder();
            Builder s2 = new Builder();
            Builder s3 = new Builder();
            s2.X = 0;
            s2.Y = 1;
            s3.X = 0;
            s3.Y = 2;
            _maxNbSettlers = 4;
            _buildings.Add(hotel);
            _buildings.Add(restaurant);
            AddSettler(s1);
            AddSettler(s2);
            AddSettler(s3);
        }

        /// <summary>
        /// Returns the array containing the buildings
        /// </summary>
        public string[,] GameBoardBuilder
        {
            get { return _gameBoardBuilder; }
        }

        /// <summary>
        /// Returns the array containing the 
        /// </summary>
        public List<Settler>[,] GameBoardSettler
        {
            get { return _gameBoardSettler; }
        }

        /// <summary>
        /// Returns the width of the game 
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Returns the length of the game board
        /// </summary>
        public int Lenght
        {
            get { return _lenght; }
        }

        /// <summary>
        /// Returns the list of buildings contained in the village
        /// </summary>
        public List<Building> Buildings
        {
            get { return _buildings; }
        }

        /// <summary>
        /// Returns and modifies the list of buildings under construction contained in the village
        /// </summary>
        public List<InConstructionBuilding> InConstruction

        {
            get { return _inConstruction; }
            set { _inConstruction = value; }
        }

        /// <summary>
        /// Returns the list of settlers contained in the village
        /// </summary>
        public List<Settler> GetSettlers()
        {
            return _settlers;
        }

        /// <summary>
        /// Counts the number of settlers available based on the chosen settler type
        /// </summary>
        /// <param name="type">Type of settlers whose number we want to know is available ("A" for a psortif, "B" for a builder and "C" for a hidden one)</param>
        /// <returns>Returns the number of colons available according to the type of colon chosen</returns>
        public int NbSettlerAvailable(string type)
        {
            return FindAvailable(type).Count();
        }

        /// <summary>
        /// Return a table of settlers free
        /// </summary>
        /// <param name="type">The type of the settlers required</param>
        /// <returns>A table of settlers available with the good type</returns>
        public List<Settler> FindAvailable(string type)
        {
            List<Settler> availables = new List<Settler>();
            foreach (Settler settler in _settlers)
            {
                if (settler.Available && settler.SettlerType.Equals(type))
                {
                    availables.Add(settler);
                }
            }
            return availables;
        }

        /// <summary>
        /// Allows you to add a building in the 
        /// </summary>
        /// <param name="building">Building to add to a village (it must therefore be created beforehand</param>
        public void AddBuildings(Building building)
        {
            _buildings.Add(building);
        }

        /// <summary>
        /// Add a settler to the village
        /// </summary>
        /// <param name="settler">Settler to add to the </param>
        public void AddSettler(Settler settler)
        {
            while (_gameBoardSettler[settler.X, settler.Y].Count != 0)
            {
                if (settler.Y < _width)
                {
                    settler.Y++;
                }
                else
                {
                    settler.X++;
                }
            }
            _settlers.Add(settler);
            _gameBoardSettler[settler.X, settler.Y].Add(settler);
            int i = 0;
            bool addHotel = false;
            bool addRestaurant = false;
            bool addSportInfrstructure = false;
            bool canAddSportInfrastructure = settler is Athletic;
            if (settler is SportsInfrastructure)
            {
                canAddSportInfrastructure = true;
            }
            while (i < _buildings.Count() && (addHotel == false || addRestaurant == false || addSportInfrstructure == canAddSportInfrastructure))
            {
                if (_buildings[i].haveFreePlace())
                {
                    if (_buildings[i] is Restaurant)
                    {
                        _buildings[i].Settlers.Add(settler);
                        settler.Buildings[1] = _buildings[i];
                        addRestaurant = true;
                    }
                    else if (_buildings[i] is Hotel)
                    {
                        _buildings[i].Settlers.Add(settler);//Le probleme vient d'ici, on peut pas recruter autre chose qu'un batisseur en premier je sais pas pourquoi
                        settler.Buildings[0] = _buildings[i]; //Tentative Roche
                    }
                    else if (canAddSportInfrastructure && _buildings[i] is SportsInfrastructure)
                    {
                        _buildings[i].Settlers.Add(settler);
                        settler.Buildings[2] = _buildings[i];
                        addSportInfrstructure = true;
                    }
                }
                i++;
            }

        }

        // <summary>
        /// See if there is still space available for welcoming new settlers in the restaurants present in the village
        /// </summary>
        /// <returns>Returns true if there is space, false otherwise</returns>
        public bool FreeRestaurantPlaces()
        {
            bool places = false;
            foreach (Building building in _buildings)
            {
                if (building.Type == "R" && building.Settlers.Count() < building.TotalPlace) {
                    places = true;
                }
            }
            return places;
        }

        /// <summary>
        /// See if there is still space available to welcome new settlers in the hotels present in the village
        /// </summary>
        /// <returns>Returns true if there is space, false otherwise</returns>
        public bool FreeHotelPlaces()
        {
            bool places = false;
            foreach (Building building in _buildings)
            {
                if (building.Type == "H" && building.Settlers.Count() < building.TotalPlace)
                {
                    places = true;
                }
            }
            return places;
        }

        /// <summary>
        /// See if there is enough space in hotels and restaurants to recruit a new settler.
        /// </summary>
        /// <returns>Returns true if there is enough space, false otherwise.</returns>
        public bool CanRecruit()
        {
            return FreeHotelPlaces() && FreeRestaurantPlaces();
        }

        /// <summary>
        /// Find out if there is a coach available
        /// </summary>
        /// <returns>Returns the available coach if there is one, null otherwise</returns>
        public Coach CanBeCoach()
        {
            bool available = false;
            int i = 0;
            Coach coach = null;
            while (i < _settlers.Count && !available)
            {
                if (_settlers[i] is Coach && _settlers[i].Available)
                {
                    available = true;
                    coach = (Coach)_settlers[i];
                }
                i++;
            }
            return coach;
        }

        /// <summary>
        /// Find out if there is a particular sports infrastructure present in the 
        /// </summary>
        /// <param name="name">Name of the infrastructure for which we want to know if it exists or not</param>
        public void GetInfrastructureByType(string name)
        {
            SportsInfrastructure sportsInfrastructure = null;
            int i = 0;
            bool findInfrastructure = false;
            while(i<_buildings.Count && findInfrastructure)
            {
                if (_buildings[i] is SportsInfrastructure && _buildings[i].Name == name)
                {
                    sportsInfrastructure = (SportsInfrastructure)_buildings[i];
                }
            }

        }

        /// <summary>
        /// Search if a particular athlete is present
        /// </summary>
        /// <param name="id">The id of the athlete you are looking </param>
        /// <returns>Returns the athlete if it exists, null otherwise</returns>
        public Athletic FindById(int id)
        {
            Athletic ourAthlete = null;
            foreach(Settler settler in _settlers)
            {
                if (settler is Athletic)
                {
                    Athletic athlete = (Athletic)settler;
                    if (athlete.AthleticNb == id)
                    {
                        ourAthlete = athlete;
                    }
                }
            }
            return ourAthlete;
        }

        /// <summary>
        /// See if one or more athletes can 
        /// </summary>
        /// <returns>Returns true if there is one who can train (i.e. if there is a colonist who is an athlete who is available), false otherwise</returns>
        public bool CanTrain()
        {
            bool canTrain = false;
            foreach(Settler settler in _settlers)
            {
                if(settler is Athletic && settler.Available)
                {
                    canTrain = true;
                }
            }
            return canTrain;
        }

        /// <summary>
        /// Creation of the building in the 
        /// </summary>
        /// <param name="building">Building to be created</param>
        public void CreationBuilding(Building building) 
        {
            int nbColumns = 0;
            int nbLines = 0;
            if (building is Hotel)
            {
                nbColumns = Building._buildingSize.FirstOrDefault(x => x.Key == "H").Value[1];
                nbLines = Building._buildingSize.FirstOrDefault(x => x.Key == "H").Value[0];
            }
            else if (building is Restaurant)
            {
                nbColumns = Building._buildingSize.FirstOrDefault(x => x.Key == "R").Value[1];
                nbLines = Building._buildingSize.FirstOrDefault(x => x.Key == "R").Value[0];
            }
            else if(building is SportsInfrastructure)
            {
                nbColumns = Building._buildingSize.FirstOrDefault(x => x.Key == "S").Value[1];
                nbLines = Building._buildingSize.FirstOrDefault(x => x.Key == "S").Value[0];

            }
            for (int x = building.X; x < nbLines + building.X; x++)
            {
                for (int y = building.Y; y < nbColumns + building.Y; y++)
                    _gameBoardBuilder[x, y] = building.Type;
            }
        }

        /// <summary>
        /// Creation of a building in progress
        /// </summary>
        /// <param name="inConstruction">Building under </param>
        public void CreatePendingBuilding(InConstructionBuilding inConstruction)
        {
            int nbColumns = 0;
            int nbLines = 0;
            string inCreation = "";
            if (inConstruction.BuildingType == "H")
            {
                nbColumns = Building._buildingSize.FirstOrDefault(x => x.Key == "H").Value[1];
                nbLines = Building._buildingSize.FirstOrDefault(x => x.Key == "H").Value[0];
                inCreation = "XH";
            }
            else if (inConstruction.BuildingType == "R")
            {
                nbColumns = Building._buildingSize.FirstOrDefault(x => x.Key == "R").Value[1];
                nbLines = Building._buildingSize.FirstOrDefault(x => x.Key == "R").Value[0];
                inCreation = "XR";
            }
            else if (inConstruction.BuildingType == "S")
            {
                nbColumns = Building._buildingSize.FirstOrDefault(x => x.Key == "S").Value[1];
                nbLines = Building._buildingSize.FirstOrDefault(x => x.Key == "S").Value[0];
                inCreation = "XS";

            }
            for (int x = inConstruction.X; x < nbLines + inConstruction.X; x++)
            {
                for (int y = inConstruction.Y; y < nbColumns + inConstruction.Y; y++)
                    _gameBoardBuilder[x, y] = inCreation;
            }
        }


        public override string ToString()
        {
            string retour = "Mon village est composé de " + _settlers.Count() + " colons. Les voicis : \n";
            foreach (Settler settler in _settlers)
                retour += settler.ToString();
            retour += "Mon village est composé de " + _buildings.Count() + " batiments. Les voici : \n ";
            foreach (Building building in _buildings)
                retour += building.ToString();
            return retour;
        }

    }
}
