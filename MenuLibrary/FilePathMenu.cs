namespace MenuLibrary
{
    /// <summary>
    /// Path reading menu.
    /// </summary>
    public abstract class FilePathMenu : Menu
    {
   
        public FilePathMenu()
        {
            Actions = new Action[] {};
        }
        
        public FilePathMenu(string name, string[] items) : base(name, items) {}
        
        /// <summary>
        /// A method that reads a filepath.
        /// </summary>
        protected string ReadFilePath()
        {
            string input = GetDialogInput(
                "Введите абсолютный или относительный путь к файлу."+ Environment.NewLine + "Учтите, что корректными являются файлы с расширением .txt" +
                " или .json.");
            
            input = Path.GetFullPath(input);

            if (!File.Exists(input))
            {
                throw new Exception("Файла с таким путём не существует. Попробуйте ввести путь заново!");
            }
            else if (!(input.EndsWith(".json") || input.EndsWith(".txt")))
            {
                throw new Exception("Файл имеет некорректное расширение");
            }

            return input;
        }
        
    }
}