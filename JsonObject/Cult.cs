using System.Text;

namespace JsonObject
{
    /// <summary>
    /// A structure of Book of Hours' visitors.
    /// </summary>
    public struct Cult : IJsonObject
    {
        private string _id;
        private string _label;
        private Dictionary<string, int> _aspects;
        private Slot[] _slots;
        private bool _unique;
        private string _description;

        /// <summary>
        /// Cult's ID.
        /// </summary>
        public string Id
        {
            get => _id;
            private set => _id = value;
        }

        /// <summary>
        /// Cult's Label (Name).
        /// </summary>
        public string Label
        {
            get => _label;
            private set => _label = value;
        }

        /// <summary>
        /// Cult's Aspects.
        /// </summary>
        public Dictionary<string, int> Aspects
        {
            get => _aspects;
            private set => _aspects = value;
        }

        /// <summary>
        /// Cult's Slots.
        /// </summary>
        public Slot[] Slots
        {
            get => _slots;
            private set => _slots = value;
        }

        /// <summary>
        /// Cult's uniqueness,
        /// </summary>
        public bool Unique
        {
            get => _unique;
            private set => _unique = value;
        }

        /// <summary>
        /// Cult's description.
        /// </summary>
        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        /// <summary>
        /// Primary constructor.
        /// </summary>
        public Cult()
        {
            Id = "none";
            Label = "none";
            Aspects = new Dictionary<string, int>();
            Slots = [];
            Description = "none";
            Unique = false;
        }

        /// <summary>
        /// Method that returns all fields of the class/struct.
        /// </summary>
        /// <returns>An array of strings representing the properties of the class/object.</returns>
        public IEnumerable<string> GetAllFields()
        {
            return GetType().GetProperties().Select(x => x.Name);
        }

        /// <summary>
        /// Method that returns the value of the specified property.
        /// </summary>
        /// <param name="fieldName">The name of a property.</param>
        /// <returns>The value of the field.</returns>
        public string? GetField(string fieldName)
        {
            return GetType().GetProperty(fieldName)?.GetValue(this)?.ToString();
        }

