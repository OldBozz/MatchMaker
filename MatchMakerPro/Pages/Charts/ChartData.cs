namespace MatchMakerPro.Pages.Charts
{
    public class ApexChartPoint
    {
        public string XValue { get; set; } = "";
        public decimal YValue{ get; set; }
        public decimal YValue2 { get; set; }

    }
    public class ApexChartSeries
    {
        public string Name { get; set; } = "";
        public List<ApexChartPoint> ChartPoints { get; set; } = new();
    }

}
