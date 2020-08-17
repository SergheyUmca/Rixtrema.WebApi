namespace Rixtrema.DAL.Models.Request
{
    public class PercentileCreateRequest
    {
        public string Type { get; set; }
        
        public double? Val { get; set; }
        
        public int? Perc { get; set; }
        
        public string BusinessCode { get; set; }
        
        public int? Bucket { get; set; }
        
        public string State { get; set; }
    }
}