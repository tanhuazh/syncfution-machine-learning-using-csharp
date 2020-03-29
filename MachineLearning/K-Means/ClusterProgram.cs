using System;

namespace K_Means
{
    class ClusterProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nBegin k-means clustering demo\n");

            double[][] rawData = new double[10][];
            rawData[0] = new double[] { 73.0, 72.6 };
            rawData[1] = new double[] { 61.0, 54.4 };
            rawData[2] = new double[] { 67.0, 99.9 };
            rawData[3] = new double[] { 68.0, 97.3 };
            rawData[4] = new double[] { 62.0, 59.0 };
            rawData[5] = new double[] { 75.0, 81.6 };
            rawData[6] = new double[] { 74.0, 77.1 };
            rawData[7] = new double[] { 66.0, 97.3 };
            rawData[8] = new double[] { 68.0, 93.3 };
            rawData[9] = new double[] { 61.0, 59.0 };

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
            for (int i = 0; i < data.Length; ++i)
            {
                if (indices == true)
                    Console.Write(i.ToString().PadLeft(3) + " ");
                for (int j = 0; j < data[i].Length; ++j)
                {
                    double v = data[i][j];
                    Console.Write(v.ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine == true)
                Console.WriteLine("");
        }

        static void ShowVector(int[] vector, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
                Console.Write(vector[i] + " ");
            if (newLine == true) Console.WriteLine("\n");
        }

        static void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals)
        {
            for (int k = 0; k < numClusters; ++k)
            {
                Console.WriteLine("===================");
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = clustering[i];
                    if (clusterID != k) continue;
                    Console.Write(i.ToString().PadLeft(3) + " ");
                    for (int j = 0; j < data[i].Length; ++j)
                    {
                        double v = data[i][j];
                        Console.Write(v.ToString("F" + decimals) + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("===================");
            } // k
        }

        static double[][] LoadData(string dataFile, int numRows, int numCols, char delimit)
        {
            System.IO.FileStream ifs = new System.IO.FileStream(dataFile, System.IO.FileMode.Open);
            System.IO.StreamReader sr = new System.IO.StreamReader(ifs);
            string line = "";
            string[] tokens = null;
            int i = 0;
            double[][] result = new double[numRows][];
            while ((line = sr.ReadLine()) != null)
            {
                result[i] = new double[numCols];
                tokens = line.Split(delimit);
                for (int j = 0; j < numCols; ++j)
                    result[i][j] = double.Parse(tokens[j]);
                ++i;
            }
            sr.Close();
            ifs.Close();
            return result;
        }
    }

    public class Clusterer
    {
        private int numClusters;
        private int[] clustering;
        private double[][] centroids;
        private Random rnd;

        public Clusterer(int numClusters)
        {
            this.numClusters = numClusters;
            this.centroids = new double[numClusters][];
            this.rnd = new Random(0);
        }

        public int[] Cluster(double[][] data)
        {
            int numTuples = data.Length;
            int numValues = data[0].Length;
            this.clustering = new int[numTuples];

            for (int k = 0; k < numClusters; ++k)
                this.centroids[k] = new double[numValues];

            InitRandom(data);

            Console.WriteLine("\nInitial random clustering:");
            for (int i = 0; i < clustering.Length; ++i)
                Console.Write(clustering[i] + " ");
            Console.WriteLine("\n");

            bool changed = true; // change in clustering?
            int maxCount = numTuples * 10; // sanity check
            int ct = 0;
            while (changed == true && ct < maxCount)
            {
                ++ct;
                UpdateCentroids(data);
                changed = UpdateClustering(data);
            }

            int[] result = new int[numTuples];
            Array.Copy(this.clustering, result, clustering.Length);
            return result;
        }

        private void InitRandom(double[][] data)
        {
            int numTuples = data.Length;
            int clusterID = 0;
            for (int i = 0; i < numTuples; ++i)
            {
                clustering[i] = clusterID++;
                if (clusterID == numClusters)
                    clusterID = 0;
            }

            for (int i = 0; i < numTuples; ++i)
            {
                int r = rnd.Next(i, clustering.Length); // pick a cell
                int tmp = clustering[r]; // get the cell value
                clustering[r] = clustering[i]; // swap values
                clustering[i] = tmp;
            }
        } // InitRandom

        private void UpdateCentroids(double[][] data)
        {
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; ++i)
            {
                int clusterID = clustering[i];
                ++clusterCounts[clusterID];
            }

            for (int k = 0; k < centroids.Length; ++k)
                for (int j = 0; j < centroids[k].Length; ++j)
                    centroids[k][j] = 0.0;

            for (int i = 0; i < data.Length; ++i)
            {
                int clusterID = clustering[i];
                for (int j = 0; j < data[i].Length; ++j)
                    centroids[clusterID][j] += data[i][j]; // accumulate sum
            }

            for (int k = 0; k < centroids.Length; ++k)
                for (int j = 0; j < centroids[k].Length; ++j)
                    centroids[k][j] /= clusterCounts[k]; // danger?
        } // UpdateCentroids

        private bool UpdateClustering(double[][] data)
        {
            bool changed = false;
            int[] newClustering = new int[clustering.Length];
            Array.Copy(clustering, newClustering, clustering.Length);
            double[] distances = new double[numClusters];

            for (int i = 0; i < data.Length; ++i) // each tuple
            {
                for (int k = 0; k < numClusters; ++k)
                    distances[k] = Distance(data[i], centroids[k]);
                int newClusterID = MinIndex(distances); // closest centroid
                if (newClusterID != newClustering[i])
                {
                    changed = true; // note a new clustering
                    newClustering[i] = newClusterID; // accept update
                }
            }

            if (changed == false)
                return false;

            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; ++i)
            {
                int clusterID = newClustering[i];
                ++clusterCounts[clusterID];
            }
            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad proposed clustering

            Array.Copy(newClustering, this.clustering, newClustering.Length);
            return true;
        } // UpdateClustering

        private static double Distance(double[] tuple, double[] centroid)
        {
            double sumSquaredDiffs = 0.0;
            for (int j = 0; j < tuple.Length; ++j)
                sumSquaredDiffs += (tuple[j] - centroid[j]) * (tuple[j] - centroid[j]);
            return Math.Sqrt(sumSquaredDiffs);
        }

        private static int MinIndex(double[] distances)
        {
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 1; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }
    }
}
