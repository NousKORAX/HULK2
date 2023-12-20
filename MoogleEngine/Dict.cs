using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine;

public class Dict
{
    public static Dictionary<int, double> SortDictionary(Dictionary<int, double> dictionary)
    {
        List<KeyValuePair<int, double>> list = dictionary.ToList();
        list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        Dictionary<int, double> sortedDictionary = new Dictionary<int, double>();
        foreach (KeyValuePair<int, double> pair in list)
        {
            sortedDictionary.Add(pair.Key, pair.Value);
        }
        return sortedDictionary;
    }
 
}

