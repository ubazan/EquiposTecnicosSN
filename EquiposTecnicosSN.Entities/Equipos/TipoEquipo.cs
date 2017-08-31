using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquiposTecnicosSN.Entities.Equipos
{
    public enum TipoEquipo
    {
        [Display(Name = "Climatización")]
        Climatizacion = 1,
        [Display(Name = "Cirugía")]
        Cirugia = 2,
        [Display(Name = "Endoscopía")]
        Endoscopia = 3,
        [Display(Name = "Edilicios")]
        Edilicio = 4,
        [Display(Name = "Soporte de Vida")]
        SoporteDeVida = 5,
        [Display(Name = "Gases Medicinales")]
        GasesMedicinales = 6,
        [Display(Name = "Imágenes")]
        Imagenes = 7,
        [Display(Name = "Luces")]
        Luces = 8,
        [Display(Name = "Monitoreo")]
        Monitoreo = 9,
        [Display(Name = "Informática")]
        Informatica = 10,
        [Display(Name = "Odontología")]
        Odontologia = 11,
        [Display(Name = "Pruebas de Diagnóstico")]
        PruebasDeDiagnostico = 12,
        [Display(Name = "Rehabilitacion")]
        Rehabilitacion = 13,
        [Display(Name = "Terapéutica")]
        Terapeutica = 14,
        [Display(Name = "Otros")]
        Otros = 15
    }
}
