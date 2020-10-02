using System;
using System.Collections.Generic;
using System.Text;

namespace Generics
{
    public class NodoLineal<T>
    {
        public NodoLineal<T> Siguiente { get; set; }
        public NodoLineal<T> Anterior { get; set; }
        public T Valor { get; set; }
    }
}
