namespace StateMachine
{
    /// <summary>
    /// Represents a key-value pair used in dictionary parsing.
    /// </summary>
    public struct KeyValPair
    {
        /// <summary>
        /// The key of the key-value pair.
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// The value of the key-value pair.
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Initializes struct with default values.
        /// </summary>
        public KeyValPair()
        {
            Key = null;
            Value = null;
        }

        /// <summary>
        /// Initializes struct with specified key and value.
        /// </summary>
        /// <param name="key">The key of the key-value pair.</param>
        /// <param name="value">The value of the key-value pair.</param>
        public KeyValPair(string key, object value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Clears the key and value of the key-value pair.
        /// </summary>
        public void Clear()
        {
            Key = null;
            Value = null;
        }
    }
}