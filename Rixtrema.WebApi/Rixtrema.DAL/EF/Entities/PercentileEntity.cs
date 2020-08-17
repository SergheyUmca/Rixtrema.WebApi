using System.ComponentModel.DataAnnotations.Schema;

namespace Rixtrema.DAL.EF.Entities
{
    [Table("tPercentile", Schema = "dbo")]
    public class PercentileEntity
    {
        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; }
    
        [Column(TypeName = "real")]
        public double? Val { get; set; }
        
        public int? Perc { get; set; }
        
        [Column("BUSINESS_CODE", TypeName = "varchar(50)")]
        public string BusinessCode { get; set; }
        
        public int? Bucket { get; set; }
        
        [Column(TypeName = "varchar(2)")]
        public string State { get; set; }
    }
}