namespace NewsAggregationFE.Core.Interfaces
{
    public interface IConsoleView
    {
        public void ShowMessages(string message);
        public string ReadInput(string prompt);
    }
}
