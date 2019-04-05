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
    public partial class DetalleActividad : Form
    {
        int project, usuario, actividad;
        bool statusActividad;

        public DetalleActividad(int project, int usuario, int actividad)
        {
            this.usuario = usuario;
            this.project = project;
            this.actividad = actividad;
            InitializeComponent();
            load();
        }

        public void load()
        {
            BDD b = new BDD();
            label13.Text = b.GetProjectTitle(project); // titulo proyecto
            label4.Text = b.GetActivityTitle(actividad); // titulo actividad
            label6.Text = b.GetActivityDescription(actividad); // Descripcion actividad
            label8.Text = b.SelectComplexityTitle(actividad); // complejidad actividad
            label11.Text = b.GetActivityUser(actividad); // username y nombre de usuario encargado
            var status = b.GetStatusActivity(actividad); // status actual
            statusActividad = status;
            if (!status)
                label10.Text = "Actividad Pendiente";
            else
                label10.Text = "Actividad Completada";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BDD b = new BDD();
            var UsuarioEncargado = b.GetidUsuarioEncargado(actividad);
            if ( usuario != UsuarioEncargado)
            {
                MessageBox.Show("Solo el usuario encargado de la actividad puede cambiar el status de esta");
            }
            else
            {
                int Newstate;
                if (statusActividad)
                    Newstate = 0;
                else Newstate = 1;

                b.UpdateActivityStatus(actividad, Newstate);

                if (statusActividad)
                    label10.Text = "Actividad Pendiente";
                else label10.Text = "Actividad Completada";
            }

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var a = new DetalleProyecto(usuario, project);
            a.Show();
        }
    }
}
