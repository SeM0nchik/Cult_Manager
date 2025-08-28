using JsonObject;
namespace MenuLibrary
{
    /// <summary>
    /// Exit menu.
    /// </summary>
    public sealed class MenuExit : Menu
    {

        
        /// <summary>
        /// Primary constructor.
        /// </summary>
        public MenuExit() : base("Выход", new []{"Выход из программы", "Вернуться в главное меню"})
        {
            Actions = new []{ () => Clear(), Return};
        }

        private void Clear()
        {
            Console.Clear();
            Environment.Exit(0);
        }
        
        public MenuExit(string sortStatis, string filterStatus, CultCollection allCults, CultCollection selectedCults) : this()
        {
            SortStatus = sortStatis;
            FilterStatus = filterStatus;
            AllCults = allCults;
            CurrentCults = selectedCults;
        }
    }
}
