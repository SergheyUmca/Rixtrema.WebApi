using System.ComponentModel.DataAnnotations.Schema;

namespace Rixtrema.DAL.EF.Entities
{
    [Table("tPlans", Schema = "dbo")]
    public class PlanEntity
    {
        [Column("ACK_ID")]
        public int Id { get; set; }
        
        [Column(TypeName = "real")]
        public double? Earnings { get; set; }
        
        [Column("PARTICIPANT_CONTRIB_AMT", TypeName = "real")]
        public double? ParticipantContribAmt { get; set; }
        
        [Column("EMPLR_CONTRIB_INCOME_AMT",TypeName = "real")]
        public double? EmplrContribIncomeAmt { get; set; }
        
        [Column(TypeName = "real")]
        public double? ParticswithBal { get; set; }
        
        [Column(TypeName = "real")]
        public double? ActPartics { get; set; }
        
        [Column(TypeName = "real")]
        public double? PartRate { get; set; }
        
        [Column(TypeName = "real")]
        public double? AvgPartContrib { get; set; }
        
        [Column(TypeName = "real")]
        public double? AvgEmpContrib { get; set; }
        
        [Column(TypeName = "real")]
        public double? Assets { get; set; }
        
        [Column(TypeName = "real")]
        public double? Adminexp { get; set; }
        
        [Column(TypeName = "real")]
        public double? AvgBalance { get; set; }
        
        [Column(TypeName = "real")]
        public double? PartContribRate { get; set; }
        
        [Column(TypeName = "real")]
        public double? EmpContribIncomeRate { get; set; }
        
        [Column(TypeName = "real")]
        public double? AdminExpRate { get; set; }
        
        [Column(TypeName = "real")]
        public double? PercRetirees { get; set; }
    
        [Column("SPONS_DFE_MAIL_STATE", TypeName = "varchar(2)")]
        public string SponsDfeMailState { get; set; }

        [Column("BUSINESS_CODE", TypeName = "varchar(50)")]
        public string BusinessCode { get; set; }
        
        public int Bucket { get; set; }
    }
}