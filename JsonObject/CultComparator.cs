namespace JsonObject
{
    /// <summary>
    /// Defines methods for comparing two cults based on specified fields.
    /// </summary>
    public class CultComparator : IComparer<Cult>
    {
        /// <summary>
        /// The field to compare between cults.
        /// </summary>
        private string Field { get; set; }

        /// <summary>
        /// Indicates whether sorting should be in reverse order.
        /// </summary>
        private bool Reverse { get; set; }

        /// <summary>
        /// The key used for sorting aspects or other specific fields.
        /// </summary>
        private string Key { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultComparator"/> class for standard sorting.
        /// </summary>
        /// <param name="field">The field to compare.</param>
        /// <param name="reverse">Indicates whether sorting should be in reverse order.</param>
        public CultComparator(string field, bool reverse)
        {
            (Field, Reverse) = (field, reverse);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultComparator"/> class for sorting aspects or specific fields.
        /// </summary>
        /// <param name="field">The field to compare.</param>
        /// <param name="reverse">Indicates whether sorting should be in reverse order.</param>
        /// <param name="key">The key used for sorting aspects or specific fields.</param>
        public CultComparator(string field, bool reverse, string key)
        {
            (Field, Key, Reverse) = (field, key, reverse);
        }

        /// <summary>
        /// Compares two cults based on the specified field and sorting rules.
        /// </summary>
        /// <param name="x">The first cult to compare.</param>
        /// <param name="y">The second cult to compare.</param>
        /// <returns>
        /// A value indicating the relative order of the cults:
        /// - Less than 0 if x is less than y.
        /// - 0 if x is equal to y.
        /// - Greater than 0 if x is greater than y.
        /// </returns>
        /// <exception cref="Exception">Thrown when the specified field is not supported for sorting.</exception>
        public int Compare(Cult x, Cult y)
        {
            switch (Field)
            {
                case "Id":
                    return string.Compare(x.GetField("Id"), y.GetField("Id"), StringComparison.Ordinal) * (Reverse ? -1 : 1);
                case "Label":
                    return string.Compare(x.GetField("Label"), y.GetField("Label"), StringComparison.Ordinal) * (Reverse ? -1 : 1);
                case "Description":
                    return string.Compare(x.GetField("Description"), y.GetField("Description"), StringComparison.Ordinal) * (Reverse ? -1 : 1);
                case "Unique":
                    return Convert.ToBoolean(x.GetField("Unique")).CompareTo(Convert.ToBoolean(y.GetField("Unique"))) * (Reverse ? -1 : 1);
                case "Aspects":
                    {
                        // If both cults have the specified aspect, compare their aspect rates.
                        if (x.Aspects.Keys.Contains(Key) && y.Aspects.Keys.Contains(Key))
                        {
                            return x.Aspects[Key].CompareTo(y.Aspects[Key]) * (Reverse ? -1 : 1);
                        }
                        // If only one cult has the aspect, it is considered greater.
                        else if (x.Aspects.Keys.Contains(Key))
                        {
                            return 1 * (Reverse ? -1 : 1);
                        }
                        else if (y.Aspects.Keys.Contains(Key))
                        {
                            return -1 * (Reverse ? -1 : 1);
                        }
                        // If neither cult has the aspect, they are considered equal.
                        else
                        {
                            return 0;
                        }
                    }
                case "Slots":
                    {
                        throw new Exception("Sorting by the 'Slots' field is not supported.");
                    }
                default:
                    throw new Exception("The specified field for sorting does not exist.");
            }
        }
    }
}