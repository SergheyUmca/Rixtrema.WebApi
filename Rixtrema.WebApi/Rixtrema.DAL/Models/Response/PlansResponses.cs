namespace Rixtrema.DAL.Models.Response
{
    public class GetPlansResponse
    {
        public int Id { get; set; }
        
        public float? Earnings { get; set; }
        
        public float? ParticipantContribAmt { get; set; }
        
        public float? EmplrContribIncomeAmt { get; set; }
        
        public float? ParticswithBal { get; set; }
        
        public float? ActPartics { get; set; }
        
        public float? PartRate { get; set; }
        
        public float? AvgPartContrib { get; set; }
        
        public float? AvgEmpContrib { get; set; }
        
        public float? Assets { get; set; }
        
        public float? Adminexp { get; set; }
        
        public float? AvgBalance { get; set; }
        
        public float? PartContribRate { get; set; }
        
        public float? EmpContribIncomeRate { get; set; }
        
        public float? AdminExpRate { get; set; }
        
        public float? PercRetirees { get; set; }
        
        public string SponsDfeMailState { get; set; }
        
        public string BusinessCode { get; set; }
        
        public int Bucket { get; set; }
    }
}