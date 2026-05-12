using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProyectoFinalConsole
{
     internal class Program
    {
        static string cadenaDeConexion = string.Empty;
        static SqlConnection conexion = null;
        static SqlCommand mysqlCommand = null;
        static SqlDataReader mysqlDataReader = null;

        static void Main(string[] args)
        {
            ConectarASQLServer();
            Console.WriteLine("Bienvenido a la Agenda Personal");
            int opcion;
            do
            {
                Console.WriteLine("MENU DE OPCIONES");
                Console.WriteLine(
                    "\n 1. Insertar un Conctaco" +
                    "\n 2. Eliminar un Contacto" +
                    "\n 3. Listar contactos" +
                    "\n 4. Actualizar un Contacto" +
                    "\n 5. Salir \n");
                Console.Write("Ingrese una opcion: ");
                opcion = Convert.ToInt32(Console.ReadLine());
                switch (opcion)
                {
                    case 1:
                        InsertarContacto();
                        break;
                    case 2:
                    //EliminarContacto(); 
                    case 3:
                        MostrarDatosdeContactos();
                        break;
                    case 4:
                        //ActualizarContacto(); aqui va el metodo para actualizar un contacto quitan el comentario y ponen el metodo que hicieron
                        break;
                    case 5:
                        Console.WriteLine("Saliendo...");
                        CerrarConexion();
                        break;
                    default:
                        Console.WriteLine("Digite una opcion valida");
                        break;
                }

            } while (opcion != 5);
        }
        private static void ConectarASQLServer()
        {
            try
            {  // aqui cambian el nombre del servidor y el nombre de la base de datos solo eso,yo lo probe con mi servidor y mi base de datos pa ver si sirve
                // solo cambian donde dice server y database por el nombre de su servidor y el nombre de su base de datos que crearon
                cadenaDeConexion = "Server=DESKTOP-KFU0176;Database=AgendaPersonal;Trusted_Connection=True;";
                conexion = new SqlConnection(cadenaDeConexion);
                conexion.Open();
                Console.WriteLine("Conexión exitosa a SQL Server");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problema al Tratar de conectar a DB. Detalles: ");
                Console.WriteLine(ex.Message);
            }
        }
        private static void MostrarDatosdeContactos()
        {
            try
            {
                string sqlQuery = "Select * FROM Contactos";
                mysqlCommand = new SqlCommand(sqlQuery, conexion);
                mysqlDataReader = mysqlCommand.ExecuteReader();
                Console.WriteLine("ContactoID\t\tNombre\t\tTelefono\t\tEmail\t\tDireccion");
                Console.WriteLine("-------------------------------------------------------------");
                while (mysqlDataReader.Read())
                {
                    Console.WriteLine($"{mysqlDataReader["ContactoID"]}\t\t\t{mysqlDataReader["Nombre"]}\t\t\t{mysqlDataReader["Telefono"]}\t\t\t{mysqlDataReader["Email"]}\t\t\t{mysqlDataReader["Direccion"]}");
                }
                mysqlDataReader.Close();

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problema al Tratar de conectar a DB. Detalles: ");
                Console.WriteLine(ex.Message);

            }
        }
        private static void InsertarContacto()
        {
            try
            {
                Console.Write("Ingrese nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Ingrese teléfono: ");
                string telefono = Console.ReadLine();

                Console.Write("Ingrese email: ");
                string email = Console.ReadLine();

                Console.Write("Ingrese dirección: ");
                string direccion = Console.ReadLine();

                string sqlQuery = "INSERT INTO Contactos (Nombre, Telefono, Email, Direccion) VALUES (@Nombre, @Telefono, @Email, @Direccion)";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conexion))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Direccion", direccion);

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{rows} contacto insertado correctamente.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al insertar contacto: " + ex.Message);
            }
        }
        private static void CerrarConexion()
        {
            try
            {
                conexion.Close();
            }
            catch (SqlException ex)
            {

                Console.WriteLine("Problema al Tratar de conectar a DB. Detalles: ");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
