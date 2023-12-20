using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine;

 public class Coseno
{

    //Calculo la similitud entre dos vectores
    public static double Similitud(List<int> vector1, List<double> vector2)
    {
        double productoEscalar = 0;
        double magnitud1 = 0;
        double magnitud2 = 0;

        for (int i = 0; i < vector1.Count; i++)
        {
            productoEscalar += vector1[i] * vector2[i];
            magnitud1 += Math.Pow(vector1[i], 2);
            magnitud2 += Math.Pow(vector2[i], 2);
        }
        magnitud1 = Math.Sqrt(magnitud1);
        magnitud2 = Math.Sqrt(magnitud2);
        if(magnitud1==0 || magnitud2 == 0)
        {
            return 0;
        }
        return productoEscalar / (magnitud1 * magnitud2);
    }

    public static string Snippet( string txt,string doc_content1)
    {
        List<string> result = new List<string>();
        List<string> strings1=doc_content1.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        int indice = 0;
        for(int i=0; i < strings1.Count; i++)
        {
            if (strings1[i]== txt) { indice = i; break; }
        }
       for (int i = indice; i >= 0 && i >= indice - 50; i--)
            {
                result.Add(strings1[i]);
            }
            result.Reverse();
        for(int i = indice +1;i<strings1.Count && i<=indice+50; i++)
        {
            result.Add(strings1[i]);
        }

        string snippet = string.Join(" ", result);
        return snippet;
       
    }
}


