namespace Main
{

    public static class UserInput
    {
        public static string GetBlogName()
        {
            Console.WriteLine("enter the Tumblr blog name:  ");
            string input = Console.ReadLine();
            return input;
        }
        public static (int start, int end) GetRange()
        {
            Console.WriteLine("enter the range:  ");
            string input = Console.ReadLine();
            string[] parseInput = input.Split("-");
            int startRange = Int32.Parse(parseInput[0]);
            int endRange = Int32.Parse(parseInput[1]);
            return (startRange, endRange);
        }
    }
}