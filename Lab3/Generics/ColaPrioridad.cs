using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{
    public class ColaPrioridad<T> : IEnumerable<T> where T : IComparable
    {
        private Nodo<T> Raiz { get; set; }
        public int Count { get; set; } = 0;

        //Agrega un nuevo valor a la cola
        public void Add(T nuevo)
        {
            if (Count == 0)
                Raiz = new Nodo<T> { Valor = nuevo };
            else
            {
                Nodo<T> posicion = Raiz;
                int n = Count + 1;
                //Se divide entre dos a la cantidad de valores para buscar la próxima posición gracias a la forma invariante
                Pila<int> direcciones = new Pila<int>();
                while (n > 1)
                {
                    direcciones.Push(n % 2);
                    n /= 2;
                }
                //Se recorre el árbol según las direcciones
                while (direcciones.Count > 1)
                {
                    if (direcciones.Pop() == 0)
                        posicion = posicion.Izquierda;
                    else
                        posicion = posicion.Derecha;
                }
                //Se almacena el nuevo nodo donde corresponde
                if (direcciones.Pop() == 0)
                {
                    posicion.Izquierda = new Nodo<T> { Valor = nuevo };
                    posicion.Izquierda.Padre = posicion;
                    posicion = posicion.Izquierda;
                }
                else
                {
                    posicion.Derecha = new Nodo<T> { Valor = nuevo };
                    posicion.Derecha.Padre = posicion;
                    posicion = posicion.Derecha;
                }
                //Se realizan intercambios por las prioridades hasta llegar a la raíz
                while (posicion.Padre != null)
                {
                    if (posicion.Valor.CompareTo(posicion.Padre.Valor) < 0)
                    {
                        T aux = posicion.Valor;
                        posicion.Valor = posicion.Padre.Valor;
                        posicion.Padre.Valor = aux;
                    }
                    posicion = posicion.Padre;
                }
            }
            Count++;
        }

        //Elimina el nodo de la raíz, el de mayor prioridad
        public T Remove()
        {
            if (Count > 0)
            {
                T valorPorDevolver = Raiz.Valor;
                if (Count == 1)
                    Raiz = null;
                else
                {
                    Nodo<T> posicion = Raiz;
                    int n = Count;
                    //Al igual que con la inserción, se busca la posición con el cual reemplazar la raíz gracias a la forma invariante
                    Pila<int> direcciones = new Pila<int>();
                    while (n > 1)
                    {
                        direcciones.Push(n % 2);
                        n /= 2;
                    }
                    while (direcciones.Count > 1)
                    {
                        if (direcciones.Pop() == 0)
                            posicion = posicion.Izquierda;
                        else
                            posicion = posicion.Derecha;
                    }
                    if (direcciones.Pop() == 0)
                    {
                        Raiz.Valor = posicion.Izquierda.Valor;
                        posicion.Izquierda = null;
                    }
                    else
                    {
                        Raiz.Valor = posicion.Derecha.Valor;
                        posicion.Derecha = null;
                    }
                    posicion = Raiz;
                    //Se realizan los intercambios de los valores por las prioridades
                    while (posicion.Izquierda != null)
                    {
                        if (posicion.Derecha != null)
                        {
                            if (posicion.Valor.CompareTo(posicion.Izquierda.Valor) <= 0 && posicion.Valor.CompareTo(posicion.Derecha.Valor) <= 0)
                                posicion = new Nodo<T>();
                            else
                            {
                                if (posicion.Derecha.Valor.CompareTo(posicion.Izquierda.Valor) < 0)
                                {
                                    T aux = posicion.Valor;
                                    posicion.Valor = posicion.Derecha.Valor;
                                    posicion.Derecha.Valor = aux;
                                }
                                else
                                {
                                    T aux = posicion.Valor;
                                    posicion.Valor = posicion.Izquierda.Valor;
                                    posicion.Izquierda.Valor = aux;
                                }
                            }
                        }
                        else
                        {
                            if (posicion.Valor.CompareTo(posicion.Izquierda.Valor) <= 0)
                                posicion = new Nodo<T>();
                            else
                            {
                                T aux = posicion.Valor;
                                posicion.Valor = posicion.Izquierda.Valor;
                                posicion.Izquierda.Valor = aux;
                            }
                        }
                    }
                }
                Count--;
                return valorPorDevolver;
            }
            else
                return default;
        }

        //Elimina el valor de la raíz sin devolverlo
        public void Delete()
        {
            Remove();
        }

        //Devuelve el valor de la raíz sin eliminarlo
        public T Get()
        {
            if (Raiz != null)
                return Raiz.Valor;
            else
                return default;
        }

        //Deja vacía la cola
        public void Clear()
        {
            Raiz = null;
            Count = 0;
        }

        //Recorrido que devuelve todos los valores en una lista
        private void Inorden(Nodo<T> position, List<T> recorrido)
        {
            if (position.Izquierda != null)
                Inorden(position.Izquierda, recorrido);
            recorrido.Add(position.Valor);
            if (position.Derecha != null)
                Inorden(position.Derecha, recorrido);
        }

        //Método heredado de IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            List<T> recorrido = new List<T>();
            if (Raiz != null)
            {
                Inorden(Raiz, recorrido);
                recorrido.Sort();
            }
            while (recorrido.Count > 0)
            {
                yield return recorrido[0];
                recorrido.Remove(recorrido[0]);
            }
        }

        //Método heredado de IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
