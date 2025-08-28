using System.Text;
using StateMachine;
using JsonObject;
using Terminal.Gui;
namespace Parser
{
    /// <summary>
    /// Static JsonParser class.
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// A method that turns a text into a queue of chars.
        /// </summary>
        /// <param name="total">Chars in file.</param>
        /// <param name="progressBar">Progress bar.</param>
        /// <param name="json">Queue.</param>
        private static void GetJson(long total, ProgressBar progressBar, out Queue<char> json)
        {
            json = new Queue<char>();
            string? line = Console.ReadLine();
            int i = 0;
            while (line != null)
            {
                foreach(char symbol in line)
                {
                    json.Enqueue(symbol);
                    float progress = (float)(i++ + 1) / total;

                    Application.MainLoop.Invoke(() =>
                    {
                        progressBar.Fraction = progress;
                    });
                }
                Thread.Sleep(5);
                
                line = Console.ReadLine();  
            }
        }
        
        /// <summary>
        /// A method that turns a text into a queue of chars.
        /// </summary>
        /// <param name="json">Queue.</param>
        
        private static void GetJson(out Queue<char> json)
        {
            json = new Queue<char>();
            string? line = Console.ReadLine();
            while (line != null)
            {
                foreach(char symbol in line)
                {
                    json.Enqueue(symbol);
                }
                line = Console.ReadLine();  
            }
        }
        
        /// <summary>
        /// A method that turns a text into a queue of chars.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="json">Queue.</param>
        private static void GetJson(string? text, out Queue<char> json)
        {
            json = new Queue<char>();
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char symbol in text)
                {
                    json.Enqueue(symbol);
                }
            }
        }

        /// <summary>
        /// A method that converts a collection of values.
        /// </summary>
        /// <param name="objects">List of objects.</param>
        /// <param name="cults">List of cults.</param>
        private static void ValueConvertor(List<object> objects, out List<Cult> cults)
        {
            cults = new List<Cult>();
            foreach (object obj in objects)
            {
                Dictionary<string, object> dict  = (Dictionary<string, object>)obj;
                Cult cult = ConvertDictToCult(dict);
                cults.Add(cult);
            }
        }

        /// <summary>
        /// A method that converts dictionary to cult.
        /// </summary>
        /// <param name="dict">Dictionary.</param>
        /// <returns>Cult.</returns>
        private static Cult ConvertDictToCult(Dictionary<string, object> dict)
        {
            Cult cult = new Cult();
            foreach (KeyValuePair<string, object> kvp in dict)
            {
                switch (kvp.Key)
                {
                    case "id":
                    {
                        cult.SetField("Id", kvp.Value.ToString() ?? "none");
                    }
                        break;
                    case "label":
                    {
                        cult.SetField("Label", kvp.Value.ToString() ?? "none");
                    }
                        break;
                    case "description":
                    {
                        cult.SetField("Description", kvp.Value.ToString() ?? "none");
                    }
                        break;
                    case "unique":
                    {
                        cult.SetField("Unique", (bool)kvp.Value);
                    }
                        break;
                    case "aspects":
                    {
                      
                        cult.SetField("Aspects",
                            ((Dictionary<string, object>)kvp.Value).ToDictionary(x => x.Key, x => (int)x.Value));
                    }
                        break;
                    case "slots":
                    {
                        if (((List<object>)kvp.Value).Count ==  0)
                        {
                            List<object> slots =
                            [
                                new Slot()
                            ];
                            cult.SetField("Slots", slots.ToArray());
                        }
                        else
                        {
                            cult.SetField("Slots", ((List<object>)kvp.Value).ToArray());
                        }
                    }
                        break;
                }
            }

            return cult;
        }

        /// <summary>
        /// Get single cult from json object.
        /// </summary>
        /// <param name="cult">Cult.</param>
        public static void GetSingleJsonObject(out Cult cult)
        {
            GetJson(out Queue<char> jsonObject);
            DictionaryStateMaсhine machine = new(DictionaryStates.ObjectStart, jsonObject);
            machine.Run();
            cult =  ConvertDictToCult(machine.Dictionary);
        }
        
        /// <summary>
        /// Get single cult from json object.
        /// </summary>
        /// <param name="cult">Cult.</param>
        /// <param name="text">Cult.</param>
        public static void GetSingleJsonObject(string text, out Cult cult)
        {
            GetJson(text, out Queue<char> jsonObject);
            DictionaryStateMaсhine machine = new(DictionaryStates.ObjectStart, jsonObject);
            machine.Run();
            cult =  ConvertDictToCult(machine.Dictionary);
        }
        /// <summary>
        /// A method that reads json.
        /// </summary>
        /// <returns></returns>
        public static List<Cult> ReadJson(long total, ProgressBar progressBar)
        {
            GetJson(total, progressBar, out Queue<char> json);
            
            DictionaryStateMaсhine machine = new(DictionaryStates.ObjectStart, json);
            machine.Run();
            
            List<object> array = (List<object>)machine.Dictionary.Values.First();
            
            ValueConvertor(array, out List<Cult> cults);

            return  cults;
        }
        
        /// <summary>
        /// A method that reads json.
        /// </summary>
        /// <returns></returns>
        public static List<Cult> ReadJson(string? text)
        {
            GetJson(text, out Queue<char> json);
            
            DictionaryStateMaсhine machine = new(DictionaryStates.ObjectStart, json);
            machine.Run();
            
            List<object> array = (List<object>)machine.Dictionary.Values.First();
            
            ValueConvertor(array, out List<Cult> cults);

            return  cults;
        }

        /// <summary>
        /// A method, that writes all information about current cult collection.
        /// </summary>
        /// <param name="cults">Cult's collection.</param>
        /// <param name="total">Total amount.</param>
        /// <param name="progressBar">Progress Bar.</param>
        public static void WriteJson(CultCollection? cults, long total, ProgressBar progressBar)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("{");
            Console.WriteLine($"\t{'"'}elements{'"'}: [");
            for (int i = 0; i < cults?.Length; i++)
            {
                string output = cults[i].ToJson() + (i == cults.Length - 1 ? string.Empty : ",");
                Console.WriteLine(output);
        
                float progress = (float)(i + 1) / total;

                Application.MainLoop.Invoke(() =>
                {
                    progressBar.Fraction = progress;
                });
                Thread.Sleep(100);
            }
            Console.WriteLine($"\t]");
            Console.WriteLine("}");
        }
        
        /// <summary>
        /// A method, that writes all information about current cult collection.
        /// </summary>
        /// <param name="cults">Cult's collection.</param>
        public static void WriteJsonPretty(CultCollection? cults)   
        {
            for (int i = 0; i < cults?.Length; i++)
            {
                string output = cults[i].ToPrettyString();
                output += Environment.NewLine;
                
                Console.WriteLine(output);
            }
            
        }
    }
}