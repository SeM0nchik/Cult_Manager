using Terminal.Gui;
using JsonObject;
namespace MenuLibrary
{
    /// <summary>
    /// A menu that is used to sort a collection of cults.
    /// </summary>
    public sealed class MenuSorting : Menu
    {
        /// <summary>
        /// String variable that shows field we sort our collection of cults by.
        /// </summary>
        private string Field { get; set; } = "Не выбрано";

        /// <summary>
        /// Boolean variable that shows whether we need to do a reverse sorting or not.
        /// </summary>
        private bool Reverse { get; set; }
        /// <summary>
        /// String variable that shows the key of dictionary-field sorting.
        /// </summary>
        private string? Key { get; set; } = "";

        /// <summary>
        /// Primary constructor.
        /// </summary>
        public MenuSorting() : base("Сортировка", new[] { "Провести сортировку", "Вернуться в главное меню" } )
        {
            Actions = new []{Sort,  () => Return()};
        }
        
        public MenuSorting(string sortStatis, string filterStatus, CultCollection allCults, CultCollection selectedCults) : this()
        {
            SortStatus = sortStatis;
            FilterStatus = filterStatus;
            AllCults = allCults;
            CurrentCults = selectedCults;
        }
        
        /// <summary>
        /// Sort settings that defines the field and reversion of our sorting.
        /// </summary>
        private void SortSettings()
        {
            int index = 1;
            string[] all = new[]
            {
                "Id", "Label", "Description", "Aspects", "Unique", "Slots"
            };
            
            string text = "Выберите поле для сортировки:" + Environment.NewLine;
            foreach (string field in all)
            {
                text += $"{index++}. {field}" + Environment.NewLine;
            }
            text = text + Environment.NewLine + "Введите номер поля из предложенного списка: ";
            
            string input = GetDialogInput(text);
            int result;
            
            while (!(int.TryParse(input, out result) && result > 0 &&
                   result <= all.Length))
            {
                MessageBox.Query("Ошибка","Вы ввели некорректные данные. Пожалуйста, повторите ввод", "OK");
                input = GetDialogInput(text);
            }

            Field = all[result - 1];

            text = "1.Провести обычную сортировку" + Environment.NewLine + "2.Провести обратную сортировку";
            text += "Введите либо 1 либо 2:";
            
            input = GetDialogInput(text);
            while (!(int.TryParse(input, out result) && (result == 1 || result == 2)))
            {
                MessageBox.Query("Ошибка","Вы ввели некорректные данные. Пожалуйста, повторите ввод", "OK");
                input = GetDialogInput(text);
            }

            Reverse = result == 2;
        }

        /// <summary>
        /// Additional sort settings that are used in case of dictionary-fields that user chose for sorting.
        /// </summary>
        /// <exception cref="Exception">The key for sorting is undefined.</exception>
        private void AdditionalSortSettings()
        {
            switch (Field)
            {
                case "Aspects":
                    {
                        string text  = "Ключи для сортировки:" + Environment.NewLine;
                        if (CurrentCults.AllAspects.Count == 0)
                        {
                            MessageBox.Query("Ошибка","Ключи для сортировки отсутствуют", "OK");
                        }
                        else
                        {
                            for (int i = 0; i < CurrentCults.AllAspects.Count; i++)
                            {
                                text += $"{i + 1}. {CurrentCults.AllAspects[i - 1]}" + Environment.NewLine;
                            }
                            text += "Введите номер одиного из предложенных ключей для сортировки:";
                            
                            string input = GetDialogInput(text);
                            int key;
                            if (!(int.TryParse(input, out key) & (key >= 1) &
                                  (key <= CurrentCults.AllAspects.Count)))
                            {
                                throw new Exception("Такого ключа для сортировки не обнаружено.");
                            }

                            Key = CurrentCults.AllAspects[key - 1];
                        }

                        break;

                    }
            }
        }

        /// <summary>
        /// The main method of a class that starts a chain of user interactions. 
        /// </summary>
        private void Sort()
        {
            try
            {
                SortSettings();
                AdditionalSortSettings();
                CurrentCults.Sort(Field, Reverse, Key);
                MessageBox.Query("Уведомление", "Сортировка успешно проведена", "OK");
                SortStatus = $"""Сортировка проведена по полю: "{Field}" с инверсией: "{Reverse}".""";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}