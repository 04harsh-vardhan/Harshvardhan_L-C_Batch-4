using NewsAggregationFE.Core.Interfaces;

namespace NewsAggregationFE.Presentation
{
    internal class ConsoleView : IConsoleView
    {
        public void ShowMessages(string message)
        {
            Console.WriteLine(message);
        }
        public string ReadInput(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? "";
        }
    }
}
