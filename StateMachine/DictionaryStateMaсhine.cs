using System.Text;

namespace StateMachine
{
    /// <summary>
    /// Represents a state machine for parsing dictionaries.
    /// </summary>
    public class DictionaryStateMaсhine : StateMachine
    {
        /// <summary>
        /// The dictionary that stores the parsed key-value pairs.
        /// </summary>
        public Dictionary<string, object> Dictionary { get; private set; }

        /// <summary>
        /// The current state of the dictionary parsing process.
        /// </summary>
        public DictionaryStates State;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryStateMaсhine"/> class with default values.
        /// </summary>
        public DictionaryStateMaсhine()
        {
            Dictionary = new Dictionary<string, object>();
            State = new DictionaryStates();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryStateMaсhine"/> class with a specified state and character queue.
        /// </summary>
        /// <param name="state">The initial state of the dictionary parsing process.</param>
        /// <param name="charQueue">The queue of characters to be parsed.</param>
        public DictionaryStateMaсhine(DictionaryStates state, Queue<char> charQueue) : base(charQueue)
        {
            State = state;
            Dictionary = new Dictionary<string, object>();
        }

        /// <summary>
        /// Executes the dictionary parsing process based on the current state and the characters in the queue.
        /// </summary>
        /// <exception cref="Exception">Thrown when the dictionary structure is invalid or when unexpected characters are encountered.</exception>
        public override void Run()
        {
            Predicate<char> check = x => char.IsWhiteSpace(x) || x == ' ' || x == '\n';
            Predicate<char> forbidden = x => "{}[]".Contains(x);
            Predicate<char> boolean = x => "true".Contains(x) || "false".Contains(x);

            KeyValPair pair = new KeyValPair();

            while (CharQueue.Count != 0 && Stopword != "bebra")
            {
                char symbol = CharQueue.Dequeue();
                switch (State)
                {
                    case DictionaryStates.ObjectStart:
                        {
                            if (symbol == '{')
                            {
                                State = DictionaryStates.KeyStart;
                            }
                            else if (check(symbol))
                            {
                                // Skip whitespace characters.
                            }
                            else
                            {
                                throw new Exception("Какая то из скобочек не была корректно открыта...");
                            }
                            break;
                        }
                    case DictionaryStates.KeyStart:
                        {
                            if (symbol == '"')
                            {
                                State = DictionaryStates.KeyContent;
                            }
                            else if (check(symbol))
                            {
                                // Skip whitespace characters.
                            }
                            else
                            {
                                throw new Exception("Какая-то из кавычек не была корректно открыта...");
                            }
                            break;
                        }
                    case DictionaryStates.KeyContent:
                        {
                            // Parse the key content enclosed in double quotes.
                            StringBuilder keyContent = new StringBuilder(symbol.ToString());
                            do
                            {
                                keyContent.Append(CharQueue.Dequeue());
                            } while (CharQueue.Peek() != '"' && !forbidden(CharQueue.Peek()));

                            if (forbidden(CharQueue.Peek()))
                            {
                                throw new Exception("Какая-то из кавычек не была корректно закрыта...");
                            }

                            pair.Key = keyContent.ToString();
                            State = DictionaryStates.KeyEnd;
                        }
                        break;
                    case DictionaryStates.KeyEnd:
                        {
                            if (symbol == '"')
                            {
                                State = DictionaryStates.Colon;
                            }
                            break;
                        }
                    case DictionaryStates.Colon:
                        {
                            if (symbol == ':')
                            {
                                State = DictionaryStates.Value;
                            }
                            else if (!check(symbol))
                            {
                                throw new Exception("Какая-то key-value пара была некорретно написана...");
                            }
                            break;
                        }
                    case DictionaryStates.Value:
                        {
                            object value;
                            if (check(symbol))
                            {
                                // Skip whitespace characters.
                                break;
                            }
                            else if (symbol == ',' || symbol == '}')
                            {
                                throw new Exception("Остутствует значения поля.");
                            }
                            else if (symbol == '[')
                            {
                                // Recursively parse a nested array.
                                ArrayStateMashine machine = new(ArrayStates.Value, CharQueue);
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
                                bool isopen = false;
                                StringBuilder valueContent = new StringBuilder();
                                while (CharQueue.Peek() != '"' || (symbol == '\\' && CharQueue.Peek() == '"'))
                                {
                                    if (symbol == '\\' && CharQueue.Peek() == '"')
                                    {
                                        isopen = !isopen;
                                    }

                                    symbol = CharQueue.Dequeue();
                                    valueContent.Append(symbol);
                                }

                                if (isopen)
                                {
                                    throw new Exception("Цитата написана не корректно.");
                                }

                                CharQueue.Dequeue();

                                value = valueContent.ToString();
                            }
                            else if ((symbol >= '0' && symbol <= '9') || symbol == '.' || symbol == ',')
                            {
                                // Parse a numeric value.
                                StringBuilder valueContent = new StringBuilder();
                                valueContent.Append(symbol);
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
                            else if (boolean(symbol))
                            {
                                // Parse a boolean value.
                                StringBuilder valueContent = new StringBuilder();
                                valueContent.Append(symbol);
                                while (boolean(CharQueue.Peek()))
                                {
                                    valueContent.Append(CharQueue.Dequeue());
                                }

                                if (!check(CharQueue.Peek()) && !",}".Contains(CharQueue.Peek()))
                                {
                                    throw new Exception("Некорректный формат boolean...");
                                }

                                value = valueContent.ToString() == "true";
                            }
                            else
                            {
                                throw new Exception("Значение поля словаря не является корректным...");
                            }

                            pair.Value = value;
                            Dictionary.Add(pair.Key ?? "none", pair.Value);
                            pair.Clear();

                            State = DictionaryStates.Comma;
                            break;
                        }
                    case DictionaryStates.Comma:
                        {
                            if (symbol == ',')
                            {
                                State = DictionaryStates.KeyStart;
                            }
                            else if (symbol == '}')
                            {
                                State = DictionaryStates.ObjectEnd;
                                Stopword = "bebra";
                            }
                            else if (!check(symbol))
                            {
                                throw new Exception("Словарь имет некорректную структуру...");
                            }
                            break;
                        }
                }
            }
        }
    }
}