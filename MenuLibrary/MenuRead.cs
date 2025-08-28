using JsonObject;
using Parser;
using System.Text;
using Terminal.Gui;
namespace MenuLibrary
{
    /// <summary>
    /// Data reading menu.
    /// </summary>
    public sealed class MenuRead : FilePathMenu
    {

        /// <summary>
        /// Filepath to read data from.
        /// </summary>
        public string Filepath { get; private set; } = string.Empty;

        
        public MenuRead():base("Чтение данных", new []{ "Ввести данные из файла","Найти файл в директории", "Считать данные из консоли", "Вернуться в главное меню." })
        {
            Actions = new Action[] {  ReadFromFile, FileResearch, ReadConsole,Return};
        }
        
        public MenuRead(string sortStatis, string filterStatus, CultCollection allCults, CultCollection selectedCults) : this()
        {
            SortStatus = sortStatis;
            FilterStatus = filterStatus;
            AllCults = allCults;
            CurrentCults = selectedCults;
        }
        /// <summary>
        /// A method that reads data from console.
        /// </summary>
        private void ReadConsole()
        {
            try
            {
                string text = "Введите данные в консоль:" + Environment.NewLine +
                              "Для завершения ввода с консоли нажмите ОК";
                
                string json =  GetDialogInput(text);
                
                CurrentCults.AddCultList(JsonParser.ReadJson(json));
                AllCults = CurrentCults.Copy();
                MessageBox.Query("Уведомление", "Данные успешно загружены", "ОК");
        
            }
            catch (Exception ex)
            {
                MessageBox.Query("Ошибка", ex.Message, "ОК");
            }
        }
        
        
        /// <summary>
        /// Method that reads data from file of from console.
        /// </summary>
        private void ReadFile()
        {
            CreateProgress();
            try
            {
                Task.Run(() =>
                {
                    using (StreamReader file = new(Filepath, Encoding.UTF8))
                    {
                        Console.SetIn(file);
                        try
                        {
                            long totalBytes = new FileInfo(Filepath).Length; 
                            long total = (long)(totalBytes / 1.5);
                    
                            CurrentCults.AddCultList(JsonParser.ReadJson(total, CurrentProgressBar ?? new ProgressBar()));
                            AllCults = CurrentCults.Copy();

                            Application.MainLoop.Invoke(() =>
                            {
                                MessageBox.Query("Уведомление", "Данные успешно загружены", "ОК");
                            });
                        }
                        catch (Exception ex)
                        {
                            Application.MainLoop.Invoke(() =>
                            {
                                MessageBox.Query("Ошибка", ex.Message, "ОК");
                            });
                        }

                        Console.SetIn(new StreamReader(Console.OpenStandardInput(), Encoding.UTF8));
                    }
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
            ReadFile();
        }
        
        /// <summary>
        /// A method that runs the full process of reading from selected file.
        /// </summary>
        private void ReadFromFile()
        {
            try
            {
                Filepath = ReadFilePath();
                ReadFile();
            }
            catch (Exception ex)
            {
                MessageBox.Query("Ошибка", ex.Message, "ОК");
            }
        }
    }
}