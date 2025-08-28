namespace StateMachine
{
    /// <summary>
    /// Dictionary states.
    /// </summary>
    public enum DictionaryStates
    {
        ObjectStart,
        KeyStart,
        KeyContent,
        KeyEnd,
        Colon,
        Value, 
        Comma, 
        ObjectEnd,
    }
}