using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Text;
using System.Security.Cryptography;
using System.Web.UI.WebControls;

using AplicacionWebNominaISLinea.Models;



namespace AplicacionWebNominaISLinea.Controllers
{
    public class AccesoController : Controller
    {
            //Clase o entidad para conectamrema ala base de datos y objetos de la misma 
             
            //get: acceso

        public ActionResult Autenticar()
        {
            return View();
        }

        //post 

        [HttpPost]

        public ActionResult Autenticar(Empleado oEmpleado)

        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ValidarUsuarios", cn);
                    cmd.Parameters.AddWithValue("usuario", oEmpleado.correo);
                    cmd.Parameters.AddWithValue("clave", oEmpleado.clave);
                    cmd.Parameters.Add("id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.CommandType= CommandType.StoredProcedure;

                    cn.Open();  
                    cmd.ExecuteNonQuery();

                    oEmpleado.id = Convert.ToInt32(cmd.Parameters["id"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();   

                    cn.Close(); 
                }

                if (oEmpleado.id == 1 )

                {
                    //sesion ["usuario"] = oEmpleado.correo;
                    ViewData["mensaje"] = "Autenticacion Exitosa";
                    return RedirectToAction("About", "Home");
                }

                if (oEmpleado.id == 0 )
                {
                    ViewData["mensaje"] = "Usuario no encontrado";
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }

        }

    }
        
}

