using EquiposTecnicosSN.Entities.Mantenimiento;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos.Info
{
    [Table("InformacionComercial")]
    public class InformacionComercial
    {
        [Key]
        [ForeignKey("Equipo")]
        [Required]
        public int EquipoId { get; set; }

        public virtual Equipo Equipo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull =true)]
        [DisplayName("Fecha de compra")]
        public DateTime? FechaCompra { get; set; }

        [DisplayName("Precio de compra")]
        public decimal? PrecioCompra { get; set; }

        [DisplayName("Valor restante")]
        public decimal? ValorRestante { get; set; }

        [DisplayName("Garantía/Contrato")]
        public GarantiaContrato? EsGrantiaContrato { get;set; }

        [DisplayName("Fecha de fin de garantía")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public DateTime? FechaFinGarantia { get; set; }

        [DisplayName("Notas de garantía")]
        [StringLength(150)]
        public string NotasGarantia { get; set; }

        public int? ProveedorId { get; set; }

        [DisplayName("Proveedor")]
        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        public Financiamiento? Financiamiento { get; set; }
    }
}

