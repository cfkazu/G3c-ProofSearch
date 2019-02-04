using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace G3c
{
  public class Figure
    {
        public List<Figure> Figures = new List<Figure>();
        public string result;
        public int Depth;
        public Figure()
        { }
        public Figure(string result)
        {
            this.result = result;
        }
        public Figure(List<Figure>Figures,string result)
        {
            this.Figures = new List<Figure>(Figures); this.result = result;
        }
        public void Addfigure(Figure fig)
        {
            Figures.Add(fig);
        }
        public void OutPut(int Depth)
        {
            Console.WriteLine(result+Depth);
            if (Figures.Count > 1) { Console.WriteLine(Figures.Count + "個の場合分け"); }
            foreach (var  i in Figures)
            {
                i.OutPut(Depth+1);
            }
            if(Figures.Count()==0){ Console.WriteLine("-"); }
            
        }

        public void OutPut(StreamWriter writer,int Depth)
        {
            writer.WriteLine(result+Depth);
            if (Figures.Count > 1) { writer.WriteLine(Figures.Count + "個の場合分け({0})",Depth); }
            for (int i = 0;  i< Figures.Count; i++)
            {
                if (Figures.Count > 1) { Console.Write(i + ":"); }
                Figures[i].OutPut(writer, Depth + 1);
            }
          
            if (Figures.Count > 1) { writer.WriteLine(Figures.Count + "個の場合分け終了({0})", Depth); }
            if (Figures.Count() == 0) { writer.WriteLine("-"); }
        }
    }
}
