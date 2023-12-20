using System.Text.RegularExpressions;

namespace MoogleEngine;

public class TF_IDF
{
    public static Dictionary<int, Dictionary<string, double>> Calcular(List<string> docs, List<string> query)
    {
        Dictionary<int, Dictionary<string, double>> result = new Dictionary<int, Dictionary<string, double>>();
        
       for(int i = 0; i < docs.Count; i++)
        {
            Dictionary<string, double> wordscount = new Dictionary<string, double>();
            List<string> words = docs[i].Split(' ').ToList();
            foreach (string word in words)
            {
                if (wordscount.ContainsKey(word))
                {
                    wordscount[word]++;
                }
                else { wordscount[word] = 1; }
            }
            Dictionary<string, double> TFQuery = new Dictionary<string, double>();
            foreach (string word in query)
            {

                if (wordscount.ContainsKey(word))
                {
                    TFQuery.Add(word, (double)wordscount[word] / words.Count);
                }
            }
            if(TFQuery.Count == 0) { continue; }
            result.Add(i, TFQuery);
        }
       
        foreach (string word in query)
        {
            double IDF = 0;
            int count = 0;
            foreach(var dict in result)
            {
                if(dict.Value.ContainsKey(word)) { count++; }
            }
            int frecuencia = (int)(docs.Count * 0.8);
            if(count > frecuencia)
            {
                foreach (var dict in result)
                {
                    if (dict.Value.ContainsKey(word))
                    {
                        dict.Value.Remove(word);
                        if(dict.Value.Count == 0) { result.Remove(dict.Key); }
                    }
                }
                continue;
            }
            if (count != 0) { IDF=Math.Log10((double)docs.Count / count);}
            else { continue; }
            foreach (var dict in result)
            {
                if (dict.Value.ContainsKey(word))
                {
                    dict.Value[word]*=IDF;
                }
            }
        }
        return result;
    }
}
   
