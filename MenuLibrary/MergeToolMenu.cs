using JsonObject;
using  Terminal.Gui;
using Parser;
using System.Text;
using Terminal.Gui.Graphs;
namespace MenuLibrary
{
    public sealed class MergeToolMenu : FilePathMenu
    {
        /// <summary>
        /// First filepath to read data from.
        /// </summary>
        private string FirstFilePath { get;  set; } = string.Empty;
        
        /// <summary>
        /// First file name.
        /// </summary>
        public string FisrtFileName  => SecondFilepath.Split(Path.DirectorySeparatorChar)[^1];
        /// <summary>
        /// Second filepath to read data from.
        /// </summary>
        private string SecondFilepath { get;  set; } = string.Empty;
        
        /// <summary>
        /// Second file name.
        /// </summary>
        public string SecondFileName  => SecondFilepath.Split(Path.DirectorySeparatorChar)[^1];
        
        /// <summary>
        /// First collection of cults.
        /// </summary>
        public CultCollection FirstCultCollection { get; set; }
        
        /// <summary>
        /// Second collection of cults.
        /// </summary>
        public CultCollection SecondCultCollection { get; set; }
        
        
        public MergeToolMenu() : base("Объединение данных", new []{ "Считать данные из первого файла.", "Считать данные из второго файла",
            "Найти первый файл в директории", "Найти второй файл в директории","Объединить данные", "Вернуться в главное меню." })
        {
            Actions = new []{() => ReadAdditionalFile(1), () => ReadAdditionalFile(2), () => FileResearch(1), () => FileResearch(2), MergeData,  Return};
            SecondCultCollection = new CultCollection();
            FirstCultCollection = new CultCollection();
        }
        
        public MergeToolMenu(string sortStatus, string filterStatus, CultCollection allCults, CultCollection selectedCults) : this()
        {
            SortStatus = sortStatus;
            FilterStatus = filterStatus;
            AllCults = allCults;
            CurrentCults = selectedCults;
        }

        /// <summary>
        /// A method that runs the full process of reading from file.
        /// </summary>
        /// <param name="fileChoose">Which file to read.</param>
        private void ReadAdditionalFile(int fileChoose)
        {
            if (fileChoose == 1)
            {
                FirstFilePath = ReadFilePath();
            }
            else
            {
                SecondFilepath = ReadFilePath();
            }
            
            ReadFile(fileChoose);
        }
        
        /// <summary>
        /// A method that reads from a specific file.
        /// </summary>
        /// <param name="fileChoose">Which file to read.</param>
        private void ReadFile(int fileChoose)
        {
            CreateProgress();
            string filepath = fileChoose == 1 ? FirstFilePath : SecondFilepath;
            try
            {
                Task.Run(() =>
                {
                    using (StreamReader file = new(filepath, Encoding.UTF8))
                    {
                        Console.SetIn(file);
                        try
                        {
                            long totalBytes = new FileInfo(filepath).Length;
                            long total = (long)(totalBytes / 1.5);
                            if (fileChoose == 1)
                            {
                                FirstCultCollection.AddCultList(JsonParser.ReadJson(total, CurrentProgressBar ?? new ProgressBar()));
                            }
                            else
                            {
                                SecondCultCollection.AddCultList(JsonParser.ReadJson(total, CurrentProgressBar ?? new ProgressBar()));
                            }

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
                MessageBox.Query("Ошибка", ex.Message, "ОК");
                SecondCultCollection = new CultCollection();
            }
        }

        /// <summary>
        /// A method that creates a dialog in which you choose the cult you want to be in final collection.
        /// </summary>
        /// <param name="id">Cult's id.</param>
        /// <returns>Choice.</returns>
        private int Choosing(string id)
        {
            string firstText = FirstCultCollection.FindId(id)?.ToPrettyString() ?? string.Empty;
            string secondText = SecondCultCollection.FindId(id)?.ToPrettyString() ?? string.Empty;
            
            Dialog dialog = new Dialog("Выбор файла", 200, 45);
            
            int firstLineLength = firstText.Split('\n')[0].Length;
            int secondLineLength = secondText.Split('\n')[0].Length;
            
            int splitPoint = Math.Max(firstLineLength, secondLineLength) + 2;
            
            int width = splitPoint * 2;
            int height = Math.Max(secondText.Split('\n').Length, firstText.Split('\n').Length) + 10; 

            ScrollView scrollView = new ScrollView(new Rect(1, 1, 198, 40))
            {
                ContentSize = new Size(width, height) 
            };

      
            Label file1Label = new Label($"1. {FisrtFileName}")
            {
                X = 1,
                Y = 1
            };

            Label file2Label = new Label($"2. {SecondFileName}")
            {
                X = splitPoint + 2,
                Y = 1
            };

        
            Label cultInfo1 = new Label()
            {
                X = 1,
                Y = 2,
                Text = firstText,
                Width = splitPoint,
                Height = Dim.Fill() - 4,
                CanFocus = false,
            };

            Label cultInfo2 = new Label()
            {
                X = splitPoint + 5,
                Y = 2,
                Text = secondText,
                Width = Dim.Fill() - splitPoint - 3,
                Height = Dim.Fill() - 4,
                CanFocus = false,
            };
            
            Button chooseFile1Button = new Button("Выбрать культ из 1 файла")
            {
                X = 1,
                Y = Pos.Bottom(cultInfo1) + 1
            };

            Button chooseFile2Button = new Button("Выбрать культ из 2 файла")
            {
                X = splitPoint + 5,
                Y = Pos.Bottom(cultInfo2) + 1
            };

            int choice = 0;

            chooseFile1Button.Clicked += () =>
            {
                choice = 1;
                Application.RequestStop();
            };

            chooseFile2Button.Clicked += () =>
            {
                choice = 2;
                Application.RequestStop();
            };
            
            scrollView.Add(
                file1Label,
                cultInfo1,
                file2Label,
                cultInfo2,
                chooseFile1Button,
                chooseFile2Button
            );
            
            scrollView.Add(new LineView(Orientation.Vertical)
            {
                X = splitPoint + 1,
                Y = 0,
                Height = height
            });

            scrollView.SetFocus();
            
            dialog.Add(scrollView);
            
            Application.Run(dialog);
            

            return choice;
        }
        /// <summary>
        /// A method that merges data.
        /// </summary>
        private void MergeData()
        {
            CurrentCults = CultCollection.Merge(FirstCultCollection, SecondCultCollection, Choosing);
        }
        
        /// <summary>
        /// A method that runs the file manager.
        /// </summary>
        /// <param name="result">A number of the corresponding file.</param>
        private void FileResearch(int result)
        {
            FileDialog fileDialog = new FileDialog($"Файловый менеджер №{result}", Directory.GetCurrentDirectory());
            Application.Run(fileDialog);

            if (!string.IsNullOrEmpty(fileDialog.SelectedFile))
            {
                MessageBox.Query("Выбранный файл", $"Вы выбрали файл: {fileDialog.SelectedFile}", "OK");
                
                if (result == 2)
                {
                    SecondFilepath = fileDialog.SelectedFile;
                }
                else
                {
                    FirstFilePath = fileDialog.SelectedFile;
                }
            }
            ReadFile(result);
        }
    }
}