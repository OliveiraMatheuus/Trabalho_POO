using System;
using System.Collections.Generic;
using System.Linq;

namespace TrabalhoPOO
{
    internal class Carta
    {
        private static readonly Random _rnd = new Random();

        private string valor;
        private string naipe;
        private int peso;
        private string path;

        public static readonly string[] NaipesValidos = { "Paus", "Copas", "Espadas", "Ouros" };
        public static readonly string[] ValoresValidos = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        public string Valor
        {
            get { return valor; }
            set
            {
                if (ValoresValidos.Contains(value))
                    valor = value;
                else
                    throw new Exception("Valor inválido!");
            }
        }

        public string Naipe
        {
            get { return naipe; }
            set
            {
                if (NaipesValidos.Contains(value))
                    naipe = value;
                else
                    throw new Exception("Naipe inválido!");
            }
        }

        public int Peso
        {
            get { return peso; }
            private set { peso = value; }
        }

        public string Path
        {
            get { return path; }
            private set { path = value; }
        }

        private static readonly Dictionary<string, int> _pesoDic = new Dictionary<string, int>
        {
            ["A"] = 11,
            ["2"] = 2,
            ["3"] = 3,
            ["4"] = 4,
            ["5"] = 5,
            ["6"] = 6,
            ["7"] = 7,
            ["8"] = 8,
            ["9"] = 9,
            ["10"] = 10,
            ["J"] = 10,
            ["Q"] = 10,
            ["K"] = 10
        };

        private static readonly Dictionary<string, string> _naipeDic = new Dictionary<string, string>
        {
            ["Paus"] = "clubs",
            ["Copas"] = "hearts",
            ["Espadas"] = "spades",
            ["Ouros"] = "diamonds"
        };

        private void AtribuirPeso()
        {
            Peso = _pesoDic[Valor];
        }

        private void AtribuirPath()
        {
            Path = "deck_1/" + _naipeDic[Naipe] + "_" + Valor + ".png";
        }

        public Carta(string _valor, string _naipe)
        {
            Naipe = _naipe;
            Valor = _valor;
            AtribuirPeso();
            AtribuirPath();
        }

        public Carta()
        {
            Naipe = NaipesValidos[_rnd.Next(0, NaipesValidos.Length)];
            Valor = ValoresValidos[_rnd.Next(0, ValoresValidos.Length)];
            AtribuirPeso();
            AtribuirPath();
        }

        public override string ToString() => $"{Valor} de {Naipe}";
    }
}