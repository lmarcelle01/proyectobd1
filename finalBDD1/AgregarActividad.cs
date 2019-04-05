using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalBDD1
{
    public partial class AgregarActividad : Form
    {
        int usuario, project;

        public AgregarActividad(int usuario, int project)
        {
            this.usuario = usuario;
            this.project = project;
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            BDD A = new BDD();
            //  var actDone = A.SelectActividadDone(project);
            //  var actNotDone = A.SelectActividadesNotDone(project);
            //  var actividades = new Dictionary<int, string>();

            var colaboradores = A.GetProjectColaborators(project);
            var complejidad = A.GetComplexities();

           
            if (colaboradores.Count > 0)
            {
                comboBox1.DataSource = new BindingSource(colaboradores, null);
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";
            }

            comboBox2.DataSource = new BindingSource(complejidad, null);
            comboBox2.DisplayMember = "Value";
            comboBox2.ValueMember = "Key";
            
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == null || textBox2.Text == "" || textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("No se permiten campos vacios");
                return;
            }


            BDD A = new BDD();
            int complejidad = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
            int encargado = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            var created = A.AgregarActividad(project,encargado, textBox2.Text, textBox3.Text, complejidad, 0 );
            if (created)
            {
                DetalleProyecto p = new DetalleProyecto(usuario, project);
                this.Hide();
                p.Show();
            }
            else
                MessageBox.Show("Ha ocurrido un error! :-(");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            DetalleProyecto a = new DetalleProyecto(usuario, project);
            a.Show();
        }
    }
}
