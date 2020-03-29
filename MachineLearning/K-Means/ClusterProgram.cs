using System;

namespace K_Means
{
    class ClusterProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nBegin k-means clustering demo\n");

            double[][] rawData = new double[10][];
            rawData[0] = new double[] { 73, 72.6 };
            rawData[1] = new double[] { 61, 54.4 };
            // etc.
            rawData[9] = new double[] { 61, 59.0 };

            Console.WriteLine("Raw unclustered data:\n");
            Console.WriteLine(" ID Height (in.) Weight (kg.)");
            Console.WriteLine("---------------------------------");
            ShowData(rawData, 1, true, true);

            int numClusters = 3;
            Console.WriteLine("\nSetting numClusters to " + numClusters);

            Console.WriteLine("\nStarting clustering using k-means algorithm");
            Clusterer c = new Clusterer(numClusters);
            int[] clustering = c.Cluster(rawData);
            Console.WriteLine("Clustering complete\n");

            Console.WriteLine("Final clustering in internal form:\n");
            ShowVector(clustering, true);

            Console.WriteLine("Raw data by cluster:\n");
            ShowClustered(rawData, clustering, numClusters, 1);

            Console.WriteLine("\nEnd k-means clustering demo\n");
            Console.ReadLine();
        }

        static void ShowData(double[][] data, int decimals, bool indices, bool newLine)
        {
        }

        static void ShowVector(int[] vector, bool newLine)
        {
        }

        static void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals)
        {
        }
    }

    public class Clusterer
    {
        public Clusterer(int numClusters)
        {
        }

        public int[] Cluster(double[][] data)
        {
            throw new NotImplementedException();
        }
    }
}
