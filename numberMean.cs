using System;
using System.Numerics;
class MyClass
{
    static void Main(string[] args)
    {
        var userInput = SetUserInputInArray();
        int totalQueries = userInput[1];
        var numbers = SetUserInputInArray();
        long[] prefixSum = GeneratePrefixSum(numbers);
        LogMeanForQueries(totalQueries,prefixSum);
    }
   
    static long[] GeneratePrefixSum(int[] numbers)
    {
        long[] prefixSum = new long[numbers.Length + 1];
        prefixSum[0] = 0;
        for (int i = 1; i <= numbers.Length; i++)
        {
            prefixSum[i] = prefixSum[i - 1] + numbers[i - 1];
        }
        return prefixSum;
    }
    static void LogMeanForQueries(int totalQueries, long[] prefixSum)
    {
        for (var x = 0; x < totalQueries; x++)
        {
            var query = SetUserInputInArray();
            long mean = (long)((long)(prefixSum[query[1]] - prefixSum[query[0] - 1]) / (query[1] - query[0] + 1));
            Console.WriteLine(mean);
        }
    }
    static int[] SetUserInputInArray()
    {
        return Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
    }
}
