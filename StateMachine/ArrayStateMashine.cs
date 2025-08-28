using System.Text;

namespace StateMachine
{
    /// <summary>
    /// Represents a state machine for parsing arrays.
    /// </summary>
    public class ArrayStateMashine : StateMachine
    {
        /// <summary>
        /// The list that stores the parsed elements of the array.
        /// </summary>
        public List<object> Array;

        /// <summary>
        /// The current state of the array parsing process.
        /// </summary>
        public ArrayStates State;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayStateMashine"/> class with default values.
        /// </summary>
        public ArrayStateMashine()
        {
            Array = new();
            State = ArrayStates.ArrayStart;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayStateMashine"/> class with a specified state and character queue.
        /// </summary>
        /// <param name="state">The initial state of the array parsing process.</param>
        /// <param name="queue">The queue of characters to be parsed.</param>
        public ArrayStateMashine(ArrayStates state, Queue<char> queue) : base(queue)
        {
            Array = new();
            State = state;
        }

        /// <summary>
        /// Executes the array parsing process based on the current state and the characters in the queue.
        /// </summary>
        /// <exception cref="Exception">Thrown when the array structure is invalid or when unexpected characters are encountered.</exception>
        public override void Run()
        {
            Predicate<char> check = x => char.IsWhiteSpace(x) || x == ' ';
            Predicate<char> forbidden = x => "{}[],.;".Contains(x);

            while (CharQueue.Count != 0 && Stopword != "bebra")
            {
                char symbol = CharQueue.Dequeue();
                switch (State)
                {
                    case ArrayStates.ArrayStart:
                        {
                            if (symbol == '[')
                            {
                                State = ArrayStates.Value;
                            }
                            else if (check(symbol) && !forbidden(symbol))
                            {
                                // Skip whitespace characters.
                            }
                            else
                            {
                                throw new Exception("Массив данных имеет некорректную структуру...");
                            }
                        }
                        break;
                    case ArrayStates.Value:
                        {
                            object value;
                            if (check(symbol))
                            {
                                // Skip whitespace characters.
                                break;
                            }
                            else if (symbol == '[')
                            {
                                // Recursively parse a nested array.
                                ArrayStateMashine machine = new();
                                machine.Run();
                                value = machine.Array;
                            }
                            else if (symbol == '{')
                            {
                                // Recursively parse a nested dictionary.
                                DictionaryStateMaсhine nestedOne = new(DictionaryStates.KeyStart, CharQueue);
                                nestedOne.Run();
                                value = nestedOne.Dictionary;
                            }
                            else if (symbol == '"')
                            {
                                // Parse a string value enclosed in double quotes.
                                StringBuilder valueContent = new StringBuilder();
                                while (CharQueue.Peek() != '"' && !forbidden(CharQueue.Peek()))
                                {
                                    valueContent.Append(CharQueue.Dequeue());
                                }

                                if (forbidden(CharQueue.Peek()))
                                {
                                    throw new Exception("Какая-то из кавычек не была корректно закрыта...");
                                }

                                value = valueContent.ToString();
                            }
                            else if ((symbol >= '0' && symbol <= '9') || symbol == '.' || symbol == ',')
                            {
                                // Parse a numeric value.
                                StringBuilder valueContent = new StringBuilder();
                                while (CharQueue.Peek() >= '0' && CharQueue.Peek() <= '9')
                                {
                                    valueContent.Append(CharQueue.Dequeue());
                                }

                                if (!check(CharQueue.Peek()) && !",}".Contains(CharQueue.Peek()))
                                {
                                    throw new Exception("Некорректный формат числа...");
                                }

                                value = int.Parse(valueContent.ToString());
                            }
                            else
                            {
                                throw new Exception("Значение поля словаря не является корректным...");
                            }
                            Array.Add(value);
                            State = ArrayStates.Comma;
                            break;
                        }
                    case ArrayStates.Comma:
                        {
                            if (check(symbol))
                            {
                            }
                            else if (symbol == ',')
                            {
                                State = ArrayStates.Value;
                            }
                            else if (symbol == ']')
                            {
                                State = ArrayStates.ArrayEnd;
                                Stopword = "bebra";
                            }
                            else
                            {
                                throw new Exception("Массив имеет некорректную структуру...");
                            }
                        }
                        break;
                }
            }
        }
    }
}