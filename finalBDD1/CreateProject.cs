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
    public partial class CreateProject : Form
    {
        int usuario;
        public CreateProject(int idUsuario)
        {
            this.usuario = idUsuario;
            InitializeComponent();
        }

        //CREATE PROJECT
        private void button1_Click(object sender, EventArgs e)
        {
            BDD A = new BDD();
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "")
            {
                MessageBox.Show("Todos los campos son obligatorios");
            }
            else
            {
                var created = A.CreateProject(textBox1.Text, textBox2.Text, usuario);
                if (created)
                {
                    Proyectos p = new Proyectos(usuario);
                    this.Hide();
                    p.Show();
                }
                else
                    MessageBox.Show("Ha ocurrido un error! :-(");
            }
           
        }

        //GO BACK TO PROJECTS
        private void button2_Click(object sender, EventArgs e)
        {
            Proyectos p = new Proyectos(usuario);
            this.Hide();
            p.Show();
        }
    }
}
