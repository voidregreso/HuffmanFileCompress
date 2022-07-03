using System;

namespace FileCompress
{
    /// <summary>
    /// Template Class de elemento de diccionario único
    /// </summary>
    public class HuffmanDicItem
    {
        /// <summary>
        /// Carácter
        /// </summary>
        public char Character { get; set; }

        /// <summary>
        /// Cadena Cifrado
        /// </summary>
        public string Code { get; set; }

        public HuffmanDicItem(char charactor, string code)
        {
            Character = charactor;
            Code = code;
        }

    }

    /// <summary>
    /// Nodo de árbol Huffman
    /// </summary>
    public class HuffmanTreeNode
    {
        /// <summary>
        /// Peso de nodo actual
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Niño izquierdo de nodo actual
        /// </summary>
        public HuffmanTreeNode LeftChild { get; set; }

        /// <summary>
        /// Niño derecho de nodo actual
        /// </summary>
        public HuffmanTreeNode RightChild { get; set; }

        /// <summary>
        /// Elemento de Dato de nodo actual
        /// </summary>
        public char Data { get; set; }

        public HuffmanTreeNode(char data, int weight, HuffmanTreeNode lChild = null,
            HuffmanTreeNode rChild = null)
        {
            LeftChild = lChild;
            Data = data;
            RightChild = rChild;
            Weight = weight;
        }

        public HuffmanTreeNode(int weight)
        {
            Data = '\0';
            Weight = weight;
            LeftChild = RightChild = null;
        }
    }
}
