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
    public partial class UpdateProject : Form
    {
        int usuario;
        int project;
        string titulo;
        string description;
        public UpdateProject(int idusuario, int idproject)
        {
            this.usuario = idusuario;
            this.project = idproject;
            InitializeComponent();
            load();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var a = new DetalleProyecto(usuario, project);
            a.Show();
        }

        public void load()
        {
            var a = new BDD();
            titulo = a.GetProjectTitle(project);
            description = a.GetProjectDescription(project);
            label1.Text = titulo;
            textBox1.Text = titulo;
            textBox2.Text = description;
            textBox1.ForeColor = Color.Silver;
            textBox2.ForeColor = Color.Silver;
        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == titulo)
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = titulo;
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == description )
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = description;
                textBox2.ForeColor = Color.Silver;
            }
        }


        // UPDATE PROJECT
        private void button2_Click(object sender, EventArgs e)
        {
            BDD bd = new BDD();
            var edited = bd.UpdateProject(project, textBox1.Text, textBox2.Text);
            if (!edited)
            {
                MessageBox.Show("Ha ocurrido un error");
            }
            else
            {
                this.Hide();
                DetalleProyecto dp = new DetalleProyecto(usuario, project);
                dp.Show();
            }
        }
    }
}
