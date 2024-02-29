using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Obavestavac
{
    public partial class Form1 : Form
    {
        List<Informacija> Spisak;
        bool prikazano = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            StreamReader sr = new StreamReader("local.txt");
            string info = sr.ReadLine();
            Spisak = new List<Informacija>();
            for (int index = 1; info != null; index++)
            {
                string[] infos = info.Split(',');
                string datumo = infos[0];
                string datumd = infos[1];
                string naziv = infos[2];
                DateTime datumoba = DateTime.Parse(datumo);
                DateTime datumdes = DateTime.Parse(datumd);
                string noti = infos[3];
                bool n = true;
                if (noti == "0")
                    n = false;
                if (datumo != null)
                {
                    Spisak.Add(new Informacija(datumoba, datumdes, naziv, n));
                }
                info = sr.ReadLine();
            }
            sr.Close();

            Upis();

            timer1.Interval = 3600000;
            timer1.Start();
            timer1_Tick(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool gotovo = false;
            int i = 0;
            while (!gotovo && Spisak.Count > i)
            {
                if (Spisak[i].DatumObavestenja > DateTime.Now)
                {
                    gotovo = true;
                    timer1.Interval = 3600000;
                }
                else
                {
                    if (Spisak[i].Notifikacije)
                    {
                        if (!prikazano)
                        {
                            prikazano = true;
                            DialogResult dialogResult = MessageBox.Show(Spisak[i].Naziv+" - " + Spisak[i].DatumDesavanja.ToShortDateString() + "\nUdji u aplikaciju da ugasis notifikacije za ovu obavezu.", "Obaveze", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            if (dialogResult == DialogResult.OK)
                            {
                                prikazano = false;
                                this.WindowState = FormWindowState.Normal;
                                this.ShowInTaskbar = true;
                                notifyIcon1.Visible = false;
                            }
                        }
                        gotovo = true;
                        timer1.Interval = 180000;
                    }
                    else
                    {
                        i++;
                        timer1.Interval = 3600000;
                    }
                }
            }
        }

        public void Upis()
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < Spisak.Count - 1; i++)
            {
                for (int j = i + 1; j < Spisak.Count; j++)
                {
                    if (Spisak[i].DatumObavestenja > Spisak[j].DatumObavestenja)
                    {
                        Informacija temp = Spisak[i];
                        Spisak[i] = Spisak[j];
                        Spisak[j] = temp;
                    }
                }
            }

            StreamWriter sw = new StreamWriter("local.txt");
            for (int i = 0; i < Spisak.Count; i++)
            {
                string tempnoti = "Da";
                string tempupis = "1";
                if (Spisak[i].Notifikacije == false)
                {
                    tempnoti = "Ne";
                    tempupis = "0";
                }
                dataGridView1.Rows.Add(Spisak[i].DatumObavestenja.ToString("dd/MM/yyyy"), Spisak[i].DatumDesavanja.ToString("dd/MM/yyyy"), Spisak[i].Naziv, tempnoti);
                sw.WriteLine(Spisak[i].DatumObavestenja.ToString("dd/MM/yyyy") + "," + Spisak[i].DatumDesavanja.ToString("dd/MM/yyyy") + "," + Spisak[i].Naziv + "," + tempupis);
            }
            sw.Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason==CloseReason.UserClosing) 
            {
                notifyIcon1.Visible = true;
                this.WindowState=FormWindowState.Minimized; 
                this.ShowInTaskbar = false;
                e.Cancel = true;
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void buttonNotifikacije_Click(object sender, EventArgs e)
        {
            if (Spisak.Count == 0)
            {
                MessageBox.Show("Nema unesenih podataka");
            }
            else if (dataGridView1.SelectedRows.Count == 1)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                Spisak[index].Toggle();
                Upis();
            }
            else if (dataGridView1.SelectedCells.Count == 1)
            {
                int index = dataGridView1.SelectedCells[0].RowIndex;
                Spisak[index].Toggle();
                Upis();
            }
            else
            {
                MessageBox.Show("Izaberi samo jedan red ili jedno polje");
            }

            timer1_Tick(sender, e);
        }

        private void buttonDodaj_Click(object sender, EventArgs e)
        {
            ObavezaForm of = new ObavezaForm();
            of.ShowDialog();
            if (of.Menjaj == true)
            {
                Spisak.Add(of.izmena);
                Upis();
            }

            timer1_Tick(sender, e);
        }

        private void buttonIzmeni_Click(object sender, EventArgs e)
        {
            if (Spisak.Count == 0)
            {
                return;
            }
            int index;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                index = dataGridView1.SelectedRows[0].Index;
            }
            else if (dataGridView1.SelectedCells.Count == 1)
            {
                index = dataGridView1.SelectedCells[0].RowIndex;
            }
            else
            {
                MessageBox.Show("Izaberi samo jedan red ili jedno polje");
                return;
            }
            ObavezaForm of = new ObavezaForm(Spisak[index].DatumObavestenja, Spisak[index].DatumDesavanja, Spisak[index].Naziv);
            of.ShowDialog();
            if (of.Menjaj == true)
            {
                Spisak[index] = of.izmena;
                Upis();
            }

            timer1_Tick(sender, e);
        }

        private void buttonIzbrisi_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                Delete d = new Delete();
                d.ShowDialog();
                if (d.brisi)
                {
                    Spisak.RemoveAt(index);
                    Upis();
                    MessageBox.Show("Obaveza je izbrisana.");
                }
            }
            else if (dataGridView1.SelectedCells.Count == 1)
            {
                int index = dataGridView1.SelectedCells[0].RowIndex;
                Delete d = new Delete();
                d.ShowDialog();
                if (d.brisi)
                {
                    Spisak.RemoveAt(index);
                    Upis();
                    MessageBox.Show("Obaveza je izbrisana.");
                }
            }
            else
            {
                MessageBox.Show("Izaberi samo jedan red ili jedno polje");
            }

            timer1_Tick(sender, e);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            buttonNotifikacije.Location = new Point(this.Width - 163, 10);
            buttonIzbrisi.Location = new Point(this.Width - 163, 50);
            dataGridView1.Width = this.Width - 34;
            dataGridView1.Height = this.Height - 143;
        }
    }
}
