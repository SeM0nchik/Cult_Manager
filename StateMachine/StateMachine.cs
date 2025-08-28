namespace StateMachine
{
    /// <summary>
    /// Represents an abstract base class for state machines.
    /// </summary>
    public abstract class StateMachine
    {
        /// <summary>
        /// A stop word used to control the execution of the state machine.
        /// </summary>
        protected string Stopword = string.Empty;

        /// <summary>
        /// A queue of characters to be processed by the state machine.
        /// </summary>
        protected readonly Queue<char> CharQueue;

        /// <summary>
        /// Initializes a new instance of the class with an empty character queue.
        /// </summary>
        protected StateMachine()
        {
            CharQueue = new Queue<char>();
        }

        /// <summary>
        /// Initializes a new instance of the class with a specified character queue.
        /// </summary>
        /// <param name="charQueue">The queue of characters to be processed by the state machine.</param>
        protected StateMachine(Queue<char> charQueue)
        {
            CharQueue = charQueue;
        }

        /// <summary>
        /// Executes the state machine's logic. This method must be implemented by derived classes.
        /// </summary>
        public abstract void Run();
    }
}