using Microsoft.Ajax.Utilities;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace CampaniasSB.Classes
{
    public static class IJSExtensions
    {
        public static async Task<object> GuardarComo(JSRuntime js, string nombreArchivo, byte[] archivo)
        {

            return await js.InvokeAsync<object>("saveAsFile", 
                nombreArchivo, 
                Convert.ToBase64String(archivo));
        }
    }
}