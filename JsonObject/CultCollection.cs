using System.Collections;
namespace JsonObject
{
    /// <summary>
    /// Encapsulates the functionality for managing a collection of cults.
    /// </summary>
    public class CultCollection : IEnumerable<Cult>
    {
        /// <summary>
        /// A list containing all cults in the collection.
        /// </summary>
        private readonly List<Cult> _cults;

        /// <summary>
        /// Gets the total number of cults in the collection.
        /// </summary>
        public int Length => _cults.Count;

        /// <summary>
        /// Gets a list of all unique aspects from all cults in the collection.
        /// </summary>
        public List<string?> AllAspects
        {
            get
            {
                List<string?> allAspects = new List<string?>();
                foreach (Cult cult in _cults)
                {
                    foreach (string? aspect in cult.Aspects.Keys)
                    {
                        if (!allAspects.Contains(aspect))
                        {
                            allAspects.Add(aspect);
                        }
                    }
                }
                return allAspects;
            }
        }

        /// <summary>
        /// Gets a list of all unique IDs from the cults in the collection.
        /// </summary>
        public List<string> AllIds => _cults.Select(c => c.GetField("Id") ?? "none").ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="CultCollection"/> class.
        /// </summary>
        public CultCollection() {_cults =  new List<Cult>();}

        /// <summary>
        /// Initializes a new instance of the <see cref="CultCollection"/> class with a predefined list of cults.
        /// </summary>
        /// <param name="cults">The list of cults to initialize the collection with.</param>
        public CultCollection(List<Cult> cults)
        {
            _cults = cults;
        }

        /// <summary>
        /// Merges two cult collections into a single collection based on user-defined choices.
        /// </summary>
        /// <param name="x">The first cult collection to merge.</param>
        /// <param name="y">The second cult collection to merge.</param>
        /// <param name="choice">A function that defines the user's choice for conflicting cults.</param>
        /// <returns>A new <see cref="CultCollection"/> containing the merged cults.</returns>
        public static CultCollection Merge(CultCollection x, CultCollection y, Func<string, int> choice)
        {
            CultCollection result = new();
            List<string> common = x.AllIds.Intersect(y.AllIds).ToList();
            foreach (string id in common)
            {
                int what = choice.Invoke(id);

                Cult? cult1 = x.FindId(id);
                Cult? cult2 = y.FindId(id);

                Cult? finalChoice = what == 1 ? cult1 : cult2;

                result.AddCult(finalChoice);
            }

            List<string> unique = x.AllIds.Concat(y.AllIds).Except(common).ToList();

            foreach (string id in unique)
            {
                if (x.AllIds.Contains(id))
                {
                    result.AddCult(x.FindId(id));
                }
                else
                {
                    result.AddCult(y.FindId(id));
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a cult to the collection. If a cult with the same ID already exists, it will be updated.
        /// </summary>
        /// <param name="cult">The cult to add or update.</param>
        public void AddCult(Cult? cult)
        {
            if (FindId(cult?.GetField("Id")) != null)
            {
                for (int i = 0; i < _cults.Count; i++)
                {
                    if (_cults[i].GetField("Id") == cult?.GetField("Id"))
                    {
                        if (cult != null)
                        {
                            _cults[i] = (Cult)cult;
                        }
                    }
                }
            }
            else
            {
                if (cult != null)
                {
                    _cults.Add((Cult)cult);
                }
            }
        }
        /// <summary>
        /// Adds a cult to the collection. If a cult with the same ID already exists, it will be updated.
        /// </summary>
        /// <param name="id">The id of the cult that we want to change.</param>
        /// <param name="cult">The cult to add or update.</param>
        public void ChangeCult(string? id, Cult cult)
        {
            for (int i = 0; i < _cults.Count; i++)
            {
                if (_cults[i].GetField("Id") == id)
                {
                    _cults[i] = cult;
                }
            }
        }
        
        /// <summary>
        /// Adds a list of cults to the collection.
        /// </summary>
        /// <param name="cults">The list of cults to add.</param>
        public void AddCultList(List<Cult>? cults)
        {
            if (cults != null)
            {
                foreach (Cult cult in cults)
                {
                    AddCult(cult);
                }
                
            }
        }

        /// <summary>
        /// Gets the cult at the specified index in the collection.
        /// </summary>
        /// <param name="index">The index of the cult to retrieve.</param>
        /// <returns>The cult at the specified index.</returns>
        public Cult this[int index] => _cults[index];

        /// <summary>
        /// Prints the value of a specified field for all cults in the collection.
        /// </summary>
        /// <param name="fieldName">The name of the field to print.</param>
        /// <param name="printer">An optional action to customize the printing behavior.</param>
        /// <param name="printed">All field values.</param>
        public void PrintField(string fieldName, Func<string, string> printer, out string printed)
        {
            List<string?> values = new();
            printed = printer(fieldName) + Environment.NewLine;
            foreach (Cult cult in _cults)
            {
                if (!values.Contains(cult.GetField(fieldName)))
                {
                    string value = cult.GetField(fieldName) ?? "";
                    printed += value + Environment.NewLine;
                    values.Add(cult.GetField(fieldName));
                }
            }
        }

        /// <summary>
        /// Sorts the cults in the collection based on the specified field and order.
        /// </summary>
        /// <param name="field">The field to sort by.</param>
        /// <param name="reverse">Whether to sort in reverse order.</param>
        /// <param name="key">An optional key to use for sorting.</param>
        public void Sort(string field, bool reverse, string? key)
        {
            switch (field)
            {
                case "Aspects":
                    _cults.Sort(new CultComparator(field, reverse, key ?? " "));
                    break;
                default:
                    _cults.Sort(new CultComparator(field, reverse));
                    break;
            }
        }

        /// <summary>
        /// Filters the cults in the collection based on the specified field and list of allowed values.
        /// </summary>
        /// <param name="field">The field to filter by.</param>
        /// <param name="correct">The list of allowed values for the field.</param>
        /// <returns>A new <see cref="CultCollection"/> containing the filtered cults.</returns>
        public CultCollection Filter(string field, List<string?> correct)
        {
            CultCollection filtered = new CultCollection();

            foreach (Cult cult in _cults)
            {
                if (correct.Contains(cult.GetField(field)))
                {
                    filtered.AddCult(cult);
                }
            }
            return filtered;
        }

        /// <summary>
        /// Finds a cult in the collection by its ID.
        /// </summary>
        /// <param name="id">The ID of the cult to find.</param>
        /// <returns>The cult with the specified ID, or null if not found.</returns>
        public Cult? FindId(string? id)
        {
            foreach (Cult cult in _cults)
            {
                if (cult.GetField("Id") == id)
                {
                    return cult;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Removes a cult from the collection by its ID.
        /// </summary>
        /// <param name="id">The ID of the cult to remove.</param>
        public void DeleteId(string? id)
        {
            for (int i = 0; i < _cults.Count; i++)
            {
                if (_cults[i].GetField("Id") == id)
                {
                    _cults.RemoveAt(i);
                }
            }
        }

        public CultCollection Copy()
        {
            return new CultCollection(_cults);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection of cults.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<Cult> GetEnumerator()
        {
            return _cults.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of cults.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}