// This file was auto-generated by ML.NET Model Builder.

using Openbanking_Investec_DashboardML.Model;
using System;

namespace Openbanking_Investec_DashboardML.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Col11 = 13F,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Col9 with predicted Col9 from sample data...\n\n");
            Console.WriteLine($"Col11: {sampleData.Col11}");
            Console.WriteLine($"\n\nPredicted Col9: {predictionResult.Score}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}