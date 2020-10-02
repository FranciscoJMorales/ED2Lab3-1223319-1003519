using System;
using System.Collections.Generic;
using System.Text;

namespace Generics
{
    public class Nodo<T> : IComparable where T : IComparable
    {
        public Nodo<T> Padre { get; set; }
        public Nodo<T> Izquierda { get; set; }
        public Nodo<T> Derecha { get; set; }
        public T Valor { get; set; }

        public Nodo(T val)
        {
            Valor = val;
        }

        public Nodo()
        {

        }

        public int CompareTo(object obj)
        {
            return Valor.CompareTo(((Nodo<T>)obj).Valor);
        }
    }
}
