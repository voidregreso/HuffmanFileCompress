using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCompress
{
    public class HuffmanFactory
    {
        /// <summary>
        /// Contar la frecuencia de caracteres en un conjunto de caracteres, es decir, construir el bosque inicial
        /// </summary>
        private List<HuffmanTreeNode> CreateInitForest(string str)
        {
            if (string.IsNullOrEmpty(str)) return null; // handle exception

            List<HuffmanTreeNode> result = new List<HuffmanTreeNode>();
            char[] charArray = str.ToCharArray();
            List<IGrouping<char, char>> lst = charArray.GroupBy(a => a).ToList(); // Los caracteres idénticos se agrupan y
                                                                                  // los criterios de clasificación son los propios caracteres

            foreach (IGrouping<char, char> g in lst)
            {
                char data = g.Key; // Criterios de clasificación, es decir, palabras clave
                int weight = g.ToList().Count; // Número de elementos del conjunto de palabras clave
                                               // correspondientes a las variables de agrupación de tipo char
                HuffmanTreeNode node = new HuffmanTreeNode(data, weight);
                result.Add(node);
            }
            return result;
        }

        /// <summary>
        /// Construcción de un árbol de Huffman a partir de un bosque de nodos de hoja individuales
        /// </summary>
        public HuffmanTreeNode CreateHuffmanTree(List<HuffmanTreeNode> sources)
        {
            if (sources == null)
                throw new ArgumentNullException();
            if (sources.Count < 2)
                throw new ArgumentException("At least two nodes are required to construct a Huffman Tree.");

            HuffmanTreeNode root = default(HuffmanTreeNode); // Nodo raíz
            bool isNext = true; // ¿Se ha completado la construcción? Si no es así, sigue el bucle

            while (isNext)
            {
                List<HuffmanTreeNode> lst = sources.OrderBy(a => a.Weight).ToList(); // Cada ronda de construcción requiere ordenar los nodos de la hoja del bosque (a) por peso (a.Weight) de menor a mayor
                HuffmanTreeNode n1 = lst[0];
                HuffmanTreeNode n2 = lst[1];
                int weight = n1.Weight + n2.Weight; // El nuevo peso del nodo es la suma de los dos nodos menos ponderados seleccionados
                // y el menor de estos dos nodos será el subárbol izquierdo del nuevo nodo y el mayor será el subárbol derecho
                HuffmanTreeNode node = new HuffmanTreeNode(weight); 
                node.LeftChild = n1;
                node.RightChild = n2;

                if (lst.Count == 2) // Completado
                {
                    root = node;
                    isNext = false;
                }
                else
                {
                    sources = lst.GetRange(2, lst.Count - 2); // Eliminar los dos nodos de peso mínimo ya construidos de la lista que se va a actualizar
                    sources.Add(node); // e insertar el nodo recién construido al final de la lista a actualizar, y actualizarlo con los demás nodos en la siguiente ronda
                }
            }
            return root;
        }

        /// <summary>
        /// Construye un diccionario codificado Huffman, que también debe ser definido como una 
        /// función interna debido a la naturaleza recursiva de su construcción
        /// </summary>
        /// <param name="code">La cadena 0/1 que se ha construido al recorrer el nodo actual</param>
        /// <param name="current">Nodo al que se accede actualmente</param>
        /// <returns>Lista de diccionarios</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private List<HuffmanDicItem> CreateHuffmanDict(string code,
    HuffmanTreeNode current)
        {
            if (current == null)
                throw new ArgumentNullException();

            List<HuffmanDicItem> result = new List<HuffmanDicItem>();
            if (current.LeftChild == null && current.RightChild == null) // Cuando se llega al nodo hoja, la travesía está completa
            {
                result.Add(new HuffmanDicItem(current.Data, code)); // Asociar los resultados huffman resultantes con los caracteres correspondientes del diccionario
            }
            else
            {
                // Atraviesan los subárboles izquierdo y derecho respectivamente, con la ruta del subárbol izquierdo
                // establecida en 0 y la del subárbol derecho establecida en 1,
                // y empalmarlo en la cadena de código, recursivamente de arriba a abajo, y finalmente añadir el
                // resultado de la lista del diccionario recursivo al resultado final,
                // y luego volver a salir de la pila.
                List<HuffmanDicItem> dictL = CreateHuffmanDict(code + "0", current.LeftChild);
                List<HuffmanDicItem> dictR = CreateHuffmanDict(code + "1", current.RightChild);
                result.AddRange(dictL);
                result.AddRange(dictR);
            }
            return result;
        }

        /// <summary>
        /// Llamadas externas a funciones internas recursivas para construir conjuntos de diccionarios
        /// </summary>
        public List<HuffmanDicItem> CreateHuffmanDict(HuffmanTreeNode root)
        {
            if (root == null)
                throw new ArgumentNullException();
            return CreateHuffmanDict(string.Empty, root);
        }

        /// <summary>
        /// Recorre el diccionario, escaneando cada carácter de la cadena fuente para que 
        /// coincida con la codificación huffman correspondiente, 
        /// y unir todas las codificaciones de caracteres individuales en una cadena completa
        /// </summary>
        public string ToHuffmanCode(string source, List<HuffmanDicItem> lst)
        {
            if (string.IsNullOrEmpty(source) || lst == null)
                throw new ArgumentNullException();

            string result = string.Empty;
            for (int i = 0; i < source.Length; i++)
            {
                result += lst.Single(a => a.Character == source[i]).Code;
            }
            return result;
        }

        /// <summary>
        /// Funciones llamadas por el mundo exterior para codificar cadenas con huffman
        /// </summary>
        public string StringToHuffmanCode(out List<HuffmanDicItem> dict, string str)
        {
            List<HuffmanTreeNode> forest = CreateInitForest(str); // Contar las frecuencias de los caracteres para construir el bosque inicial
            HuffmanTreeNode root = CreateHuffmanTree(forest); // Construir árboles Huffman
            dict = CreateHuffmanDict(root); // Encuentre la 0/1 cadena de código huffman para cada carácter y almacénela en el diccionario
            string result = ToHuffmanCode(str, dict);
            return result;
        }

        /// <summary>
        /// Las cadenas binarias codificadas en huffman se convierten en bytes hexadecimales en grupos
        /// de ocho y los últimos menos de ocho bits se complementan con ceros al final para facilitar
        /// la escritura en el flujo de archivos binarios.
        /// </summary>
        public byte[] StringToHuffmanBytes(out List<HuffmanDicItem> dict, string str)
        {
            string res = StringToHuffmanCode(out dict, str);
            int len = (res.Length + 7) / 8;
            byte[] dat = new byte[len + 1];
            int pos = 0;
            while(pos + 8 < res.Length) // Cada grupo de ocho se almacena en una matriz de bytes
            {
                string bit = res.Substring(pos, 8);
                dat[pos / 8] = Convert.ToByte(bit, 2);
                pos += 8;
            }
            int restlen = res.Length - pos; // Calcula la longitud de los bits binarios restantes con menos de 8 bits
            string rest = res.Substring(pos, restlen);
            for (int i = 1; i <= 8 - restlen; i++) rest += "0"; // Si el número de dígitos es inferior a 8, añade 0 al final del número hasta llegar a 8.
            dat[len - 1] = Convert.ToByte(rest, 2);
            dat[len] = (byte)restlen; // Se debe recordar el número original de puestos cuando no se cubren
            return dat;
        }

        /// <summary>
        /// Descodificación de la 0/1 cadena cifrado
        /// </summary>
        public string HuffmanCodeToString(List<HuffmanDicItem> dict, string code)
        {
            string result = string.Empty;
            int i = 0;
            while (i < code.Length)
            {
                foreach (HuffmanDicItem item in dict)
                {
                    if (code[i] == item.Code[0] && i + item.Code.Length <= code.Length)
                    {
                        string temp = code.Substring(i, item.Code.Length); // Compara con el código correspondiente del carácter a
                                                                           // comparar en el diccionario y restaura el carácter si coincide
                        if (temp == item.Code)
                        {
                            result += item.Character;
                            i += item.Code.Length;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Si hay menos de 8 dígitos, añada 0 al primer dígito para obtener 8 dígitos
        /// </summary>
        public string FillZero(string str)
        {
            int len = 8 - str.Length;
            string s1 = "";
            for (int i = 1; i <= len; i++) s1 += "0";
            str = s1 + str;
            return str;
        }

        /// <summary>
        /// Matrices de bytes leídas desde flujos de archivos binarios a 0/1 cadenas codificadas en Huffman
        /// </summary>
        public string HuffmanBytesToString(List<HuffmanDicItem> dict, byte[] bs)
        {
            string cs = "", tmp = "";
            for (int i = 0; i < bs.Length - 2; i++) {
                tmp = Convert.ToString(bs[i], 2);
                tmp = FillZero(tmp);
                cs += tmp;
            }
            int len = (int)bs[bs.Length-1];
            tmp = Convert.ToString(bs[bs.Length - 2], 2);
            tmp = FillZero(tmp);
            cs += tmp.Substring(0,len); // Recuperar el último byte y restaurar el byte complementado
            return HuffmanCodeToString(dict, cs);
        }

    }
}
