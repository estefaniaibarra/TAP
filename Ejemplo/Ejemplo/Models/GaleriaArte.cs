using System;
using System.Collections.Generic;
using System.Text;

namespace Ejemplo.Models
{
    public class GaleriaArte
    {
        public string Nombre { get; set; }

        public string Autor { get; set; }

        public string Imagen { get; set; } = "";

        public double Precio { get; set; }
    }
}
