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
    public partial class Proyectos : Form
    {
        int usuario;
        public Proyectos(int idUsuario)
        {
            this.usuario = idUsuario;
            InitializeComponent();
            loadData();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //CARGAR LOS PROYECTOS DEL USUARIO
        private void loadData()
        {
            BDD A = new BDD();
            var dic = A.SelectProjectsFromUser(this.usuario);
            label3.Text = A.GetNameAndUserName(this.usuario);
            if(dic.Count > 0)
            {
                comboBox1.DataSource = new BindingSource(dic, null);
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";
            }
            

            foreach(var a in dic)
            {
                ProjectsList.Items.Add(a.Value);
            }
            
        }

        //GO TO CREATE PROJECT
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var NewProject = new CreateProject(usuario);
            NewProject.Show();
        }


        // VER DETALLE PROYECTO
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
              //BDD B = new BDD();
                var idproyecto = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
                var detalle = new DetalleProyecto(usuario, idproyecto);
                this.Hide();
                detalle.Show();
                
            }
            else
            {
                MessageBox.Show("There is no project selected");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var a = new Form1();
            a.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
