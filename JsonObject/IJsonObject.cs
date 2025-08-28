namespace JsonObject
{
    /// <summary>
    /// Interface for all objects in a JSON file.
    /// </summary>
    public interface IJsonObject
    {
        /// <summary>
        /// Method that returns all fields of the class/struct.
        /// </summary>
        /// <returns>An array of strings representing the fields of the class/object.</returns>
        IEnumerable<string> GetAllFields();

        /// <summary>
        /// Method that returns the value of the specified field.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The value of the field.</returns>
        string? GetField(string fieldName);

        /// <summary>
        /// Method that sets the value of the specified field.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="value">The value to set.</param>
        void SetField(string fieldName, string value);
    }
}