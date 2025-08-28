namespace JsonObject
{
    /// <summary>
    /// A class that consists of methods of extension.
    /// </summary>
    public static class EnhanceClass
    {
        /// <summary>
        /// Visual length.
        /// </summary>
        /// <returns>Length.</returns>
        public static int VisualLength(this string text)
        {
            int counter = 0;
            foreach (char symbol in text)
            {
                if ((symbol >= 0x4E00 && symbol <= 0x9FFF) || symbol == '（' ||
                    symbol == '）') //Chinese chars visually are 2 times wider than others.
                {
                    counter++;
                }

                counter++;

            }
            return counter;
        }
    }
}