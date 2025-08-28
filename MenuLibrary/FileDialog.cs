using Terminal.Gui;
namespace MenuLibrary
{
    internal class FileDialog : Dialog
    {
        private string _currentDirectory;
        private ListView _fileListView;
        private Label _statusLabel;
        private string _selectedFile;

        /// <summary>
        /// Gets the selected file path.
        /// </summary>
        public string SelectedFile => _selectedFile;

        
        public FileDialog(string title, string initialDirectory) : base(title, 60, 20)
        {
            _currentDirectory = initialDirectory;
            _selectedFile = string.Empty;

            _statusLabel = new Label($"Текущая директория: {_currentDirectory}")
            {
                X = 0, Y = 0, Width = Dim.Fill(), Height = 1
            };

            _fileListView = new ListView() { X = 0, Y = 1, Width = Dim.Fill(), Height = Dim.Fill() - 3 };

            _fileListView.OpenSelectedItem += OpenSelectedItem;
            UpdateFileList();

            Add(_statusLabel, _fileListView);

            Button okButton = new Button("ОК", is_default: true);
            okButton.Clicked += () =>
            {
                Application.RequestStop();
            };

            Button cancelButton = new Button("Отмена");
            cancelButton.Clicked += () =>
            {
                _selectedFile = string.Empty;
                Application.RequestStop();
            };

            AddButton(okButton);
            AddButton(cancelButton);
        }

        /// <summary>
        /// Updates the file list view with the current directory contents.
        /// </summary>
        private void UpdateFileList()
        {
            List<string> items = new List<string> { "На уровень выше" };

            try
            {
                items.AddRange(Directory.GetDirectories(_currentDirectory).Select(dir => $"{'\u2192'}{new DirectoryInfo(dir).Name}"));
                items.AddRange(Directory.GetFiles(_currentDirectory).Select(file => Path.GetFileName(file)));
            }
            catch (Exception ex)
            {
                items.Add($"Ошибка: {ex.Message}");
            }

            _fileListView.SetSource(items);
            _statusLabel.Text = $"Текущая директория: {_currentDirectory}";
        }

        /// <summary>
        /// Handles the selection of an item in the file list view.
        /// </summary>
        /// <param name="args">Event arguments containing the selected item.</param>
        private void OpenSelectedItem(ListViewItemEventArgs args)
        {
            string selected = args.Value.ToString() ?? string.Empty;

            if (selected == "На уровень выше")
            {
                GoToParentDirectory();
            }
            else if (selected.StartsWith("\u2192"))
            {
                string dirName = selected.Substring(1, selected.Length - 1);
                _currentDirectory = Path.Combine(_currentDirectory, dirName);
                UpdateFileList();
            }
            else
            {
                _selectedFile = Path.Combine(_currentDirectory, selected);
                _statusLabel.Text = $"Выбран файл: {_selectedFile}";
                Application.RequestStop();
            }
        }

        /// <summary>
        /// Navigates to the parent directory.
        /// </summary>
        private void GoToParentDirectory()
        {
            DirectoryInfo? parent = Directory.GetParent(_currentDirectory);
            if (parent != null)
            {
                _currentDirectory = parent.FullName;
                UpdateFileList();
            }
        }
    }
}