using System;
using System.Collections.Generic;
using System.Text;

namespace Generics
{
    public class Pila<T>
    {
        private NodoLineal<T> Head;
        public int Count { get; set; } = 0;

        public void Push(T value)
        {
            if (Head == null)
                Head = new NodoLineal<T> { Valor = value, Anterior = null, Siguiente = null };
            else
            {
                Head.Siguiente = new NodoLineal<T> { Valor = value, Anterior = Head, Siguiente = null };
                Head = Head.Siguiente;
            }
            Count++;
        }

        public void Delete()
        {
            Pop();
        }

        public T Get()
        {
            if (Head != null)
                return Head.Valor;
            else
                return default;
        }

        public T Pop()
        {
            if (Head != null)
            {
                T valor = Head.Valor;
                Head = Head.Anterior;
                if (Head != null)
                    Head.Siguiente = null;
                Count--;
                return valor;
            }
            else
                return default;
        }

        public void Clear()
        {
            Head = null;
            Count = 0;
        }
    }
}
