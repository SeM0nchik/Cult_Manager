using JsonObject;
using Parser;
using Terminal.Gui;
namespace MenuLibrary
{
    public sealed class MenuAdd : Menu
    {
      
        public MenuAdd() : base("Изменить данные",
            new[]
            {
                "Дозагрузить данные", "Редактировать данные", "Удалить данные", "Объеденить данные из 2ух наборов",
                "Вернуться в главное меню"
            })
        {
            Actions = new[] { AddData, EditData, DeleteData, MergeData, Return};
        }

        public MenuAdd(string sortStatis, string filterStatus, CultCollection allCults, CultCollection selectedCults) : this()
        {
            SortStatus = sortStatis;
            FilterStatus = filterStatus;
            AllCults = allCults;
            CurrentCults = selectedCults;
        }
        
        /// <summary>
        /// A method that adds data to cult collection.
        /// </summary>
        private void AddData()
        {
            Application.Shutdown();
            MenuRead menu = new(SortStatus, FilterStatus, AllCults, CurrentCults);
            menu.Run();
        }
        
        /// <summary>
        /// A method that deletes data from the collection.
        /// </summary>
        private void DeleteData()
        {

            if (CurrentCults.Length > 0)
            {
                string printed;
                Func<string,string> printer = x => $"Список {x} культов для удаления :";
                CurrentCults.PrintField("Id", printer, out printed);
                string text = printed;
                text += "Введите Id культа, который вы хотите удалить: " + Environment.NewLine;

                string result = GetDialogInput(text);
                if (CurrentCults.FindId(result) == null)
                {
                    throw new Exception("Вы ввели несуществующее Id культа. Попробуйте еще раз!");
                }
                CurrentCults.DeleteId(result);
                MessageBox.Query("Уведомление" ,"Данные были успешно удалены.", "OK");
            }
            else
            {
                MessageBox.Query("Ошибка" ,"Остутствуют данные о культах. Пожалуйста зазрузите данные.", "OK");
            }

        }

        /// <summary>
        /// A method that edits cult with corresponding id. If cult is not found, new cult will be initialized
        /// </summary>
        private void EditData()
        {
            try
            {
                string printed;
                Predicate<string?> check = x => x == null || CurrentCults.FindId(x) == null;
                Func<string, string> printer = x => $"Список {x} культов для редактирования:";
                CurrentCults.PrintField("Id", printer, out  printed);
                string text = printed;
                text += "Введите Id культа, который вы хотите отредактировать: ";

                string result = GetDialogInput(text);
                
                if (check(result))
                {
                    MessageBox.Query("Уведомление","Искомый культ не найден. Будет создан новый экземпляр", "OK");
                }

                string input = GetDialogInput("Введите культ в формате объекта json. Для окончания ввода нажмите OK");
                JsonParser.GetSingleJsonObject(input, out Cult cult);

                if (check(result))
                {
                    CurrentCults.AddCult(cult);
                }
                else
                {
                    CurrentCults.ChangeCult(result, cult);
                }

                MessageBox.Query("Уведомление","Данные были успешно изменены.", "OK");
            }
            catch (Exception ex)
            {
                MessageBox.Query("Ошибка",ex.Message, "OK");
            }
        }


        /// <summary>
        /// A method that create a MergeToolMenu.
        /// </summary>
        private void MergeData()
        {
            Application.Shutdown();
            MergeToolMenu menu = new MergeToolMenu(SortStatus, FilterStatus, AllCults, CurrentCults);
            menu.Run();
        }
    }
}