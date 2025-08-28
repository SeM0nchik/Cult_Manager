using System.Text;
namespace JsonObject
{
    public struct Slot : IJsonObject
    {
        
        private string _id;
        private string _label;
        private string _actionid;
        private Dictionary<string, int> _required;
        private Dictionary<string, int> _forbidden;
        private string _description;

        /// <summary>
        /// Slot's ID.
        /// </summary>
        public string Id
        {
            get => _id;
            private set => _id = value;
        }

        /// <summary>
        /// Slot's Label (Name).
        /// </summary>
        public string Label
        {
            get => _label;
            private set => _label = value;
        }

        /// <summary>
        /// Slot's ActionId
        /// </summary>
        public string ActionId
        {
            get => _actionid;
            private set => _actionid = value;
        }

        /// <summary>
        /// Slot's Required.
        /// </summary>
        public Dictionary<string, int> Required
        {
            get => _required;
            private set => _required = value;
        }

        /// <summary>
        /// Slot's Forbidden.
        /// </summary>
        public Dictionary<string, int> Forbidden
        {
            get => _forbidden;
            private set => _forbidden = value;
        }

        /// <summary>
        /// Slot's description.
        /// </summary>
        public string Description
        {
            get => _description;
            private set => _description = value;
        }
        /// <summary>
        /// Primary constructor.
        /// </summary>
        public Slot()
        {
            Id = "none";
            Label = "none";
            ActionId = "none";
            Description = "none";
            Required = new Dictionary<string, int>() { { "none", 0 } };
            Forbidden = new Dictionary<string, int>() { {"none", 0} };
        }
        /// <summary>
        /// Method that returns all fields of the class/struct.
        /// </summary>
        /// <returns>An array of strings representing the fields of the class/object.</returns>
        public IEnumerable<string> GetAllFields()
        {
            return GetType().GetProperties().Select(x => x.Name);
        }
        
        /// <summary>
        /// Method that returns the value of the specified property.
        /// </summary>
        /// <param name="fieldName">The name of the property.</param>
        /// <returns>The value of the property.</returns>
        public string? GetField(string fieldName)
        {
            return GetType().GetProperty(fieldName)?.GetValue(this)?.ToString();
        }

