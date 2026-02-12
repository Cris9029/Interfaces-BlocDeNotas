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
         
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;

            RichTextBox actual = pestañaActual();
            if (actual == null) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documentos de texto|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, actual.Text);

                // Guardar la ruta en la pestaña
                tabControl1.SelectedTab.Tag = sfd.FileName;

                // Cambiar nombre de la pestaña
                tabControl1.SelectedTab.Text = Path.GetFileName(sfd.FileName);

                cambios = false;
                MostrarEstado("Archivo guardado.");
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;

            RichTextBox actual = pestañaActual();
            if (actual == null) return;

            string ruta = tabControl1.SelectedTab.Tag as string;

            //Guardar
            if (!string.IsNullOrEmpty(ruta) && File.Exists(ruta))
            {
                File.WriteAllText(ruta, actual.Text);
                cambios = false;
                MostrarEstado("Archivo guardado.");
            }
            else //Guardar Como
            {
                guardarComoToolStripMenuItem_Click(sender, e);
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Documentos de texto|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                TabPage pagina = new TabPage(Path.GetFileName(ofd.FileName));
                pagina.Tag = ofd.FileName;

                RichTextBox nuevo = new RichTextBox();
                nuevo.Dock = DockStyle.Fill;
                nuevo.Multiline = true;
                nuevo.AcceptsTab = true;
                nuevo.Text = File.ReadAllText(ofd.FileName);

                nuevo.TextChanged += (s, f) =>
                {
                    cambios = true;
                    MostrarEstado("Se realizaron cambios.");
                };

                pagina.Controls.Add(nuevo);
                tabControl1.TabPages.Add(pagina);
                tabControl1.SelectedTab = pagina;

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
       
        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }

        private void nuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevaPestaña();
            MostrarEstado("Pestaña creada correctamente.");
        }
        private void nuevaPestaña()
        {
            TabPage nuevaPagina = new TabPage("Nuevo documento");
            nuevaPagina.Tag = null;

            RichTextBox nuevo = new RichTextBox();
            nuevo.Multiline = true;
            nuevo.Dock = DockStyle.Fill;
            nuevo.AcceptsTab = true;

            nuevo.TextChanged += (s, e) =>
            {
                cambios = true;
                MostrarEstado("Se realizaron cambios.");
            };

            nuevaPagina.Controls.Add(nuevo);
            tabControl1.TabPages.Add(nuevaPagina);
            tabControl1.SelectedTab = nuevaPagina;
        }
        public RichTextBox pestañaActual()
        {
            if (tabControl1.SelectedTab == null) return null;

            return tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
        }

        public void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            MostrarEstado("Se realizaron cambios.");
        }
        private void CerrarPestanaActual()
        {
            if (tabControl1.SelectedTab == null) return;

            RichTextBox actual = pestañaActual();
            if (actual == null) return;

            if (cambios)
            {
                DialogResult res = MessageBox.Show(
                    "Hay cambios sin guardar. ¿Quiere guardar?","Cerrar pestaña", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning);

                if (res == DialogResult.Cancel) return;

                if (res == DialogResult.Yes)
                {
                    guardarToolStripMenuItem_Click(null, null);
                }
            }

            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            MostrarEstado("Pestaña cerrada.");
        }

        private void cerrarPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CerrarPestanaActual();
        }
    }
}
