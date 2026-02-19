using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bloc_de_Notas___Interfaces
{
    public partial class Form2 : Form
    {
        private Form1 f;
        private int pos = -1;
        private bool direccion = true;

        public Form2(Form1 f)
        {
            InitializeComponent();
            this.f = f;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RichTextBox tb1 = f.pestañaActual();
            if (tb1 == null) return;
            textBox1.Text = "";
            tb1.SelectionLength = 0;
            tb1.SelectionStart = 0;
            tb1.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pos = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            direccion = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            direccion = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pos = -1;
        }
        private void Buscar()
        {
            RichTextBox tb1 = f.pestañaActual();
            if (tb1 == null) return;
            string buscar = textBox1.Text;
            if (string.IsNullOrEmpty(buscar))
                return;
            StringComparison comp;
            if (checkBox1.Checked)
            {
                comp = StringComparison.Ordinal;
            }
            else
            {
                comp = StringComparison.OrdinalIgnoreCase;
            }
            int inicio;
            if (direccion)
            {
                if (pos < 0)
                {
                    inicio = tb1.SelectionStart;
                }
                else
                {
                    inicio = pos + 1;
                }
                if (inicio >= tb1.Text.Length)
                    inicio = 0;

                pos = tb1.Text.IndexOf(buscar, inicio, comp);
            }
            else
            {
                if (pos < 0)
                {
                    inicio = tb1.SelectionStart;
                }
                else
                {
                    inicio = pos - 1;
                }
                if (inicio < 0)
                    inicio = tb1.Text.Length - 1;

                pos = tb1.Text.LastIndexOf(buscar, inicio, comp);
            }
            if (pos >= 0)
            {
                tb1.SelectionStart = pos;
                tb1.SelectionLength = buscar.Length;
                tb1.Focus();
            }
            else
            {
                MessageBox.Show("Texto no encontrado", "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            direccion = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            direccion = true;
        }
    }
}