        /// <summary>
        /// Method that sets the corresponding property to the specified value.
        /// </summary>
        /// <param name="fieldName">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        public void SetField(string fieldName, string value)
        {
            if (GetAllFields().Contains(fieldName))
            {
                switch (fieldName)
                {
                    case "Id":
                    {
                        if (value.Length > 15)
                        {
                            Id = value[..12] + "...";
                        }
                        else
                        {
                            Id = value;
                        }
                    }
                        break;
                    case "Label":
                    {
                        if (value.Length > 15)
                        {
                            Label = value[12..] + "...";
                        }
                        else
                        {
                            Label = value;
                        }
                    }
                        break;
                    case "ActionId":
                    {
                        if (ActionId.Length > 15)
                        {
                            ActionId = value[..12] + "...";
                        }
                        else
                        {
                            ActionId = value;
                        }

                        break;
                    }
                    case "Description":
                    {
                        Description = value;
                    }
                        break;
                }
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Method that sets the corresponding property to the specified value.
        /// </summary>
        /// <param name="fieldName">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        public void SetField(string fieldName, Dictionary<string, object?> value)
        {
            if (fieldName == "Required")
            {
                Required = value.ToDictionary(x => x.Key.ToString(), x => (int)(x.Value ?? "0"));
            }
            else if (fieldName == "Forbidden")
            {
                Forbidden = value.ToDictionary(x => x.Key.ToString(), x => (int)(x.Value ?? "0"));
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        
        /// <summary>
        /// Converts a string type field to JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the string type property.</returns>
        private string StringFieldToJson(string field) { return $"\t\t\t\t\t{'"'}{field.ToLower()}{'"'} :{'"'}{GetField(field)}{'"'},"; }
      
        /// <summary>
        /// Converts a string-object Dictionary property to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the string-object Dictionary property.</returns>
        private string DictStringToJson(string field)
        {
            Dictionary<string, int> dict;
            
            if (field == "Required")
            {
                dict = Required;
            }
            else 
            {
                dict = Forbidden;
            }

            string result = $"\t\t\t\t\t{'"'}{field.ToLower()}{'"'}: " +'{' + Environment.NewLine;
            for (int i = 0; i < dict.Count; i++)
            {
                string key = dict.Keys.ToList()[i];
                int val = dict.Values.ToList()[i];
            
                //Last iteration means we don't need a comma.
                if (i != dict.Count - 1)
                {
                    result += $"\t\t\t\t\t\t{'"'}{key}{'"'}: {val},"  + Environment.NewLine;
                }
                else
                {
                    result +=  $"\t\t\t\t\t\t{'"'}{key}{'"'}: {val}" + Environment.NewLine;
                }
            }
            result += "\t\t\t\t\t},";
            
            return result;
        }
        /// <summary>
        /// Method that returns json-string representation of a Slot.
        /// </summary>
        /// <returns>Json-string representation of a Slot.</returns>
        public string ToJson()
        {
            string result = $"\t\t\t\t" + '{' + Environment.NewLine;
            foreach (string field in GetAllFields())
            {
                if (field == "Required" || field == "Forbidden")
                {
                    result += DictStringToJson(field);
                }
                else if (field == "Description")
                {
                    result += StringFieldToJson(field)[..^1];
                }
                else
                {
                    result += StringFieldToJson(field);
                }
                
                result += Environment.NewLine;
            }
            result += $"\t\t\t\t" + '}';
            return result;
        }
        
        /// <summary>
        /// A method that formats slot.
        /// </summary>
        /// <param name="slot">The slot to format.</param>
        /// <param name="maxKeyWidth">Max key width.</param>
        /// <param name="maxValueWidth">Max value width.</param>
        /// <returns>A formatted string.</returns>
        public static string FormatSlot(Slot slot, int maxKeyWidth, int maxValueWidth)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("├" + new string('─', maxKeyWidth + maxValueWidth + 3) + "┤");
            sb.Append("│   ").Append("ID:".PadRight(maxKeyWidth - 2)).Append(" ").Append(TruncateOrPad(slot.Id, maxValueWidth)).AppendLine(" │");
            sb.Append("│   ").Append("Label:".PadRight(maxKeyWidth - 2)).Append(" ").Append(TruncateOrPad(slot.Label, maxValueWidth)).AppendLine(" │");
            sb.Append("│   ").Append("Action ID:".PadRight(maxKeyWidth - 2)).Append(" ").Append(TruncateOrPad(slot.ActionId, maxValueWidth)).AppendLine(" │");

            // Required
            sb.Append("│   ").Append("Required:".PadRight(maxKeyWidth - 2)).Append(" ").Append("".PadRight(maxValueWidth)).AppendLine(" │");
            foreach (KeyValuePair<string, int> req in slot.Required)
            {
                sb.Append("│     ").Append((req.Key + ":").PadRight(maxKeyWidth - 4)).Append(" ").Append(TruncateOrPad(req.Value.ToString(), maxValueWidth)).AppendLine(" │");
            }

            // Forbidden
            sb.Append("│   ").Append("Forbidden:".PadRight(maxKeyWidth - 2)).Append(" ").Append("".PadRight(maxValueWidth)).AppendLine(" │");
            foreach (KeyValuePair<string, int> forb in slot.Forbidden)
            {
                sb.Append("│     ").Append((forb.Key + ":").PadRight(maxKeyWidth - 4)).Append(" ").Append(TruncateOrPad(forb.Value.ToString(), maxValueWidth)).AppendLine(" │");
            }

            // Description (с переносом строки, если текст слишком длинный)
            sb.Append("│   ").Append("Description:".PadRight(maxKeyWidth - 2)).Append(" ").Append(TruncateOrPad(slot.Description, maxValueWidth)).AppendLine(" │");

            return sb.ToString();
        }
        
        /// <summary>
        /// A method that shortens or widens text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="maxLength">Max length.</param>
        /// <returns>New text.</returns>
        public static string TruncateOrPad(string text, int maxLength)
        {
            if (text.Length > maxLength)
            {
                return text.Substring(0, maxLength - 3) + "...";
            }
            return text.PadRight(maxLength); 
        }
    }
    
}