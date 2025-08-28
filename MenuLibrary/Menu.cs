using JsonObject;
using Terminal.Gui;
namespace MenuLibrary
{
    /// <summary>
    /// Abstract class representing the skeleton of a menu. It generalizes all methods and properties inherent to this class.
    /// </summary>
    public abstract class Menu
    {
        protected Toplevel CurrentToplevel;
        protected Window CurrentWindow;
        
        protected ProgressBar? CurrentProgressBar;
        protected Dialog? CurrentDialog;

        /// <summary>
        /// Menu name.
        /// </summary>
        protected string MenuName { get; set; }

        /// <summary>
        /// Current state of sorting.
        /// </summary>
        public string SortStatus { get; set; } = "Сортировка не проведена";

        /// <summary>
        /// Current state of filters.
        /// </summary>
        public string FilterStatus { get; set; } = "Фильтры отсутствуют";

        protected Action[] Actions { get; set; }

        protected string[] ItemsNames { get; set; }
        
        /// <summary>
        /// Collection of visitors that we are working with.
        /// </summary>
        public  CultCollection CurrentCults { get; set; }
        
        public  CultCollection AllCults { get; set; }

        protected void Return()
        {
            Application.Shutdown();
            MenuMain main = new MenuMain(SortStatus, FilterStatus, AllCults.Copy(), CurrentCults.Copy());
            main.Run();
        }
        protected Menu()
        {
            MenuName = "";
            ItemsNames = new string[0];
            CurrentToplevel = new Toplevel();
            CurrentWindow = new Window();
            CurrentCults = new CultCollection();
            AllCults = new CultCollection();
            Actions = new Action[0];
        }
        protected Menu(string menuName, string[] itemNames) : this()
        {
            MenuName = menuName;
            ItemsNames = itemNames;
            Actions = new Action[ItemsNames.Length];
        }
        
        protected Menu(string sortStatus, string filterStatus, CultCollection allCults, CultCollection selectedCults, string menuName, string[] itemsNames) : this(menuName, itemsNames)
        {
            SortStatus = sortStatus;
            FilterStatus = filterStatus;
            AllCults = allCults;
            CurrentCults = selectedCults;
        }
        
        /// <summary>
        /// Method that displays the menu to the user.
        /// </summary>
        protected virtual void DisplayMenu()
        {
            CurrentToplevel = Application.Top;

            Colors.Base.Normal = Application.Driver.MakeAttribute(Color.Gray, Color.Black);
            Colors.Base.Focus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Black);

            CurrentWindow = new Window(MenuName)
            {
                X = 0,
                Y = 1, // Оставляем место для меню
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1 // Уменьшаем высоту на 1 для меню
            };
            
            CreateMenuBar();

            for (int i = 0; i < ItemsNames.Length; i++)
            {
                Button button = new Button(ItemsNames[i])
                {
                    X = Pos.Center(),
                    Y = 2 + (2 * i),
                    Width = 20
                };
                int i1 = i;
                button.Clicked += () =>
                {
                    OnButtonClicked(ItemsNames[i1]);
                };
                CurrentWindow.Add(button);
            }
            
            CurrentToplevel.Add(CurrentWindow);
        }

