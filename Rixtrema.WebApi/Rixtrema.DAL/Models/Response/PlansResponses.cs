namespace Rixtrema.DAL.Models.Response
{
    public class GetPlansResponse
    {
        public int Id { get; set; }
        
        public double? Earnings { get; set; }
        
        public double? ParticipantContribAmt { get; set; }
        
        public double? EmplrContribIncomeAmt { get; set; }
        
        public double? ParticswithBal { get; set; }
        
        public double? ActPartics { get; set; }
        
        public double? PartRate { get; set; }
        
        public double? AvgPartContrib { get; set; }
        
        public double? AvgEmpContrib { get; set; }
        
        public double? Assets { get; set; }
        
        public double? Adminexp { get; set; }
        
        public double? AvgBalance { get; set; }
        
        public double? PartContribRate { get; set; }
        
        public double? EmpContribIncomeRate { get; set; }
        
        public double? AdminExpRate { get; set; }
        
        public double? PercRetirees { get; set; }
        
        public string SponsDfeMailState { get; set; }
        
        public string BusinessCode { get; set; }
        
        public int Bucket { get; set; }
    }
}