        /// <summary>
        /// Method that sets the corresponding property to the specified value.
        /// </summary>
        /// <param name="fieldName">The name of a property.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="KeyNotFoundException">Property is not found.</exception>
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
                                Id = value[12..] + "...";
                            }
                            else
                            {
                                Id = value;
                            }
                        }
                        break;
                    case "Label":
                        {
                            Label = value;
                        }
                        break;
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
        /// <exception cref="KeyNotFoundException">Property is not found.</exception>
        public void SetField(string fieldName, bool value)
        {
            if (fieldName == "Unique")
            {
                Unique = value;
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
        /// <exception cref="KeyNotFoundException">Property is not found.</exception>
        public void SetField(string fieldName, Dictionary<string, int> value)
        {
            if (fieldName == "Aspects")
            {
                Aspects = value;
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
        /// <exception cref="KeyNotFoundException">Property is not found.</exception>
        public void SetField(string fieldName, object[] value)
        {
            if (fieldName == "Slots")
            {
                List<Slot> arr = new();
                foreach (object obj in value)
                {
                    Slot newSlot = new();
                    Dictionary<string, object> slot = (Dictionary<string, object>)obj;
                    foreach (KeyValuePair<string, object> kvp in slot)
                    {
                        switch (kvp.Key)
                        {
                            case "id":
                                {
                                    newSlot.SetField("Id", kvp.Value.ToString() ?? "none");
                                }
                                break;
                            case "label":
                                {
                                    newSlot.SetField("Label", kvp.Value.ToString() ?? "none");
                                }
                                break;
                            case "description":
                                {
                                    newSlot.SetField("Description", kvp.Value.ToString() ?? "none");
                                }
                                break;
                            case "actionid":
                                {
                                    newSlot.SetField("ActionId", kvp.Value.ToString() ?? "none");
                                }
                                break;
                            case "required":
                                {
                                    newSlot.SetField("Required", (Dictionary<string, object?>)kvp.Value);
                                }
                                break;
                            case "forbidden":
                                {
                                    newSlot.SetField("Forbidden", (Dictionary<string, object?>)kvp.Value);
                                }
                                break;
                        }

                    }

                    arr.Add(newSlot);
                }

                Slots = arr.ToArray();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Overrides the ToString method to provide a custom string representation of the object.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public override string ToString()
        {
            return $"Id: {Id}, Label: {Label}, , Aspects: {Aspects}";
        }

        /// <summary>
        /// Converts a string type field to JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the string type property.</returns>
        private string StringFieldToJson(string field)
        {
            return $"\t\t\t{'"'}{field.ToLower()}{'"'} :{'"'}{GetField(field)}{'"'},";
        }

        /// <summary>
        /// Converts a Boolean type field to JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the Boolean type property.</returns>
        private string BooleanFieldToJson(string field)
        {
            return $"\t\t\t{'"'}{field.ToLower()}{'"'} :{GetField(field)?.ToLower()},";
        }

        /// <summary>
        /// Converts a string-object Dictionary property to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the string-object Dictionary property.</returns>
        public string DictToJson(string field)
        {
            string result = $"\t\t\t{'"'}{field.ToLower()}{'"'}: " + '{' + Environment.NewLine;
            for (int i = 0; i < Aspects.Count; i++)
            {
                string key = Aspects.Keys.ToList()[i];
                int val = Aspects.Values.ToList()[i];

                //Last iteration means we don't need a comma.
                result += $"\t\t\t\t{'"'}{key}{'"'}: {val}";
                if (i != Aspects.Count - 1)
                {
                    result += ',';
                }

                result += Environment.NewLine;
            }

            result += "\t\t\t},";
            return result;
        }
        /// <summary>
        /// Converts an array property to a JSON string.
        /// </summary>
        /// <param name="field">Property name.</param>
        /// <returns>A json representation of a json string.</returns>
        public string ArrayToJson(string field)
        {
            string result = $"\t\t\t{'"'}{field.ToLower()}{'"'}: [" + Environment.NewLine;
            for (int i = 0; i < Slots.Length; i++)
            {
                result += Slots[i].ToJson();
                if (i != Slots.Length - 1)
                {
                    result += ",";
                }

                result += Environment.NewLine;
            }

            result += "\t\t\t],";
            return result;
        }

        /// <summary>
        /// Method that returns json-string representation of a Cult.
        /// </summary>
        /// <returns>Json-string representation of a Cult.</returns>
        public string ToJson()
        {
            string result = "\t\t{" + Environment.NewLine;
            int len = GetAllFields().ToList().Count;
            for (int i = 0; i < len; i++)
            {
                string field = GetAllFields().ToList()[i];
                switch (field)
                {
                    case "Aspects":
                        result += DictToJson("Aspects");
                        break;
                    case "Slots":
                        result += ArrayToJson("Slots");
                        break;
                    case "Unique":
                        result += BooleanFieldToJson("Unique");
                        break;
                    default:
                        result += StringFieldToJson(field);
                        break;
                }

                if (i == len - 1)
                {
                    result = result[..^1];
                }

                result += Environment.NewLine;
            }

            result += "\t\t}";

            return result;
        }

        /// <summary>
        /// Method that converts slot to a pretty string.
        /// </summary>
        /// <returns>A pretty string.</returns>
        public string ToPrettyString()
        {
            StringBuilder sb = new StringBuilder();

            
            int maxKeyWidth = 20; 
            int maxValueWidth = 40; 

           
            maxKeyWidth = Math.Max(maxKeyWidth, Id.VisualLength());
            maxKeyWidth = Math.Max(maxKeyWidth, Label.VisualLength());
            maxKeyWidth = Math.Max(maxKeyWidth, "Aspects:".VisualLength());
            foreach (KeyValuePair<string, int> aspect in Aspects)
            {
                maxKeyWidth = Math.Max(maxKeyWidth, aspect.Key.VisualLength() + 2); // +2 для ": "
            }
            foreach (Slot slot in Slots)
            {
                maxKeyWidth = Math.Max(maxKeyWidth, slot.Id.VisualLength());
                maxKeyWidth = Math.Max(maxKeyWidth, slot.Label.VisualLength());
                maxKeyWidth = Math.Max(maxKeyWidth, slot.ActionId.VisualLength());
                foreach (KeyValuePair<string, int> req in slot.Required)
                {
                    maxKeyWidth = Math.Max(maxKeyWidth, req.Key.VisualLength() + 2);
                }
                foreach (KeyValuePair<string, int> forb in slot.Forbidden)
                {
                    maxKeyWidth = Math.Max(maxKeyWidth, forb.Key.VisualLength() + 2);
                }
                maxKeyWidth = Math.Max(maxKeyWidth, "Description:".VisualLength());
            }
            maxKeyWidth = Math.Max(maxKeyWidth, "Description:".VisualLength());
            maxKeyWidth = Math.Max(maxKeyWidth, "Unique:".VisualLength());

            
            int tableWidth = maxKeyWidth + maxValueWidth + 7; 

            
            string CreateHorizontalLine() => "├" + new string('─', tableWidth - 4) + "┤";

          
            sb.AppendLine("┌" + new string('─', tableWidth - 4) + "┐");

           
            sb.Append("│ ").Append("Cult Information".PadRight(tableWidth - 6)).AppendLine(" │");

            
            sb.AppendLine(CreateHorizontalLine());
            sb.Append("│ ").Append("ID:".PadRight(maxKeyWidth)).Append(" ").Append(Slot.TruncateOrPad(Id, maxValueWidth)).AppendLine(" │");
            sb.Append("│ ").Append("Label:".PadRight(maxKeyWidth)).Append(" ").Append(Slot.TruncateOrPad(Label, maxValueWidth)).AppendLine(" │");

         
            sb.AppendLine(CreateHorizontalLine());
            sb.Append("│ ").Append("Aspects:".PadRight(maxKeyWidth)).Append(" ").Append("".PadRight(maxValueWidth)).AppendLine(" │");
            foreach (KeyValuePair<string, int> aspect in Aspects)
            {
                sb.Append("│   ").Append((aspect.Key + ":").PadRight(maxKeyWidth - 2)).Append(" ").Append(Slot.TruncateOrPad(aspect.Value.ToString(), maxValueWidth)).AppendLine(" │");
            }

           
            sb.AppendLine(CreateHorizontalLine());
            sb.Append("│ ").Append("Slots:".PadRight(maxKeyWidth)).Append(" ").Append("".PadRight(maxValueWidth)).AppendLine(" │");
            foreach (Slot slot in Slots)
            {
                sb.Append(Slot.FormatSlot(slot, maxKeyWidth, maxValueWidth));
            }

            
            sb.AppendLine(CreateHorizontalLine());
            sb.Append("│ ").Append("Description:".PadRight(maxKeyWidth)).Append(" ").Append("".PadRight(maxValueWidth)).AppendLine(" │");
            foreach (string line in SplitText(Description, maxValueWidth))
            {
                sb.Append("│ ").Append("".PadRight(maxKeyWidth)).Append(" ").Append(Slot.TruncateOrPad(line, maxValueWidth)).AppendLine(" │");
            }

         
            sb.AppendLine(CreateHorizontalLine());
            sb.Append("│ ").Append("Unique:".PadRight(maxKeyWidth)).Append(" ").Append(Slot.TruncateOrPad(Unique.ToString(), maxValueWidth)).AppendLine(" │");

           
            sb.AppendLine("└" + new string('─', tableWidth - 4) + "┘");

            return sb.ToString();
        }

        /// <summary>
        /// Splits a text into lines of a fixed maximum length.
        /// </summary>
        /// <param name="text">The input text to be split.</param>
        /// <param name="maxLength">The maximum length of each line.</param>
        /// <returns>An enumerable collection of strings, where each string has a length of at most <paramref name="maxLength"/>.</returns>
        private IEnumerable<string> SplitText(string text, int maxLength)
        {
            for (int i = 0; i < text.Length; i += maxLength)
            {
                yield return text.Substring(i, Math.Min(maxLength, text.Length - i));
            }
        }
    }
}