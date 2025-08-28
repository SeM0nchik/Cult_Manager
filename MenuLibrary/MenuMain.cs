using JsonObject;
namespace MenuLibrary
{
    /// <summary>
    /// Main menu in console application that interacts with user.
    /// </summary>
    public class MenuMain : Menu
    {
        /// <summary>
        /// All menu that are available.
        /// </summary>
        private readonly Menu[] _menuItems = new Menu[]{ 
            new MenuRead(), new MenuFiltering(), new MenuSorting(), new MenuAdd(), new MenuPrint(), new MenuExit()
        };
        
        /// <summary>
        /// Primary constructor.
        /// </summary>
        public MenuMain() : base ("Главное меню", new[]
        {
            "Ввести данные", "Отфильтровать данные", "Отсортировать данные", "<основная задача> Изменить данные", "Вывести данные", "Выход"
        })
        {
            Actions[0] += () => CurrentCults = new CultCollection();
            Actions[0] += () => AllCults = new CultCollection();
            
            for (int i = 0; i < Actions.Length; i++)
            {
                Menu item = _menuItems[i];
                Actions[i] = () => item.Run();
            }
            
        }

        public MenuMain(string sortStatus, string filterStatus, CultCollection all, CultCollection cults) : this()
        {
            AllCults = all;
            CurrentCults = cults;
            SortStatus = sortStatus;
            FilterStatus = filterStatus;

            foreach (Menu menu in _menuItems)
            {
                menu.CurrentCults = CurrentCults;
                menu.FilterStatus = FilterStatus;
                menu.SortStatus = SortStatus;
                menu.AllCults = AllCults;
            }
        }
        
    }
}