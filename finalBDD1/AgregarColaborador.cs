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
    public partial class AgregarColaborador : Form
    {
        int idusuario, idproject;
        public AgregarColaborador(int idusuario, int idproject)
        {
            this.idusuario = idusuario;
            this.idproject = idproject;
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            BDD b = new BDD();
            label1.Text = b.GetProjectTitle(idproject);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BDD a = new BDD();
            if(textBox1.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("Debe escribir el nombre del usuario que desea agregar como colaborador");
                return;
            }

            var idColaborator = a.AddColaborator(textBox1.Text, idproject);
            if(idColaborator == 0)
            {
                MessageBox.Show("Usuario invalido: este usuario no existe o ya es colaborador en el proyecto");
            }
            else
            {
                this.Hide();
                var b = new DetalleProyecto(idusuario, idproject);
                b.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var a = new DetalleProyecto(idusuario, idproject);
            a.Show();
        }
    }
}
