using Terminal.Gui;

namespace MenuLibrary
{
    /// <summary>
    /// A menu class that filters all cults.
    /// </summary>
    public sealed class MenuFiltering : Menu
    {
        
        /// <summary>
        /// Field to filter our cults.
        /// </summary>
        private string Field { get; set; } = string.Empty;

      
        public MenuFiltering() : base("Фильтрация", new[] { "Выбрать поле для фильтра.", "Отменить все фильтры.", "Вернуться в главное меню." })
        {
            Actions = new []{Filter, DeleteFilters, Return};
        }

        /// <summary>
        /// Delete all filters.
        /// </summary>
        private void DeleteFilters()
        {
            FilterStatus = "Фильтры отсутствуют";
            CurrentCults = AllCults;
        }
        
       /// <summary>
       /// Specific filtering settings.
       /// </summary>
        private void FilterSettings()
        {
            string[] all = new[]
            {
                "Id", "Label", "Aspects", "Slots", "Unique", "Description"
            };

            string text = "Выберите поле для фильтра:" + Environment.NewLine;
            int index = 1;
            foreach (string field in all)
            {
                text += $"{index++}. {field}" + Environment.NewLine;
            }
            text += $"Введите число от 1 до {all.Length}:";
            
            int result;
            
            while (true)
            {
                string input = GetDialogInput(text);
                
                if (!int.TryParse(input, out result))
                {
                    MessageBox.Query("Ошибка"," Введенные данные не являются числом. Попробуйте еще раз.", "OK");
                    continue;
                }
                
                if (result > 0 && result <= all.Length)
                {
                    break;
                }
                MessageBox.Query("Ошибка",$"Число должно быть от 1 до {all.Length}. Попробуйте еще раз.", "OK");
            }
            
            Field = all[result - 1];
        }
       
       /// <summary>
       /// Specific filtering additional settings: user need to choose objects that he/she wants to leave.
       /// </summary>
       /// <exception cref="Exception">If user don't choose any objects, we don't filter our collection. </exception>
        private void AdditionalFilterSettings()
        {
            if (Field == "Aspects" || Field == "Slots")
            {
                //Coming soon.
               MessageBox.Query("Ошибка", "Извините, но фильтрация по такому полю не производится", "OK");
            }
            else
            {
                string printed;
                //In fact, you can choose only one field to filter, but you can do that multiple times.
                Func<string, string> printer = x => $"Доступные для выбора объекты по полю {x}";
                CurrentCults.PrintField(Field, printer, out printed);
                printed += Environment.NewLine + "Введите значения объектов, которые вы хотите оставить";
                
                string line = GetDialogInput(printed);
                List<string?> idlist = new List<string?>();
                
                //We ask to input objects until there is an empty string.
                while (line != string.Empty)
                {
                    idlist.Add(line);
                    line = GetDialogInput(printed);
                }

                if (idlist.Count == 0)
                {
                    throw new Exception("Вы не выбрали ли одного из доступных объектов.");
                }
                
                CurrentCults = CurrentCults.Filter(Field, idlist);
            }
        }
       
       /// <summary>
       /// Main method of menu that starts a chain of interaction with user.
       /// </summary>
        public void Filter()
        {
            try
            {
                FilterSettings();
                AdditionalFilterSettings();
                if (FilterStatus != "Фильтры отсутствуют")
                {
                    FilterStatus += $", {Field}";
                }
                else
                {
                    FilterStatus = $"Установлены фильтры по полям: {Field}";
                }
                MessageBox.Query("Уведомление", "Фильтры успешно установлены.", "OK");
            }
            catch (Exception ex)
            {
                MessageBox.Query("Ошибка", ex.Message, "OK");
            }
            
        }

    }
}