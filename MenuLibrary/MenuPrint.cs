using System.Text;
using Terminal.Gui;
using JsonObject;
namespace MenuLibrary
{
    /// <summary>
    /// Data printing menu.
    /// </summary>
    public sealed class MenuPrint : Menu
    {
        
        public string Filepath = Path.GetFullPath(@"..\..\..\..\WorkingFiles\PrintedResult.json".Replace('\\', Path.DirectorySeparatorChar));
        
        /// <summary>
        /// Primary constructor.
        /// </summary>
        public MenuPrint() : base("Вывод данных",new [] { "Записать данные в файл","Найти файл в директории","Вывести данные в консоль", "Вернуться в главное меню"} )
        {
            Actions =new []{ PrintToSelectedFile, FileResearch, PrintToConsole,  Return};
        }

        public MenuPrint(string sortStatis, string filterStatus, CultCollection allCults, CultCollection selectedCults) : this()
        {
            SortStatus = sortStatis;
            FilterStatus = filterStatus;
            AllCults = allCults;
            CurrentCults = selectedCults;
        }
        
        /// <summary>
        /// A method that sets a filepath where current cults collection will be saved.
        /// </summary>
        /// <exception cref="Exception">If user's input is incorrect we should notify him/her.</exception>
        private void SetFilePath()
        {

            string text = "Dведите абсолютный путь файлу, куда вы хотите сохранить результат работы консольного приложения."
                          + Environment.NewLine +
                          "Если вы не хотите вводить путь к файлу, то файл будет сохранён по умолчанию в папку WorkingFiles";

            string path = GetDialogInput(text);
            
            if (string.IsNullOrEmpty(path))
            {
            }
            else
            {
                if (!File.Exists(path))
                {
                    throw new Exception("Файла с таким путём не существует. Попробуйте ввести путь заново!");
                }
                else if (!(path.EndsWith(".json") || path.EndsWith(".txt")))
                {
                    throw new Exception("У введенного вами файла некорректное расширение!");
                }
                Filepath = Path.GetFullPath(path);
            }
        }
        
        /// <summary>
        /// A method that prints data to console.
        /// </summary>
        private void PrintToConsole()
        {
            
            using (StringWriter stringWriter = new StringWriter())
            {
                TextWriter current = Console.Out;
                Console.SetOut(stringWriter);
                
                Parser.JsonParser.WriteJsonPretty(CurrentCults);
                
                Console.SetOut(current);

                CustomOutput("Коллекция культов", stringWriter.ToString());
            }
        }
        
        /// <summary>
        /// Method that writes current cult collection to the file PrintedResult.json or print it to the console.
        /// </summary>
        private void PrintToFile()
        {
            CreateProgress();
            try
            {
                Task.Run(() =>
                {
                    using (StreamWriter fileReader = new StreamWriter(Filepath, false, Encoding.UTF8))
                    {
                        TextWriter current = Console.Out;
                        Console.SetOut(fileReader);

                        long total = CurrentCults.Length;

                        // Передаем ProgressBar для обновления прогресса
                        Parser.JsonParser.WriteJson(CurrentCults, total, CurrentProgressBar ?? new ProgressBar());

                        Console.SetOut(current);
                    }

                    Application.MainLoop.Invoke(() =>
                    {
                        MessageBox.Query("Уведомление", "Файл успешно сохранён", "ОК");
                    });
                });
        
                Application.Run(CurrentDialog);
            }
            catch (Exception ex)
            {
                Application.MainLoop.Invoke(() =>
                {
                    MessageBox.Query("Ошибка", ex.Message, "ОК");
                });
            }
        }

        /// <summary>
        /// A method that runs the full process of printing to selected file.
        /// </summary>
        private void PrintToSelectedFile()
        {
            try
            {
                SetFilePath();
                PrintToFile();
            }
            catch (Exception ex)
            {
                MessageBox.Query("Ошибка", ex.Message, "ОК");   
            }
        }
        
        /// <summary>
        /// A method that runs a file manager.
        /// </summary>
        private void FileResearch()
        {
            FileDialog fileDialog = new FileDialog("Файловый менеджер", Directory.GetCurrentDirectory());
            Application.Run(fileDialog);

            if (!string.IsNullOrEmpty(fileDialog.SelectedFile))
            {
                MessageBox.Query("Выбранный файл", $"Вы выбрали файл: {fileDialog.SelectedFile}", "OK");
                Filepath = fileDialog.SelectedFile;
            }
            
            PrintToFile();
        }
    }
}