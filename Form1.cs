using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FileCompress
{
    public partial class Form1 : Form
    {
        public string nombre = "";
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Eliminar la extensión al final del nombre del archivo en la ruta completa, incluyendo el punto.
        /// </summary>
        private string EliminateSuffix(string path)
        {
            return Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Evento de respuesta de clic para el botón de diálogo "Seleccionar texto/archivo cifrado".
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Please choose file";
            if(encdecbox.Checked) fileDialog.Filter = "Nuestro compress file format(*.zzz)|*.zzz|All files(*.*)|*.*";
            else fileDialog.Filter = "Text files(*.txt;*.log)|*.txt;*.log|All files(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                nombre = fileDialog.FileName;
                filepath.Text = fileDialog.FileName;
            }
        }

        /// <summary>
        /// Escribir la información del diccionario en un archivo de configuración de texto que 
        /// se utilizará como base para restaurar los caracteres la próxima vez que se descifre el archivo zzz.
        /// </summary>
        private bool WriteDictFile(List<HuffmanDicItem> dics, string dictpath)
        {
            // Formato del diccionario: Cada elemento de la línea es:
            // <Código ASCII de caracteres hexadecimales> <Cadena de códigos Huffman>
            try
            {
                string str = "";
                // Hay que tener en cuenta que los caracteres pueden ser espacios y símbolos especiales.
                // Y en este caso no es conveniente restaurar el diccionario la próxima vez
                // que se lea, por lo que su código ASCII se almacena en hexadecimal.
                foreach (HuffmanDicItem item in dics)
                {
                    str += (Convert.ToString(item.Character, 16) + " " + item.Code) + "\n";
                }
                byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
                using (FileStream fsWrite = new FileStream(dictpath, FileMode.Create))
                {
                    fsWrite.Write(data, 0, data.Length);
                };
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Leer archivos de diccionario.
        /// </summary>
        private List<HuffmanDicItem> ReadDictFile(string dictpath)
        {
            List<HuffmanDicItem> hds = new List<HuffmanDicItem>();
            try
            {
                string[] lines = File.ReadAllLines(dictpath);
                foreach (string line in lines)
                {
                    string[] dat = line.Split(' ');
                    char ch = (char)Convert.ToByte(dat[0], 16);
                    string cstr = dat[1];
                    HuffmanDicItem hf = new HuffmanDicItem(ch, cstr);
                    hds.Add(hf);
                }
            } catch (Exception)
            {
            }
            return hds;
        }

        /// <summary>
        /// Evento de respuesta para el clic del botón "Realizar la tarea de cifrado/descifrado".
        /// </summary>
        private void hacer_Click(object sender, EventArgs e)
        {
            if(File.Exists(nombre))
            {
                if (encdecbox.Checked) // Descifrado
                {
                    string ndict = EliminateSuffix(nombre) + ".dict";
                    string nfile = EliminateSuffix(nombre) + "_decoded.txt";
                    if (File.Exists(ndict))
                    {
                        HuffmanFactory hf = new HuffmanFactory();
                        // Leer información de datos comprimidos
                        FileStream f0 = new FileStream(nombre, FileMode.Open, FileAccess.Read);
                        BinaryReader r0 = new BinaryReader(f0);
                        byte[] fb = r0.ReadBytes((int)f0.Length);
                        // Recuerde siempre cerrar el flujo de archivos, de lo contrario el
                        // archivo permanecerá ocupado hasta que el programa salga.
                        f0.Dispose();

                        // Leer la información del diccionario
                        List<HuffmanDicItem> dic = ReadDictFile(ndict);
                        if(dic.Count == 0)
                        {
                            MessageBox.Show("Failed to extract information from dictionary file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        string decode = hf.HuffmanBytesToString(dic, fb); // Descifrado
                        File.WriteAllText(nfile, decode); // Escribir los datos decodificados en un archivo de texto
                        MessageBox.Show("File extracted successfully", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } else MessageBox.Show("The corresponding huffman character encoding dictionary file does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else // Cifrado
                {
                    HuffmanFactory hf = new HuffmanFactory();
                    List<HuffmanDicItem> dic = new List<HuffmanDicItem>(); // Diccionario de codificación de frecuencia - carácter
                    byte[] myByte;
                    try
                    {
                        using (FileStream fsRead = new FileStream(nombre, FileMode.Open))
                        {
                            int fsLen = (int)fsRead.Length;
                            byte[] heByte = new byte[fsLen];
                            int r = fsRead.Read(heByte, 0, heByte.Length); 
                            string myStr = System.Text.Encoding.UTF8.GetString(heByte);
                            myByte = hf.StringToHuffmanBytes(out dic, myStr); // Cifrado de Huffman
                        }
                        if (myByte.Length > 0)
                        {
                            string nfile = EliminateSuffix(nombre) + ".zzz";
                            string ndict = EliminateSuffix(nombre) + ".dict";
                            // Escribir información de datos comprimidos
                            using (FileStream fsWrite = new FileStream(nfile, FileMode.Create))
                            {
                                fsWrite.Write(myByte, 0, myByte.Length);
                            };
                            // Escribir el archivo de diccionario de codificación huffman para cada carácter
                            if (!WriteDictFile(dic, ndict)) throw new InvalidOperationException("Cannot write Huffman dictionary to file!");
                            double rate = (double)new FileInfo(nfile).Length / (double)new FileInfo(nombre).Length * 100.0;
                            MessageBox.Show("File compressed successfully with rate of " + Convert.ToString(rate) + "%", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    } catch (Exception ex)
                    {
                        MessageBox.Show("Error occurred: \n" + ex.Message + "\n" + ex.StackTrace, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            } else
            {
                MessageBox.Show("File does not exist","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void filepath_TextChanged(object sender, EventArgs e)
        {
            nombre = filepath.Text;
        }

    }
}
