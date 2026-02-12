using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Bloc_de_Notas___Interfaces
{
    public partial class Form1 : Form
    {

           private string ruta=string.Empty;
           private bool cambios=false;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            cambios=true;
            MostrarEstado("Se realizaron cambios.");
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documentos de texto|*.txt";
            sfd.AddExtension = true;
            DialogResult res = sfd.ShowDialog();

            if (res == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, textBox1.Text);
                ruta = sfd.FileName;
                cambios = false;
                MostrarEstado("Archivo guardado correctamente.");
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ruta) && File.Exists(ruta)) // Guardar
            {
                File.WriteAllText(ruta, textBox1.Text);
            }
            else // Guardar Como
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Documentos de texto|*.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ruta = sfd.FileName;
                    File.WriteAllText(ruta, textBox1.Text);

                }
            }
            cambios = false;
            MostrarEstado("Archivo guardado correctamente.");
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "Documentos de texto|*.txt";
            sfd.AddExtension = true;
            DialogResult res = sfd.ShowDialog();

            if (res == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(sfd.FileName);
                ruta = sfd.FileName;
                cambios = false;
                MostrarEstado("Archivo abierto correctamente.");
            }
                
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cambios == true)
            {
                DialogResult res = MessageBox.Show("Hay cambios sin guardar. ¿Desea guardar antes de salir?","Salir",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    guardarToolStripMenuItem_Click(sender, e);
                    this.Close();
                }
                else if (res == DialogResult.No)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void buscarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        private void MostrarEstado(string mensaje)
        {
            toolStripStatusLabel1.Text = mensaje;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void buscarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(this);
            f.Show();
        }
        public TextBox getTextBox()
        {
            return textBox1;
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }
    }
}
