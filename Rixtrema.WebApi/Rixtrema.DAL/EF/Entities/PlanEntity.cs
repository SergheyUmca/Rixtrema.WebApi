using System.ComponentModel.DataAnnotations.Schema;

namespace Rixtrema.DAL.EF.Entities
{
    [Table("tPlans", Schema = "dbo")]
    public class PlanEntity
    {
        [Column("ACK_ID")]
        public int Id { get; set; }
        
        [Column(TypeName = "real")]
        public float? Earnings { get; set; }
        
        [Column("PARTICIPANT_CONTRIB_AMT", TypeName = "real")]
        public float? ParticipantContribAmt { get; set; }
        
        [Column("EMPLR_CONTRIB_INCOME_AMT",TypeName = "real")]
        public float? EmplrContribIncomeAmt { get; set; }
        
        [Column(TypeName = "real")]
        public float? ParticswithBal { get; set; }
        
        [Column(TypeName = "real")]
        public float? ActPartics { get; set; }
        
        [Column(TypeName = "real")]
        public float? PartRate { get; set; }
        
        [Column(TypeName = "real")]
        public float? AvgPartContrib { get; set; }
        
        [Column(TypeName = "real")]
        public float? AvgEmpContrib { get; set; }
        
        [Column(TypeName = "real")]
        public float? Assets { get; set; }
        
        [Column(TypeName = "real")]
        public float? Adminexp { get; set; }
        
        [Column(TypeName = "real")]
        public float? AvgBalance { get; set; }
        
        [Column(TypeName = "real")]
        public float? PartContribRate { get; set; }
        
        [Column(TypeName = "real")]
        public float? EmpContribIncomeRate { get; set; }
        
        [Column(TypeName = "real")]
        public float? AdminExpRate { get; set; }
        
        [Column(TypeName = "real")]
        public float? PercRetirees { get; set; }
    
        [Column("SPONS_DFE_MAIL_STATE", TypeName = "varchar(2)")]
        public string SponsDfeMailState { get; set; }

        [Column("BUSINESS_CODE", TypeName = "varchar(50)")]
        public string BusinessCode { get; set; }
        
        public int Bucket { get; set; }
    }
}