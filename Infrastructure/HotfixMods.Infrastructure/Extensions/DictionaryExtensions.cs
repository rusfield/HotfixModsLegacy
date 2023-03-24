using HotfixMods.Infrastructure.Comparers;

namespace HotfixMods.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<T, string> SortByKey<T>(this Dictionary<T, string> dict, bool ascending = true) 
            where T : notnull
        {
            List<KeyValuePair<T, string>> list = new List<KeyValuePair<T, string>>(dict);
            list.Sort((x, y) => Comparer<T>.Default.Compare(x.Key, y.Key));
            if (!ascending)
                list.Reverse();
            Dictionary<T, string> sortedDict = new Dictionary<T, string>();
            foreach (var pair in list)
            {
                sortedDict.Add(pair.Key, pair.Value);
            }

            return sortedDict;
        }

        public static Dictionary<T, string> SortByValue<T>(this Dictionary<T, string> dict, bool ascending = true)
            where T : notnull
        {
            List<KeyValuePair<T, string>> list = new List<KeyValuePair<T, string>>(dict);
            list.Sort((x, y) => new NumericStringComparer().Compare(x.Value, y.Value));
            if (!ascending)
                list.Reverse();
            Dictionary<T, string> sortedDict = new Dictionary<T, string>();
            foreach (var pair in list)
            {
                sortedDict.Add(pair.Key, pair.Value);
            }

            return sortedDict;
        }

        public static void InitializeDefaultValue<T>(this Dictionary<T, string> dict)
            where T : notnull
        {
            dict[default(T)] = "None";
        }

        public static void ShowKeyInValue<T>(this Dictionary<T, string> dict, string delimiter)
        {
            delimiter = delimiter.Trim();
            foreach(var key in dict.Keys)
            {
                if(!key.ToString().Equals(dict[key], StringComparison.InvariantCultureIgnoreCase))
                    dict[key] = $"{key} {delimiter} {dict[key]}";
            }
        }
    }
}