        /// <summary>
        /// Creates a menu bar.
        /// </summary>
        private void CreateMenuBar()
        {
            MenuItem[] aboutMenuItems = new MenuItem[] {
                new MenuItem ("_Создатель", "", () =>  MessageBox.Query("Создатель", "Павлычев Семён Михайлович", "ОК")),
                new MenuItem ("_Вариант", "", () => MessageBox.Query("Вариант", "Вариант: 3", "ОК"))
            };
            
            MenuItem[] aboutSortItems = new MenuItem[] {
                new MenuItem ("_Статус Сортировки", "", () =>  MessageBox.Query("Статус Сортировки", SortStatus, "ОК")),
            };
            
            MenuItem[] aboutFilterItems = new MenuItem[] {
                new MenuItem ("_Посмотреть Фильтры", "", () =>  MessageBox.Query("Посмотреть Фильтры", FilterStatus, "ОК")),
            };

            MenuBar menuBar = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_О программе ?", aboutMenuItems),
                new MenuBarItem("_Сортировка \u21C5", aboutSortItems),
                new MenuBarItem("_Фильтры \u2699", aboutFilterItems),
            });
            
            CurrentToplevel.Add(menuBar);
        }
        
        /// <summary>
        /// A reaction on an event.
        /// </summary>
        /// <param name="buttonLabel">Button label.</param>
        private void OnButtonClicked(string? buttonLabel)
        {
            int index = Array.IndexOf(ItemsNames, buttonLabel);
            if (index >= 0 && index < Actions.Length)
            {
                
                if (MenuName == "Главное меню")
                {
                    Application.Shutdown();
                }
                Actions[index].Invoke(); // Вызываем действие
            }
            else
            {
                MessageBox.Query("Ошибка", "Действие кнопки не найдено", "ОК");
            }
        }
        
        /// <summary>
        /// A method that gets a dialog input.
        /// </summary>
        /// <param name="text">Text that has to be in this window.</param>
        /// <param name="width">Window width.</param>
        /// <param name="height">Window height.</param>
        /// <returns></returns>
        protected static string GetDialogInput(string text, int width = 80, int height = 35)
        {
            Dialog dialog = new Dialog("Ввод данных", width, height);
            Label label = new(1, 1, text);
            
            int y = text.Split("\n").Length;
            
            TextView textField = new TextView(new Rect(1, y+3, width - 10, 5))
            {
                CanFocus = true, 
                WordWrap = false 
            };
            
            textField.SetFocus();
            
            textField.TextChanged += () =>
            {
                int textLength = textField.Text.Length;
                if (textLength > textField.Bounds.Width)
                {
                    textField.Width = textLength + 1;
                }
            };
            
            Button okButton = new Button("ОК");

            string? input = string.Empty;
            
            okButton.Clicked += () =>
            {
                input = textField.Text.ToString();
                Application.RequestStop();
            };
            
            
            dialog.Add(label);
            dialog.Add(textField);
            dialog.AddButton(okButton);
            Application.Run(dialog);
            
            return input;
        }
        
        /// <summary>
        /// A method that outputs some data.
        /// </summary>
        /// <param name="title">Output title.</param>
        /// <param name="message">Output message.</param>
        protected static void CustomOutput(string title, string message)
        {
            int maxLineLength = message.Split('\n').Select(x => x.Length).Max(); 
            int lineCount = message.Split('\n').Length; 
            
            int minWidth = 30;
            int minHeight = 10;

         
            int width = Math.Max(minWidth, maxLineLength + 4); 
            int height = Math.Max(minHeight, lineCount + 4);  
            
            Dialog dialog = new Dialog(title, 120, 60);

            ScrollView scrollView = new ScrollView(new Rect(1, 1, 100, 50))
            {
                ContentSize = new Size(width,height) // Размер содержимого
            };
            
            Label label = new Label(1, 1, message)
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                TextAlignment = TextAlignment.Left 
            };
            scrollView.Add(label);
            
            Button okButton = new Button("ОК")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(scrollView) + 1
            };
            okButton.Clicked += () => Application.RequestStop();
            
            dialog.Add(scrollView);
            dialog.Add(okButton);

            Application.Run(dialog);
        }

        /// <summary>
        /// A method that creates a progress bar.
        /// </summary>
        protected void CreateProgress()
        {
            CurrentProgressBar = new ProgressBar(new Rect(1,1, 45, 3))
            {
                ColorScheme = new ColorScheme
                {
                    Normal = Terminal.Gui.Attribute.Make(Color.BrightBlue, Color.Cyan), // Темно-синий текст на голубом фоне
                    HotNormal = Terminal.Gui.Attribute.Make(Color.BrightBlue, Color.Cyan) // Цвет для "активного" состояния
                },
                X = Pos.Center(),
                Y = 1
            };
            CurrentProgressBar.Fraction = 0.0f;
            CurrentDialog = new Dialog("Прогресс", 50,8);
            Button okButton = new Button()
            {
                Text = "OK",
                X = Pos.Center(),
                Y = Pos.Bottom(CurrentProgressBar) + 1
            };
            
            okButton.Clicked += () => Application.RequestStop(); // Закрыть окно при нажатии
            
            CurrentDialog.Add(okButton);
            CurrentDialog.Add(CurrentProgressBar);
            
        }
        
        /// <summary>
        /// The main method of the class that starts the menu's operation.
        /// </summary>
        public virtual void Run()
        {
            Application.Init();
            DisplayMenu();
            Application.Run(CurrentToplevel);
        }
    }
}