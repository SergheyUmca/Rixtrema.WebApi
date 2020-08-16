using System.ComponentModel.DataAnnotations.Schema;

namespace Rixtrema.DAL.EF.Entities
{
    [Table("tStates", Schema = "dbo")]
    public class StateEntity
    {
        public int Id { get; set; }
    
        [Column(TypeName = "varchar(2)")]
        public string Code { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }
    }
}