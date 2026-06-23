using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabalhoPOO
{
    internal class Carta
    {
        private string valor;
        private string naipe;
        private int peso;
        private string path;

        public string[] NaipesValidos = { "Paus", "Copas", "Espadas", "Ouros" };
        public string[] ValoresValidos = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        public string Valor
        {
            get { return valor; }
            set
            {
                if (ValoresValidos.Contains(value))
                    valor = value;
                else throw new Exception("Valor inválido!");
            }
        }
        public string Naipe
        {
            get { return naipe; }
            set
            {
                if (NaipesValidos.Contains(value))
                    naipe = value;
                else throw new Exception("Naipe inválido!");
            }
        }
        public int Peso
        {
            get { return peso; }
            set
            {
                peso = value;
            }
        }

        public string Path { get { return path; } set { path = value; } }

        private void atribuiPeso()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic["A"] = 11;
            dic["2"] = 2;
            dic["3"] = 3;
            dic["4"] = 4;
            dic["5"] = 5;
            dic["6"] = 6;
            dic["7"] = 7;
            dic["8"] = 8;
            dic["9"] = 9;
            dic["10"] = 10;
            dic["J"] = 10;
            dic["Q"] = 10;
            dic["K"] = 10;

            this.Peso = dic[this.Valor];
        }
        public void atribuirPath()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Paus"] = "clubs";
            dic["Copas"] = "hearts";
            dic["Espadas"] = "spades";
            dic["Ouros"] = "diamonds";
            Path = "deck_1/" + dic[this.Naipe] + "_" + this.Valor + ".png";
        }
        public Carta(string _valor, string _naipe)
        {

            Naipe = _naipe;
            Valor = _valor;

            atribuiPeso();
            atribuirPath();
        }

        public Carta()
        {
            Random rnd = new Random();
            Naipe = this.NaipesValidos[rnd.Next(0, this.NaipesValidos.Length)];
            Valor = this.ValoresValidos[rnd.Next(0, this.ValoresValidos.Length)];
            atribuiPeso();

            atribuirPath();
        }
    }

}