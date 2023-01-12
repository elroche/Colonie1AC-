using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colony
{
    class Simulation
    {
        private Village _village;
        private static int _turn = 1;
        private int _turnNb;

        /// <summary>
        /// Constructor that creates a simulation, containing a village
        /// </summary>
        public Simulation()
        {
            _turnNb = _turn;
            _village = new Village();
        }


        public override string ToString()
        {
            string retour = "\nMA SIMULATION : \nMon village est le suivant : \n" + _village.ToString();
            retour += "\nIl y a : " + _village.Buildings.Count() + " batiments\n";
            if (_village.Buildings.Count() != 0)
                retour += "Les batiments existants sont les suivants : \n";
            foreach (Building b in _village.Buildings)
                retour += b.ToString();
            if (_village.GetSettlers().Count() != 0)
                retour += "Les colons existants sont les suivants : \n";
            foreach (Settler s in _village.GetSettlers())
                retour += s.ToString();
            retour += "\n";
            return retour;
        }

        public void Launch()
        {
            Console.WriteLine("Bienvenue dans notre jeu : LE VILLAGE OLYMPIQUE !\n");
            Console.WriteLine("- Si vous voulez lire les règles du jeu, entrez 1 \n- Si vous voulez dirèctement jouer, entrez 2");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 1)
            {
                RulesOfTheGame();
                Console.WriteLine("Entrez 2 si maintenant vous souhaitez jouer\n");
                choice = int.Parse(Console.ReadLine());
            }
            if (choice == 2)
                Play();
            else
            {
                Console.WriteLine("Votre réponse n'est pas valide, veuillez entrer une réponse valide \n");
                Launch();
            }
        }

        /// <summary>
        /// Method that allows us to play, and launching a game and continuing it until it is finished (i.e. won or lost)
        /// </summary>
        public void Play()
        {
            bool end = _village.ProfessionnelNb == 3;
            while (!end && _turnNb < 100)
            {
                PendingBuildingCreation();
                if (_village.CanRecruit() || _village.NbSettlerAvailable("B") >= Math.Min(Math.Min(Hotel.BuilderNb, Restaurant.BuilderNb), SportsInfrastructure.BuilderNb) || _village.CanTrain()) //Verifie qu'on peut effectuer une action sur ce tour, ou alors ça passe tout seul au tour suivant
                {
                    Console.WriteLine("Vous êtes au tour {0} \n --------- ", _turnNb);
                    DisplayGameBoard();

                    bool buildBuilding = true;
                    bool recruitSettler = true;
                    bool trainAthetics = true;
                    Console.WriteLine("Entrez 0 pour passer au tour suivant sans effectuer aucune action");
                    if (_village.NbSettlerAvailable("B") >= Math.Min(Math.Min(Hotel.BuilderNb, Restaurant.BuilderNb), SportsInfrastructure.BuilderNb))
                    {
                        Console.WriteLine("Entrez 1 pour créer un batiment");
                    }
                    else
                    {
                        Console.WriteLine("Vous n'avez pas assez de Colon pour construire un batiment");
                        buildBuilding = false;
                    }
                    if (_village.CanRecruit())
                    {
                        Console.WriteLine("Entrez 2 pour recruter un colon");
                    }
                    else
                    {
                        Console.WriteLine("Vous n'avez pas assez d'infrastructure pour accueillir des colons, ");
                        if (!_village.FreeHotelPlaces())
                        {
                            Console.WriteLine("Il vous faut un hotel");
                        }
                        if (!_village.FreeRestaurantPlaces())
                        {
                            Console.WriteLine("Il vous faut un restaurant");
                        }
                        recruitSettler = false;
                    }
                    if (_village.CanTrain())
                    {
                        Console.WriteLine("Entrez 3 pour entrainer un sportif");
                    }
                    else
                    {
                        trainAthetics = false;
                        Console.WriteLine("Vous n\'avez pas de sportif à entrainer");
                    }


                    bool creation = true;
                    int create = int.Parse(Console.ReadLine());
                    switch (create)
                    {
                        case 0:
                            break;
                        case 1:
                            {
                                if (buildBuilding == true)
                                    creation = ChoiceBuilding();
                                else
                                {
                                    Console.WriteLine("Vous n'avez toujours pas assez de Colon pour construire un batiment, entrez une réponse valide");
                                    creation = false;
                                }
                            }
                            break;
                        case 2:
                            if (recruitSettler == true)
                                creation = ChoiceSettler();
                            else
                            {
                                Console.WriteLine("Vous n'avez toujours pas assez d'infrastructure pour accueillir des colons, veuillez entrer une réponse valide ");
                                creation = false;
                            }
                            break;
                        case 3:
                            if (trainAthetics)
                            {
                                creation = _village.CanTrain();
                                Coach coach = _village.CanBeCoach();
                                Athletic athlete = ChooseAnAthlete();
                                if (coach != null)
                                {
                                    athlete.myCoach = coach;
                                }
                                athlete.Train(coach, _turnNb);
                            }

                            break;
                        default:
                            Console.WriteLine("Votre réponse n'est pas valide");
                            creation = false;
                            break;
                    }

                    if (creation == true)
                        _turnNb++;
                }
                else
                {
                    _turnNb++;
                }
                foreach (Settler settler in _village.GetSettlers())
                {
                    settler.Play(_village.GameBoardSettler, _turnNb);
                    Console.WriteLine(settler);
                }
            }

        }

        /// <summary>
        /// Shows the rules of the game
        /// </summary>
        public void RulesOfTheGame()
        {
            Console.WriteLine("----------LES REGLES DU JEU----------");
            Console.WriteLine("OBJECTIF : \n\nL'objectif de ce jeu est simple : construire un village olympique assez développé afin de lancer les Jeux Olympiques 2022! \nPour cela, il faudra que le village contienne au moins 3 sportifs suffisamment entraînés (soit au niveau ...), accompagné bien evidemment de leur infrastructure nécéssaire pour s'entrainter et participer aux JO, et des hotels et restaurants nécéssaire pour les accueil. \nDe plus, il faudra avoir réaliser tout cela avant avoir atteint le 100ième tour du jeu!\n\n ");//TODO Ajouter le niveau d'entrainement des athlètes pour gagner!!!
            Console.WriteLine("INFORMATIONS SUPPLEMENTAIRES : \n");
            Console.WriteLine("Il existe 3 types de colons : \n- Les batisseurs \n- Les sportifs \n- Les coachs\n");
            Console.WriteLine("Il existe 7 catégories de sportifs : \n- Les nageurs \n- Les volleyeur \n- Les handballeur \n- Les basketteur \n- Les footballeur \n- Les rugbyman \n- Les athlètes\n");
            Console.WriteLine("Il existe 3 types de batiments : \n - Les hotels \n - Les restaurants \n - Les infrastructures sportives\n");
            Console.WriteLine("De plus, il existe aussi 7 types d'infrastructuresportives : \n- Une piscine olympique \n- Un terrain de volleyball \n- Un terrain de handball \n- Un terrain de basketball \n- Un terrain de football \n- Un terrain de rugby \n- Une piste d'athlétisme\n");
            Console.WriteLine("Pour accueilr un nouveau colon, il est donc nécéssaire qu'il y ait encore une place  disponnible dans un hotel et dans un restaurant. De plus, si ce colon est un sportif, il faut qu'il y ait déjà dans le village l'infrastructure sportive correspondant à sa discipline (par exemple, un il faut qu'il y ait un terrain de football pour recruter un footballeur)\n");
            Console.WriteLine("Afin de construire les infrastructures nécéssaire, il faut que des batisseurs soient réquisitionnés, qu'ils se déplacent sur la surface de construction, et qu'il prennent le temps de batire la batiment. \n- Un hotel necessite 2 batisseurs et est construit en 2 tours \n- Un restaurant necessite 2 batisseurs et est construit en 2 tours \n- Une infrastructure sportive necessite 2 batisseurs et est construit en 1 tours\n");
            Console.WriteLine("Il est donc possible qu'à certains moments du jeu, vous ne pouvez pas recruter de colons car il n'y as pas l'espace nécéssaire dans les hotels est restaurants, ou alors que vous ne pouvez pas construire de nouveau batiments car vous n'avez pas assez de batisseur. Mais ne vous en faites pas, cela vous sera indiqué.\n");
            Console.WriteLine("A chaque tour du jeu, en fonction de ce que vous pouvez faire, il vous sera donc proposé de construire un batiment, de recruter un colon ou d'entainer un sportif. Cependant, vous avez aussi la possibilité de passer le tour si vous ne souhaitez réaliser aucune action.\n");
            Console.WriteLine("L'objectif étant d'avoir des sportif ayant un certain niveau, il faudra donc les entrainer. Plus vous entrainez un sportif, plus son niveau augmentera.\n");
            Console.WriteLine("Il est important de savoir que lorsqu'un sportif s'entraine avec un coach, son niveau augmente plus vite\n\n");
            Console.WriteLine("A vous de jouer maintenant, bonne chance !\n");
        }

        /// <summary>
        /// Allows us to train athletes
        /// </summary>
        /// <returns>We return the athlete we train</returns>
        public Athletic ChooseAnAthlete()
        {
            Athletic athlete = null;
            List<Settler> settlers = _village.GetSettlers();
            for (int i=0;  i < settlers.Count; i++)
            {
                if (settlers[i] is Athletic && settlers[i].Available)
                {
                    athlete = (Athletic)settlers[i];
                    Console.WriteLine("Tapez {0} pour entrainer le {1} {2}", athlete.AthleticNb, athlete.Sport, athlete.Nationality);
                }
            }
            int id = int.Parse(Console.ReadLine());
            return _village.FindById(id);
        }

        /// <summary>
        /// Allows you to know if actions are possible on this turn
        /// </summary>
        /// <returns>If we can perform actions on this turn it returns true, otherwise it returns false</returns>
        public bool Proceed() 
        {
            bool proceed = false;
            if (_village.NbSettlerAvailable("B") >= Math.Min(Math.Min(Hotel.BuilderNb, Restaurant.BuilderNb), SportsInfrastructure.BuilderNb))
            {
                proceed = true;
            }
            else
            {
                if (_village.CanRecruit())
                {
                    proceed = true;
                }
            }
            return proceed;
        }


        /// <summary>
        /// Create the buildings that are being created if it is their turn to create
        /// </summary>
        public void PendingBuildingCreation()
        {
            foreach ( InConstructionBuilding inConstruction in _village.InConstruction)
            {
                if (inConstruction.CreationTurn == _turnNb) 
                { 
                    if(inConstruction.BuildingType == "H")
                    {
                        Hotel hotel = new Hotel(inConstruction.X, inConstruction.Y);
                        _village.AddBuildings(hotel); 
                        _village.CreationBuilding(hotel);
                    }
                    else if(inConstruction.BuildingType == "R")
                    {
                        Restaurant restaurant = new Restaurant(inConstruction.X, inConstruction.Y);
                        _village.AddBuildings(restaurant);
                        _village.CreationBuilding(restaurant);
                    }
                    else
                    {
                        SportsInfrastructure sportsInfrastructurel = new SportsInfrastructure(inConstruction.X, inConstruction.Y, inConstruction.Name);
                        _village.AddBuildings(sportsInfrastructurel); 
                        _village.CreationBuilding(sportsInfrastructurel);
                        _village.SportsInfrastructures[inConstruction.Name] = true;
                    }
                    foreach (Settler builder in inConstruction.Settlers)
                    {
                        builder.IsInActivity = false;
                    }
                }
             
            }
        }

        /// <summary>
        /// Displays the game board
        /// </summary>
        public void DisplayGameBoard()
        {
            for (int i = 0; i < _village.Lenght; i++)
            {
                Console.Write("\n");
                for (int j = 0; j < _village.Width; j++)
                {
                    string info = " ";
                    ConsoleColor foreGroundColor = ConsoleColor.White;
                    ConsoleColor backGroundColor = ConsoleColor.Black;
                    if (_village.GameBoardBuilder[i, j] is null && _village.GameBoardSettler[i, j].Count == 0)
                    {
                        Console.Write(".\t");
                    }
                    else if (_village.GameBoardSettler[i, j].Count == 0 && (_village.GameBoardBuilder[i, j] == "XH" || _village.GameBoardBuilder[i, j] == "XR" || _village.GameBoardBuilder[i, j] == "XS"))
                    {
                        if (_village.GameBoardBuilder[i, j] == "XH")
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("X\t");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (_village.GameBoardBuilder[i, j] == "XR")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X\t");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (_village.GameBoardBuilder[i, j] == "XS")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("X\t");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        if (_village.GameBoardBuilder[i, j] != null)
                        {
                            if (foreGroundColor == ConsoleColor.White)
                            {
                                foreGroundColor = ConsoleColor.Black;
                            }
                            else
                            {
                                foreGroundColor = ConsoleColor.Red;
                            }
                            if (_village.GameBoardBuilder[i, j].Equals("H"))
                            {
                                backGroundColor = ConsoleColor.Cyan;
                            }
                            else if (_village.GameBoardBuilder[i, j].Equals("R"))
                            {
                                backGroundColor = ConsoleColor.Red;
                            }
                            else if (_village.GameBoardBuilder[i, j].Equals("S"))
                            {
                                backGroundColor = ConsoleColor.Green;
                            }
                        }
                        Console.BackgroundColor = backGroundColor;
                        if (_village.GameBoardSettler[i, j] != null)
                        {
                            foreach(Settler settler in _village.GameBoardSettler[i, j]) { 
                                foreGroundColor = settler.Available ? ConsoleColor.White : ConsoleColor.Red;
                                if (settler is Builder)
                                {
                                    info = "B";
                                }
                                else if (settler is Athletic)
                                {
                                    info = "A";
                                }
                                else if (settler is Coach)
                                {
                                    info = "C";
                                }
                                Console.ForegroundColor = foreGroundColor;
                                Console.Write(info);
                            }
                        }
                        Console.Write("\t");
                        Console.ResetColor();
                    }
                }
            }
            Console.Write("\n");
        }


        /// <summary>
        ///Check if the space where you want to build a building is not already occupied or if it does not come out of the board
        /// </summary>
        /// <param name="type">type of building ("H" for a hotel, "R" for a restaurant and "S" for a sports infrastructure)</param>
        /// <param name="x">Abscissa of the position on the board of the top left corner of the building you want to build</param>
        /// <param name="y">Ordinate of the position on the board of the top left corner of the building you want to build</param>
        /// <returns></returns>
        public bool FreeSpaceBuilding(string type, int x, int y) 
        {
            int lines;
            int columns;
            lines = Building.GetLinesNb(type);
            columns = Building.GetColumnsNb(type);
            if (x + lines >= _village.GameBoardBuilder.GetLength(0) || y + columns >= _village.GameBoardBuilder.GetLength(1))
            {
                Console.WriteLine("Vous ne pouvez pas construire ici, vous sortirez du plateau de jeu");
                return false;
            }
            for (int i = x; i <= x + lines - 1; i++)
            {
                for (int j = y; j <= y + columns - 1; j++)
                {
                    if (_village.GameBoardBuilder[i, j] != null)
                    {
                        Console.WriteLine("Tu ne peux pas construire sur un batiment qui existe déjà ou qui est en cours de construction, choisi un autre emplacement!");
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Allows us to choose the type of building we want to build
        /// </summary>
        /// <returns>Returns true if the player has chosen to build a building, false if the player has finally changed his mind</returns>
        public bool ChoiceBuilding()
        {
            Console.WriteLine("Entrez 0 pour revenir en arrière");
            bool creation = false;
            bool createHotel = false;
            bool createRestaurant = false;
            bool createSportsInfrastructure = false;
            if (_village.NbSettlerAvailable("B") >= Hotel.BuilderNb)
            {
                Console.WriteLine("Entrez 1 pour créer un Hotel");
                createHotel = true; 
            }
            if (_village.NbSettlerAvailable("B") >= Restaurant.BuilderNb)
            {
                Console.WriteLine("Entrez 2 pour créer un Restaurant");
                createRestaurant = true;
            }
            if (_village.NbSettlerAvailable("B") >= SportsInfrastructure.BuilderNb)//TODO les conditions pour construire une infrastructure ne sont pas les mêmes
            {
                Console.WriteLine("Entrez 3 pour créer une Infrastructure Sportive");
                createSportsInfrastructure = true;
            }
            int create = int.Parse(Console.ReadLine());

            if (create == 1 || create == 2 || create == 3)
            {
                Console.WriteLine("Entrez la ligne de l'angle en haut à gauche de votre batiment");
                int x = int.Parse(Console.ReadLine());
                Console.WriteLine("Entrez la colonne de l'angle en haut à gauche de votre batiment");
                int y = int.Parse(Console.ReadLine());

                if (create == 1)
                {
                    if (createHotel == true)
                    {
                        if (FreeSpaceBuilding(Hotel.type, x, y))
                        {
                            CreateBuilding(Hotel.type, x, y);
                            creation = true;
                        }
                    }
                    else
                        Console.WriteLine("Cette réponse n'est pas valide car tu n'as pas assez de batisseur pour construire un hotel, choisi une réponse qui t'es proposé");

                }
                else if (create == 2)
                {
                    if (createRestaurant == true)
                    {
                        if (FreeSpaceBuilding(Restaurant.type, x, y))
                        {
                            CreateBuilding(Restaurant.type, x, y);
                            creation = true;
                        }
                    }
                    else
                        Console.WriteLine("Cette réponse n'est pas valide car tu n'as pas assez de batisseur pour construire un restaurant, choisi une réponse qui t'es proposé");
                }
                else if (create == 3)
                {
                    if (createSportsInfrastructure == true)
                    {
                        if (FreeSpaceBuilding(SportsInfrastructure.type, x, y))
                        {
                            CreateBuilding(SportsInfrastructure.type, x, y);
                            creation = true;
                        }
                    }
                    else 
                        Console.WriteLine("Cette réponse n'est pas valide car tu n'as pas assez de batisseur pour construire une infrastructure sportive, choisi une réponse qui t'es proposé");
                }
            }
            else if (create == 0)
                creation = false;
            else
            {
                Console.WriteLine("Les données que vous avez entrées concernant le type de batiment que vous souhaitez construire ne sont pas valides. Veuillez saisir des informations valables \n");
                ChoiceBuilding();
            }
            return creation;
        }

        /// <summary>
        /// Allows you to create a building
        /// </summary>
        /// <param name="type">Type of building to be created ("R" for a restaurant, "H" for a hotel and "S" for a sports infrastructure)</param>
        /// <param name="x">Abscissa in the village of the position of the top left corner of the building to be created</param>
        /// <param name="y">Ordinate in the village of the position of the top left corner of the building to be created</param>
        public void CreateBuilding(string type, int x, int y)
        {
            int nbBuilders = 0;
            string name = "";
            int turnNb = 0;
            if (type == "H")
            {
                nbBuilders = Hotel.BuilderNb;
                turnNb = Hotel.TurnNb;
            }
            else if (type == "R")
            {
                nbBuilders = Restaurant.BuilderNb;
                turnNb = Restaurant.TurnNb;
            }
            else if (type == "S")
            {
                name = ChoiceSportsInfrastructure();
                nbBuilders = SportsInfrastructure.BuilderNb;
                turnNb = SportsInfrastructure.TurnNb;
            }
            List<Settler> settlers = BusyBulderList(nbBuilders);
            foreach (Settler settler in settlers)
            {
                settler.CalculatingItinerary(x, y);
            }
            InConstructionBuilding inConstruction = new InConstructionBuilding(type, turnNb + _turnNb, x, y, settlers, name);
            _village.InConstruction.Add(inConstruction);
            _village.CreatePendingBuilding(inConstruction);
        }



        /// <summary>
        /// Allows the player to choose the sports infrastructure he wishes to create
        /// </summary>
        /// <returns>returns true if he has selected a sports infrastructure to create, false if ultimately he does not want to create a sports infrastructure</returns>
        public string ChoiceSportsInfrastructure()
        {
            Console.WriteLine("Choisissez l'infrastructure sportive que vous souhaitez construire");
            Console.WriteLine("Entrez 1 pour une piscine olympique \nEntrez 2 pour un terrain de sport collectif intérieur \nEntrez 3 pour un stade");
            int infrasctructure = int.Parse(Console.ReadLine());
            string sportsinfrasctructure = "";
            if (infrasctructure == 1)
                sportsinfrasctructure = "Piscine olympique";
            else if (infrasctructure == 2)
            {
                sportsinfrasctructure = "Terrain de sport collectif intérieur";
            }
            else if (infrasctructure == 3)
            {
                sportsinfrasctructure = "Stade";
            }
            else
            {
                Console.WriteLine("Votre réponse n'est pas valable. Veuillez entrer une réponse valide");
                ChoiceSportsInfrastructure();
            }
            return sportsinfrasctructure; 
        }

        /// <summary>
        /// Available bulder assigment at the building
        /// </summary>
        /// <param name="nbBuilders">The number of builder assigments</param>
        /// <returns>A list of builder assigment's</returns>
        public List<Settler> BusyBulderList(int nbBuilders)
        {
            List<Settler> busyBuilder = new List<Settler>();
            int nbAvailable = 0;
            for (int i = 0; i < nbBuilders; i++)
            {
                nbAvailable = _village.FindAvailable("B").Count();
                Console.WriteLine(nbAvailable);
                busyBuilder.Add(_village.FindAvailable("B")[0]);
                _village.FindAvailable("B")[0].IsInActivity = true;
            }

            return busyBuilder;

        }

        /// <summary>
        /// Allows the player to choose the type of settler they wish to recruit
        /// </summary>
        /// <returns>returns true if he has selected a settler to recruit, false if he does not want to recruit a settler</returns>
        public bool ChoiceSettler()
        {
            Console.WriteLine("Entrez 0 pour revenir en arrière");
            Console.WriteLine("Entrez 1 pour recruter un Batisseur");
            Console.WriteLine("Entrez 2 pour recruter un Coach");
            if (CanRecruitAthlete())
                Console.WriteLine("Entrez 3 pour recruter un Sportif");
            else
                Console.WriteLine("Vous ne pouvez pas recruter un sportif car vous n'avez aucune infrastructure sportive");
            int create = int.Parse(Console.ReadLine());
            bool creation = true;
            if (create ==0)
                creation = false;
            else if (create == 1)
            {

                Builder builder = new Builder();
                _village.AddSettler(builder);
            }
            else if (create == 2)
            {
                Coach coach = new Coach();
                _village.AddSettler(coach);
                Athletic.LevelIncrease++;
                Console.WriteLine("Athletic.LevelIncrease : " + Athletic.LevelIncrease);
            }
            else if (create == 3)
            {
                bool createAthletic = ChoiceAthletics();
                if (createAthletic == false)
                    ChoiceSettler();
            }
            else
            {
                Console.WriteLine("Votre réponse n'est pas valide");
                ChoiceSettler();
            }
            return creation;
        }

        /// <summary>
        /// Allows the player to choose the athlete he wishes to recruit (his sport and his nationality)
        /// </summary>
        /// <returns>returns true if he has recruited an athlete, false if he does not want to recruit one</returns>
        private bool ChoiceAthletics()
        {
            bool createAthletics = true;

            string nationality2 = "";
            while (nationality2 == "")
            {
                Console.WriteLine("Entrez 0 pour revenir en arrière");
                Console.WriteLine("Choisissez sa nationnalité \nEntrez 1 pour que le sportif soit Français \nEntrez 2 pour que le sportif soit Anglais\nEntrez 3 pour que le sportif soit Américain\nEntrez 4 pour que le sportif soit Japonais");//A en rajouter
                int nationality = int.Parse(Console.ReadLine());

                if (nationality == 0)
                {
                    createAthletics = false; 
                    nationality2 = "rien";
                }
                else if (nationality == 1)
                    nationality2 = "Francais";
                else if (nationality == 2)
                    nationality2 = "Anglais";
                else if (nationality == 3)
                    nationality2 = "Americain";
                else if (nationality == 4)
                    nationality2 = "Japonais";
                else
                    Console.WriteLine("Votre réponse n'est pas valide, veuillez entrer un numéro valide");
            }

            if (createAthletics == true)
            {
                string sport2 = "";
                string infrastructure = "";
                bool swimingPool = false;
                bool field = false;
                bool stage = false;
                bool value;
                while (sport2 == "")
                {
                    Console.WriteLine("Choisissez son sport :");
                    _village.SportsInfrastructures.TryGetValue("Piscine olympique", out value);
                    if (value == true)
                    {
                        Console.WriteLine("Entrez 1 pour de la natation ");
                        swimingPool = true;
                    }
                    else
                    {
                        _village.SportsInfrastructures.TryGetValue("Terrain de sport collectif intérieur", out value);
                        if (value == true)
                        {
                            Console.WriteLine("Entrez 2 pour du volley ");
                            Console.WriteLine("Entrez 3 pour du hand");
                            Console.WriteLine("Entrez 4 pour du basket");
                            field = true;
                        }
                        else
                        {
                            _village.SportsInfrastructures.TryGetValue("Stade", out value);
                            if (value  == true)
                            {
                                Console.WriteLine("Entrez 5 pour du football ");
                                Console.WriteLine("Entrez 6 pour du rugby");
                                Console.WriteLine("Entrez 7 pour de l'athlétisme");
                                stage = true;
                            }
                        }
                    }
                    
                    int sport = int.Parse(Console.ReadLine());

                    if (sport == 1 && swimingPool)
                    {
                        sport2 = "Natation";
                    }
                    else if (field)
                    {
                        if (sport == 2)
                        {
                            sport2 = "Volley";
                        }
                        else if (sport == 3)
                        {
                            sport2 = "Hand";
                        }
                        else if (sport == 4 && field)
                        {
                            sport2 = "Basketball";
                        }
                    }
                    else if (stage)
                    {
                        if (sport == 5)
                        {
                            sport2 = "Football";
                        }
                        else if (sport == 6 && stage)
                        {
                            sport2 = "Rugby";
                        }
                        else if (sport == 7 && stage)
                        {
                            sport2 = "Athlétisme";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Votre réponse n'est pas valide, veuillez entrer un numéro valide");
                    }
                }// TODO Attention tester avec le mauvais numéros

                    Athletic athletic = new Athletic(nationality2, sport2, _village);
                    _village.AddSettler(athletic);
                    Console.WriteLine("Vous avez recruté un nouveau sportif : ");
                    Console.WriteLine(athletic);
            }
            return createAthletics;
        }

        /// <summary>
        /// Allows you to know if you can rectify an athlete (i.e. if there is a sports infrastructure present on the game board)
        /// </summary>
        /// <returns>returns true if we have a sports infrastructure and therefore if we can recruit an athlete, false otherwise</returns>
        public bool CanRecruitAthlete()
        {
            bool canRecruitAthlete = false;
            foreach (Building building in _village.Buildings)
                if (building is SportsInfrastructure)
                    canRecruitAthlete = true;
            return canRecruitAthlete;
        }
    }
}