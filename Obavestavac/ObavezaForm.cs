using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Obavestavac
{
    public partial class ObavezaForm : Form
    {
        public Informacija izmena;
        public bool Menjaj;
        public ObavezaForm()
        {
            InitializeComponent();
            izmena = new Informacija(DateTime.Now, DateTime.Now, "");
        }

        public ObavezaForm(DateTime datumo, DateTime datumd, string naziv)
        {
            InitializeComponent();
            izmena = new Informacija(datumo, datumd, naziv);
        }

        private void ObavezaForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            textBox1.Text = izmena.Naziv;
            dateTimePicker1.Value = izmena.DatumObavestenja;
            dateTimePicker2.Value = izmena.DatumDesavanja;
            Menjaj = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0 && dateTimePicker1.Value < dateTimePicker2.Value)
            {
                Menjaj = true;
                this.Close();
            }
            else
                MessageBox.Show("Mora da se unese naziv i da datum obavestenja bude pre datuma desavanja");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            izmena.Naziv = textBox1.Text;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            izmena.DatumObavestenja = dateTimePicker1.Value;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            izmena.DatumDesavanja = dateTimePicker2.Value;
        }
    }
}
