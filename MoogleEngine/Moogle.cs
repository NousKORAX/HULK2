
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.RegularExpressions;

namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        query= query.ToLower();

        //Esta expresion regular coincide con cualquier caracter que no sea una letra o un numero
        string pattern = @"[^a-z0-9óéíüúáñ]+";
        Regex rgx = new Regex(pattern, RegexOptions.Compiled)
            ;
        //En esta linea reemplazo todos los caracteres que coinciden con la expresion regular
        //una o mas veces con un espacio en blanco
        query = rgx.Replace(query, " ");

        //Despues aplico el metodo Split() para separar la cadena por los espacios en blanco y llevarla a una lista
        List<string> WordsQuery = query.Split(' ',StringSplitOptions.RemoveEmptyEntries).ToList();
        string rute = @"C:\Users\Ronal\Downloads\Moogle-main\Content";

        //Con este metodo voy accediendo a los txt que estan en una ruta determinada ylos agrego a la lista
        List<string> files = Directory.GetFiles(rute, "*.txt").ToList();

        //En esta lista van a estar los titulos de cada documento
        List<string> titles = new List<string>();

        //En esta lista van a estar los contenidos de cada documento tal como aparecen
        List<string> FilesInformation1 = new List<string>();

        //En esta lista van a estar los contenidos de cada documento aplicandoles el mismo metodo Replace()
        List<string> FilesInformation = new List<string>();

        //Voy añadiendo los elementos a cada lista segun correspondan 
        for (int i = 0; i < files.Count; i++)
        {
           string content = File.ReadAllText(files[i]);
            FilesInformation1.Add(content);
           content=content.ToLower();
            content= rgx.Replace(content, " ");
            FilesInformation.Add(content);
        }
        //En este bucle recorro el contenido de cada documento y si esta vacio se lo quito a cada lista
        for (int i = 0; i < FilesInformation.Count; i++)
        {
            if (string.IsNullOrEmpty(FilesInformation[i]))
            {

                FilesInformation.RemoveAt(i);
                files.RemoveAt(i);
                FilesInformation1.RemoveAt(i);
            }
        }

        //Aqui voy agregando los titulos de los txt sin la extension
        for (int i = 0; i < files.Count; i++)
        {
            titles.Add(Path.GetFileNameWithoutExtension(files[i]));
        }
        Dictionary<int, Dictionary<string, double>> QueryValues = TF_IDF.Calcular(FilesInformation, WordsQuery);
        Dictionary<int,double> Scores= new Dictionary<int, double>();
        foreach (var item in QueryValues)
        {
            List<int> VectorBinario= new List<int>();
            List<double> values= new List<double>();
            foreach (string item1 in WordsQuery)
            {
                if (item.Value.ContainsKey(item1)) { VectorBinario.Add(1); values.Add(item.Value[item1]); }
                else { VectorBinario.Add(1);values.Add(0); }
            }
            double similitud=Coseno.Similitud(VectorBinario, values);
            Scores.Add(item.Key, similitud);
        }
        Scores =Dict.SortDictionary(Scores);
        List<double> scores=Scores.Values.ToList();
        List<int> indices=Scores.Keys.ToList();
        scores.Reverse();
        indices.Reverse();
        List<SearchItem> Items = new List<SearchItem>();
        for (int i=0;i<indices.Count;i++)
        {
           List<double> values = QueryValues[indices[i]].Values.ToList();
            double MaxValue=values.Max();
            string word = "";
            foreach (var par in QueryValues[indices[i]])
            {
                if (par.Value == MaxValue) { word = par.Key;  }
            }
            Items.Add(new SearchItem(titles[indices[i]], Coseno.Snippet(word, FilesInformation1[indices[i]]) , scores[i]));
            if (i > 10) { break; }
        }
       
        return new SearchResult(Items.ToArray(), "");

    }
}
