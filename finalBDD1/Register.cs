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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


        //REGISTER
        private void button1_Click(object sender, EventArgs e)
        {

            if(textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "" || textBox3.Text == null || textBox3.Text == "" || textBox5.Text == null || textBox5.Text == "" || textBox6.Text == null || textBox6.Text == "" || textBox7.Text == null || textBox7.Text == "")
            {
                MessageBox.Show("Todos los campos son obligatorios");
                return;
            }


            var A = new BDD();

            if (textBox7.Text != textBox6.Text)
            {
                MessageBox.Show("Passwords doesn't match");
            }
            else
            {
                var fecha = dateTimePicker1.Value.Date.ToString();


                var created = A.CreateUser(textBox1.Text, textBox2.Text, textBox3.Text, textBox6.Text, fecha, textBox5.Text);
                if (created)
                {
                    this.Hide();
                    var login = new Form1();
                    login.Show();
                }
                else
                    MessageBox.Show("Username already exists");
            }

        }

        //BACK BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var login = new Form1();
            login.Show();
        }
    }
}
