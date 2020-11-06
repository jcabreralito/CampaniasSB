using CampaniasSB.Models;
using System;

namespace CampaniasSB.Classes
{
    public class DBHelper
    {
        public static Response SaveChanges(CampaniasContext db)
        {
            try
            {
                db.SaveChanges();
                return new Response { Succeeded = true, };
            }
            catch (Exception ex)
            {
                var response = new Response { Succeeded = false, };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "Registro Duplicado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "No se puede eliminar el registro, existen movimientos relacionados";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("IX_"))
                {
                    response.Message = "Registro Duplicado";
                }
                else
                {
                    response.Message = ex.Message;
                }

                return response;
            }
        }
    }
}