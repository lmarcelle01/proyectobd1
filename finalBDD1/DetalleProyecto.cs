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
    public partial class DetalleProyecto : Form
    {
        int usuario;
        int project;

        public DetalleProyecto(int idUsuario, int idProyecto)
        {
            this.usuario = idUsuario;
            this.project = idProyecto;
            InitializeComponent();
            load();

        }



        public void load()
        {

            BDD A = new BDD();
            var actDone = A.SelectActividadDone(project);
            var actNotDone = A.SelectActividadesNotDone(project);
            var actividades = new Dictionary<int, string>();
            foreach(var a in actDone)
            {
                actividades.Add(a.Key, a.Value);
            }
            foreach(var a in actNotDone)
            {
                actividades.Add(a.Key, a.Value);
            }

            if(actividades.Count > 0)
            {
                comboBox2.DataSource = new BindingSource(actividades, null);
                comboBox2.DisplayMember = "Value";
                comboBox2.ValueMember = "Key";
            }
            


            var colaboradores = A.GetProjectColaborators(project);

            richTextBox1.AppendText(A.GetProjectDescription(project));

            label5.Text = A.GetProjectTitle(project);

            foreach ( var a in actNotDone)
            {
                listBox2.Items.Add(a.Value);
            }
            foreach(var a in actDone)
            {
                listBox3.Items.Add(a.Value);
            }

            foreach(var a in colaboradores)
            {
                listBox1.Items.Add(a.Value);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // go to update project
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var a = new UpdateProject(usuario, project);
            a.Show();
        }

        // go back
        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var p = new Proyectos(usuario);
            p.Show();
        }

        //go to agregar colaborador
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var p = new AgregarColaborador(usuario, project);
            p.Show();
        }

        //go to agregar actividad
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var a = new AgregarActividad(usuario, project);
            a.Show();

        }

        //go to detalle actividad
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("There's no selected activity");
            }
            else
            {
                BDD bdd = new BDD();
                var activity = comboBox2.Text;
                var activityid = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
                var a = new DetalleActividad(project,usuario,activityid);
                this.Hide();
                a.Show();
            }
        }
    }
}
