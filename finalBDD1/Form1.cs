using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace finalBDD1
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
       
        //LOG IN
        private void button2_Click(object sender, EventArgs e)
        {
            
            var A = new BDD();
            var yes = A.Authenticate(textBox1.Text, textBox2.Text);
            if (yes != 0)
            {
                this.Hide();
                Proyectos proyectos = new Proyectos(yes);
                proyectos.Show();
            }
            else
                MessageBox.Show("Invalid User or Password");
            
        }
       
        // Go to REGISTER PAGE
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

                this.Hide();
                var Register = new Register();
                Register.Show();

        }
    }




    public class BDD
    {
        private static string ConnectionStr = "data source=WINDOWS-90MR23V\\SQLEXPRESS; initial catalog = probando123; persist security info=True; Integrated Security = SSPI";
        private SqlConnection ConnectionA = new SqlConnection(ConnectionStr);

        //este metodo permite crear un nuevo usuario retorna true, si lo hizo exitosamente. False en caso contrario.
        public bool CreateUser(string Nombre, string Apellido, string Username, string Contrasenia, string FechaNacimiento, string Descripcion)
        {
            ConnectionA.Open();
            bool creado = true;
            try
            {
                SqlCommand Insert = new SqlCommand();
                Insert.Connection = ConnectionA;
                Insert.CommandText = "INSERT INTO Usuario(NombreUsuario, ApellidoUsuario, Username, Contrasenia, FechaNacimiento, DescripcionUsuario)  " +
                    " VALUES(@param1,@param2,@param3,@param4,@param5, @param6)";
                Insert.Parameters.AddWithValue("@param1", Nombre);
                Insert.Parameters.AddWithValue("@param2", Apellido);
                Insert.Parameters.AddWithValue("@param3", Username);
                Insert.Parameters.AddWithValue("@param4", Contrasenia);
                Insert.Parameters.AddWithValue("@param5", FechaNacimiento);
                Insert.Parameters.AddWithValue("@param6", Descripcion);

                Insert.ExecuteNonQuery();
            }
            catch (Exception)
            {
                creado = false;
            }
            finally
            {
                ConnectionA.Close();
            }

            return creado;

        }





        //Este metodo permite editar uno de los campos del usuario
        //0: nombre, 1: apellido, 2: username, 3: contrasennia, 4 o mayor: descripcion
        public bool UpdateUser(int id, int campo, string value)
        {
            bool Updated = true;
            string qry;

            switch (campo)
            {
                case 0:
                    qry = "UPDATE Usuario SET NombreUsuario = @param2 WHERE idUsuario = @param3 ";
                    break;
                case 1:
                    qry = "UPDATE Usuario SET ApellidoUsuario = @param2 WHERE idUsuario = @param3 ";
                    break;
                case 2:
                    qry = "UPDATE Usuario SET Username = @param2 WHERE idUsuario = @param3 ";
                    break;
                case 3:
                    qry = "UPDATE Usuario SET Contrasenia = @param2 WHERE idUsuario = @param3 ";
                    break;
                default:
                    qry = "UPDATE Usuario SET DescripcionUsuario = @param2 WHERE idUsuario = @param3 ";
                    break;

            }

            ConnectionA.Open();
            try
            {
                SqlCommand Update = new SqlCommand();
                Update.Connection = ConnectionA;
                Update.CommandText = qry;
                Update.Parameters.AddWithValue("@param2", value);
                Update.Parameters.AddWithValue("@param3", id);
                Update.ExecuteNonQuery();
            }
            catch (Exception)
            {
                Updated = false;
            }
            finally
            {
                ConnectionA.Close();
            }

            return Updated;
        }







        // Este metodo retorna el id del usuario, si este se autentica exitosamente, de lo contrario, retorna 0
        public int Authenticate(string username, string password)
        {
            int correct;
            ConnectionA.Open();
            try
            {

                SqlCommand commandSelect = new SqlCommand();
                commandSelect.Connection = ConnectionA;
                commandSelect.CommandText = "SELECT idUsuario FROM [Usuario] WHERE [Username]=@param1 AND [Contrasenia] = @param2";
                commandSelect.Parameters.AddWithValue("@param1", username);
                commandSelect.Parameters.AddWithValue("@param2", password);
                var complejidad = commandSelect.ExecuteNonQuery();
                var rdr = commandSelect.ExecuteReader();
                rdr.Read();
                correct = rdr.GetInt32(0);
                rdr.Close();

            }
            catch (Exception)
            {
                correct = 0;
            }
            finally
            {
                ConnectionA.Close();
            }
            return correct;

        }

        //deben existir el usuario
        public bool CreateProject(string title, string desc, int idUsuario)
        {
            bool userAdded = true;
            bool created = true;
            ConnectionA.Open();
            try
            {
                SqlCommand Insert1 = new SqlCommand();
                Insert1.Connection = ConnectionA;
                Insert1.CommandText = "INSERT INTO Proyecto(Titulo, DescripcionProyecto)  " +
                    " VALUES(@param1,@param2)";
                Insert1.Parameters.AddWithValue("@param1", title);
                Insert1.Parameters.AddWithValue("@param2", desc);

                Insert1.ExecuteNonQuery();
            }
            catch (Exception)
            {
                created = false;
            }
            finally
            {
                ConnectionA.Close();
            }


            ConnectionA.Open();
            try
            {
                SqlCommand commandSelect = new SqlCommand();
                commandSelect.Connection = ConnectionA;
                commandSelect.CommandText = "SELECT MAX(idProyecto) FROM [Proyecto]";
                var complejidad = commandSelect.ExecuteNonQuery();
                var rdr = commandSelect.ExecuteReader();
                rdr.Read();
                var correct = rdr.GetInt32(0);
                rdr.Close();

                SqlCommand Insert = new SqlCommand();
                Insert.Connection = ConnectionA;
                Insert.CommandText = "INSERT INTO ProyectoUsuario(idUsuario, idProyecto)  " +
                    " VALUES(@param1,@param2)";
                Insert.Parameters.AddWithValue("@param1", idUsuario);
                Insert.Parameters.AddWithValue("@param2", correct);

                Insert.ExecuteNonQuery();
            }
            catch (Exception)
            {
                userAdded = false;
            }
            finally
            {
                ConnectionA.Close();
            }

            if (created && userAdded)
                return true;
            else return false;

        }

        //falta validar que el usuario sea colaborador de ese proyecto
        public bool AgregarActividad(int idProyecto, int idUsuario, string title, string description, int idComplejidad, int status)
        {
            bool created = true;
            ConnectionA.Open();
            try
            {
                SqlCommand Insert = new SqlCommand();
                Insert.Connection = ConnectionA;
                Insert.CommandText = "INSERT INTO Actividad(idProyecto , idUsuario, idComplejidad, TituloActividad, DescripcionActividad, StatusActividad)  " +
                    " VALUES(@param1,@param2,@param3,@param4,@param5,@param7)";
                Insert.Parameters.AddWithValue("@param1", idProyecto);
                Insert.Parameters.AddWithValue("@param2", idUsuario);
                Insert.Parameters.AddWithValue("@param3", idComplejidad);
                Insert.Parameters.AddWithValue("@param4", title);
                Insert.Parameters.AddWithValue("@param5", description);
                Insert.Parameters.AddWithValue("@param7", status);

                Insert.ExecuteNonQuery();
            }
            catch
            {
                created = false;
            }
            finally
            {
                ConnectionA.Close();
            }
            return created;
        }

        //falta validar que el usuario no sea colaborador 
        public bool AgregarColaborador(int idUsuario, int idProyecto)
        {
            bool added = true;

            ConnectionA.Open();

            try
            {
                SqlCommand Insert = new SqlCommand();
                Insert.Connection = ConnectionA;
                Insert.CommandText = "INSERT INTO ProyectoUsuario(idUsuario, idProyecto)  " +
                    " VALUES(@param1,@param2)";
                Insert.Parameters.AddWithValue("@param1", idUsuario);
                Insert.Parameters.AddWithValue("@param2", idProyecto);

                Insert.ExecuteNonQuery();
            }
            catch (Exception)
            {
                added = false;
            }
            finally
            {
                ConnectionA.Close();
            }
            return added;
        }

        //Seleccionar los proyectos del usuario 
        public Dictionary<int, string> SelectProjectsFromUser(int idUsuario )
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            ConnectionA.Open();
           
            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = "SELECT PU.idProyecto, Titulo FROM ProyectoUsuario AS PU INNER JOIN Proyecto AS P on P.idProyecto = PU.idProyecto WHERE idUsuario = @param1";
            commandSelect.Parameters.AddWithValue("@param1", idUsuario);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            while (rdr.Read())
            {
              //list.Add(rdr["Titulo"].ToString());
                dic.Add(rdr.GetInt32(0), rdr.GetString(1));
                    //(rdr["Titulo"].ToString());

            }
            rdr.Close();
            ConnectionA.Close();
            return dic;
            
        }

        // get actividades done
        public Dictionary<int, string> SelectActividadDone(int idProyecto )
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = "SELECT idActividad, TituloActividad FROM Actividad WHERE idProyecto = @param1 AND StatusActividad = 1";
            commandSelect.Parameters.AddWithValue("@param1", idProyecto);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            while (rdr.Read())
            {
                dic.Add(rdr.GetInt32(0), rdr.GetString(1));
            }
            rdr.Close();
            ConnectionA.Close();

            return dic;
        }

        // get actividades not done
        public Dictionary<int, string> SelectActividadesNotDone(int idProyecto)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = "SELECT idActividad, TituloActividad FROM Actividad WHERE idProyecto = @param1 AND StatusActividad = 0";
            commandSelect.Parameters.AddWithValue("@param1", idProyecto);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            while (rdr.Read())
            {
                dic.Add(rdr.GetInt32(0), rdr.GetString(1));
            }
            rdr.Close();
            ConnectionA.Close();

            return dic;

        }

        //get project description 
        public string GetProjectDescription(int idProject)
        {
            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = " SELECT DescripcionProyecto FROM Proyecto WHERE idProyecto = @param1";
            commandSelect.Parameters.AddWithValue("@param1", idProject);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();

            string a =rdr.GetString(0);
            rdr.Close();
            ConnectionA.Close();
            return a;
        }

        // get list of colaborators on this project
        public Dictionary<int, string> GetProjectColaborators(int idproyecto)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = "  SELECT PU.idUsuario, NombreUsuario FROM ProyectoUsuario AS PU INNER JOIN Usuario AS U ON U.idUsuario = PU.idUsuario WHERE idProyecto = @param1 ";
            commandSelect.Parameters.AddWithValue("@param1", idproyecto);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            while (rdr.Read())
            {
                dic.Add(rdr.GetInt32(0), rdr.GetString(1));
            }
            rdr.Close();
            ConnectionA.Close();

            return dic;
        }

        // get project title
        public string GetProjectTitle(int idProject)
        {
            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = " SELECT Titulo FROM Proyecto WHERE idProyecto = @param1";
            commandSelect.Parameters.AddWithValue("@param1", idProject);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();
            string a = rdr.GetString(0);
            rdr.Close();
            ConnectionA.Close();
            return a;

        }

        //get complexity
        public Dictionary<int,string> GetComplexities()
        {
            var dic = new Dictionary<int, string>();
            var qry = "SELECT idComplejidad, Complejidad FROM Complejidad";

            ConnectionA.Open();
            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();

            while (rdr.Read())
            {
                dic.Add(rdr.GetInt32(0), rdr.GetString(1));
            }
            rdr.Close();
            ConnectionA.Close();
            return dic;

        }

        //Update project titulo y descripcion
        public bool UpdateProject(int idProject, string titulo, string desc)
        {
            var qry = "UPDATE Proyecto SET Titulo = @param1 , DescripcionProyecto = @param3 WHERE idProyecto = @param2";
            bool updated = true;

            ConnectionA.Open();
            try
            {
                SqlCommand Update = new SqlCommand();
                Update.Connection = ConnectionA;
                Update.CommandText = qry;
                Update.Parameters.AddWithValue("@param1", titulo);
                Update.Parameters.AddWithValue("@param2", idProject);
                Update.Parameters.AddWithValue("@param3", desc);
                Update.ExecuteNonQuery();
        }
            catch (Exception)
            {
                updated = false;
            }
            finally
            {
                ConnectionA.Close();
            }

            return updated;

        }

        // Agregar colaborador a un proyecto
        public int AddColaborator(String username, int proyecto)
        {
            int idColaborator= 0;
            bool add = false;
            var qry = "SELECT idUsuario FROM Usuario WHERE Username = @param1";
            string qry2 = "SELECT * FROM ProyectoUsuario WHERE idUsuario = @param1 AND idProyecto = @param2";
            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;
            commandSelect.Parameters.AddWithValue("@param1", username);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            if (rdr.Read())
                idColaborator = rdr.GetInt32(0);
            rdr.Close();
            ConnectionA.Close();

            //si me devuelve 0, el usuario no existe 

            if(idColaborator != 0)
            {
                ConnectionA.Open();

                SqlCommand commandSelec = new SqlCommand();
                commandSelec.Connection = ConnectionA;
                commandSelec.CommandText = qry2;
                commandSelec.Parameters.AddWithValue("@param1", idColaborator);
                commandSelec.Parameters.AddWithValue("@param2", proyecto );

                commandSelec.ExecuteNonQuery();
                var rdr1 = commandSelec.ExecuteReader();
                if (!rdr1.Read())
                {
                    add = true;
                }
                else
                {
                    idColaborator = 0;
                }
                rdr1.Close();
                ConnectionA.Close();


                if (add)
                {
                    ConnectionA.Open();

                    SqlCommand AddColab = new SqlCommand();
                    AddColab.Connection = ConnectionA;
                    AddColab.CommandText = "INSERT INTO ProyectoUsuario(idUsuario, idProyecto) VALUES (@param1,@param2)";
                    AddColab.Parameters.AddWithValue("@param1", idColaborator);
                    AddColab.Parameters.AddWithValue("@param2", proyecto);
                    AddColab.ExecuteNonQuery();

                }


            }
            return idColaborator;


        }

        // Seleccionar titulo de una actividad
        public string GetActivityTitle(int idActividad)
        {
            var qry = "SELECT TituloActividad FROM Actividad WHERE idActividad = @param1";

            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;
            commandSelect.Parameters.AddWithValue("@param1", idActividad);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();
            string a = rdr.GetString(0);
            rdr.Close();
            ConnectionA.Close();
            return a;

        }


        //seleccionar descripccion de una actividad
        public string GetActivityDescription(int idActividad)
        {
            var qry = "SELECT DescripcionActividad FROM Actividad WHERE idActividad = @param1";

            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;
            commandSelect.Parameters.AddWithValue("@param1", idActividad);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();
            string a = rdr.GetString(0);
            rdr.Close();
            ConnectionA.Close();
            return a;
        }

        //Seleccionar titulo complejidad de una actividad 
        public string SelectComplexityTitle(int idActividad)
        {
            var qry = "SELECT Complejidad FROM Complejidad AS C INNER JOIN Actividad AS A ON A.idComplejidad = C.idComplejidad WHERE idActividad = @param1";

            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;
            commandSelect.Parameters.AddWithValue("@param1", idActividad);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();
            string a = rdr.GetString(0);
            rdr.Close();
            ConnectionA.Close();
            return a;
        }

        // seleccionar nombre usuario encargado
        public string GetActivityUser(int idActividad)
        {
            var qry = "SELECT Username, NombreUsuario FROM Usuario AS U INNER JOIN Actividad AS A ON A.idUsuario = U.idUsuario WHERE idActividad = @param1";

            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;
            commandSelect.Parameters.AddWithValue("@param1", idActividad);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();
            string a = rdr.GetString(0) + " - " + rdr.GetString(1);
            rdr.Close();
            ConnectionA.Close();
            return a;
        }

        //get status value
        public bool GetStatusActivity(int idActividad)
        {
            var qry = "SELECT StatusActividad FROM Actividad WHERE idActividad = @param1";

            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;
            commandSelect.Parameters.AddWithValue("@param1", idActividad);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();
            var a = rdr.GetBoolean(0);
            rdr.Close();
            ConnectionA.Close();
            return a;
        }

        //get id de usuario encargado de tarea
        public int GetidUsuarioEncargado(int actividad)
        {
            var qry = "SELECT idUsuario FROM Actividad WHERE idActividad = @param1";

            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;
            commandSelect.Parameters.AddWithValue("@param1", actividad);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();
            var a = rdr.GetInt32(0);
            rdr.Close();
            ConnectionA.Close();
            return a;
        }


        //Update activity status
        public bool UpdateActivityStatus(int actividad, int status)
        {
            var qry = "UPDATE Actividad SET StatusActividad = @param1  WHERE idActividad = @param2";
            bool updated = true;

            ConnectionA.Open();
            try
            {
                SqlCommand Update = new SqlCommand();
                Update.Connection = ConnectionA;
                Update.CommandText = qry;
                Update.Parameters.AddWithValue("@param1", status);
                Update.Parameters.AddWithValue("@param2", actividad);
                Update.ExecuteNonQuery();
            }
            catch (Exception)
            {
                updated = false;
            }
            finally
            {
                ConnectionA.Close();
            }

            return updated;

        }


        //get user by id
        public string GetNameAndUserName(int usuario)
        {
            var qry = "SELECT Username, NombreUsuario FROM Usuario WHERE idUsuario = @param1";

            ConnectionA.Open();

            SqlCommand commandSelect = new SqlCommand();
            commandSelect.Connection = ConnectionA;
            commandSelect.CommandText = qry;
            commandSelect.Parameters.AddWithValue("@param1", usuario);

            var complejidad = commandSelect.ExecuteNonQuery();
            var rdr = commandSelect.ExecuteReader();
            rdr.Read();
            string a = rdr.GetString(0) + " - " + rdr.GetString(1);
            rdr.Close();
            ConnectionA.Close();
            return a;
        }



    }




}
