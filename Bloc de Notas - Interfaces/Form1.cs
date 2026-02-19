using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Bloc_de_Notas___Interfaces
{
    public partial class Form1 : Form
    {
        private HashSet<TabPage> pestañasConCambios = new HashSet<TabPage>();
        private bool converting = false;

        Dictionary<string, Image> emojiImgs = new Dictionary<string, Image>()
        {
            { ":)", Properties.Resources.feliz },
            { ":(", Properties.Resources.triste },
            { ";)", Properties.Resources.guino },
            { ":D", Properties.Resources.contento }
        };

        public Form1()
        {
            InitializeComponent();
            nuevaPestaña();
        }

        private bool PestañaTieneCambios(TabPage page = null)
        {
            return pestañasConCambios.Contains(page ?? tabControl1.SelectedTab);
        }

        private void MarcarCambios(TabPage page, bool tieneCambios)
        {
            if (tieneCambios)
                pestañasConCambios.Add(page);
            else
                pestañasConCambios.Remove(page);
        }

        public RichTextBox pestañaActual()
        {
            if (tabControl1.SelectedTab == null) return null;
            return tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
        }

        private void MostrarEstado(string mensaje)
        {
            toolStripStatusLabel1.Text = mensaje;
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
                File.WriteAllText(sfd.FileName, TextoPlano(actual));

                tabControl1.SelectedTab.Tag = sfd.FileName;
                tabControl1.SelectedTab.Text = Path.GetFileName(sfd.FileName);

                MarcarCambios(tabControl1.SelectedTab, false);
                MostrarEstado("Archivo guardado.");
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;
            RichTextBox actual = pestañaActual();
            if (actual == null) return;

            string ruta = tabControl1.SelectedTab.Tag as string;

            if (!string.IsNullOrEmpty(ruta) && File.Exists(ruta))
            {
                File.WriteAllText(ruta, TextoPlano(actual));
                MarcarCambios(tabControl1.SelectedTab, false);
                MostrarEstado("Archivo guardado.");
            }
            else
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

                RichTextBox nuevo = CrearRichTextBox(pagina);
                nuevo.Text = File.ReadAllText(ofd.FileName);
                Convertir(nuevo);

                pagina.Controls.Add(nuevo);
                tabControl1.TabPages.Add(pagina);
                tabControl1.SelectedTab = pagina;

                MarcarCambios(pagina, false);
                MostrarEstado("Archivo abierto correctamente.");
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pestañasConCambios.Count > 0)
            {
                DialogResult res = MessageBox.Show(
                    "Hay cambios sin guardar. ¿Desea guardar antes de salir?",
                    "Salir", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

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

        private void nuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevaPestaña();
            MostrarEstado("Pestaña creada correctamente.");
        }

        private void nuevaPestaña()
        {
            TabPage nuevaPagina = new TabPage("Nuevo documento");
            nuevaPagina.Tag = null;

            RichTextBox nuevo = CrearRichTextBox(nuevaPagina);
            nuevaPagina.Controls.Add(nuevo);
            tabControl1.TabPages.Add(nuevaPagina);
            tabControl1.SelectedTab = nuevaPagina;
        }

        private RichTextBox CrearRichTextBox(TabPage pagina)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            rtb.Multiline = true;
            rtb.AcceptsTab = true;

            rtb.TextChanged += (s, e) =>
            {
                if (converting) return;

                Convertir(rtb);

                MarcarCambios(pagina, true);
                MostrarEstado("Se realizaron cambios.");
            };

            return rtb;
        }

        private void CerrarPestanaActual()
        {
            if (tabControl1.SelectedTab == null) return;

            TabPage actual = tabControl1.SelectedTab;

            if (PestañaTieneCambios(actual))
            {
                DialogResult res = MessageBox.Show(
                    "Hay cambios sin guardar. ¿Quiere guardar?",
                    "Cerrar pestaña", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (res == DialogResult.Cancel) return;
                if (res == DialogResult.Yes) guardarToolStripMenuItem_Click(null, null);
            }

            pestañasConCambios.Remove(actual);
            tabControl1.TabPages.Remove(actual);
            MostrarEstado("Pestaña cerrada.");
        }

        private void cerrarPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CerrarPestanaActual();
        }


        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pestañasConCambios.Count == 0) return;

            DialogResult res = MessageBox.Show(
                "Hay cambios sin guardar. ¿Desea guardar antes de salir?",
                "Salir", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (res == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (res == DialogResult.Yes)
            {
                guardarToolStripMenuItem_Click(null, null);
                if (pestañasConCambios.Count > 0)
                    e.Cancel = true;
            }
        }

        private void buscarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(this);
            f.Show();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e) { }
        private void buscarToolStripMenuItem_Click(object sender, EventArgs e) { }


        private void Convertir(RichTextBox rtb)
        {
            converting = true;
            try
            {
                int cursor = rtb.SelectionStart;

                foreach (var emo in emojiImgs)
                {
                    int index;
                    while ((index = rtb.Text.IndexOf(emo.Key)) != -1)
                    {
                        rtb.Select(index, emo.Key.Length);
                        rtb.SelectedText = "";
                        InsertarImagen(rtb, emo.Value, index);
                    }
                }

                rtb.SelectionStart = Math.Min(cursor, rtb.TextLength);
                rtb.SelectionLength = 0;
            }
            finally
            {
                converting = false;
            }
        }

        private string TextoPlano(RichTextBox rtb)
        {
            string rtf = rtb.Rtf;

            foreach (var emo in emojiImgs)
            {
                string pictBlock = GetRtfPictBlock(emo.Value);
                if (pictBlock != null)
                    rtf = rtf.Replace(pictBlock, emo.Key);
            }

            using (RichTextBox temp = new RichTextBox())
            {
                try
                {
                    temp.Rtf = rtf;
                    return temp.Text;
                }
                catch
                {
                    return rtb.Text;
                }
            }
        }

        private Dictionary<Image, string> _pictBlockCache = new Dictionary<Image, string>();

        private string GetRtfPictBlock(Image img)
        {
            if (_pictBlockCache.TryGetValue(img, out string cached))
                return cached;

            using (RichTextBox temp = new RichTextBox())
            {
                Clipboard.SetImage(img);
                temp.Paste();
                Clipboard.Clear();

                string rtf = temp.Rtf ?? "";

                int start = rtf.IndexOf("{\\pict");
                if (start == -1)
                {
                    _pictBlockCache[img] = null;
                    return null;
                }

                int depth = 0, end = -1;
                for (int i = start; i < rtf.Length; i++)
                {
                    if (rtf[i] == '{') depth++;
                    else if (rtf[i] == '}')
                    {
                        depth--;
                        if (depth == 0) { end = i; break; }
                    }
                }

                if (end == -1)
                {
                    _pictBlockCache[img] = null;
                    return null;
                }

                string block = rtf.Substring(start, end - start + 1);
                _pictBlockCache[img] = block;
                return block;
            }
        }

        private void InsertarImagen(RichTextBox rtb, Image img, int index)
        {
            rtb.Select(index, 0);

            Clipboard.SetImage(img);
            rtb.Paste();
            Clipboard.Clear();
        }
    }
}