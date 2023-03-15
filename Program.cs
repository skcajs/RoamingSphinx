namespace RoamingSphynx
{
    class Program
    {
        static void Main(string[] args)
        {
            FitnessFunction fitness = new BasicFitnessFunction();
            Population pop = new Population(100, 16);
            GA ga = new GA(pop, fitness);
            ga.Fitness();
            Console.WriteLine(ga.records.Count);
            // foreach(FitnessRecord record in ga.records) {
            //     Console.WriteLine(string.Join("", record.Gene));
            //     Console.WriteLine(record.Fitness);
            //     Console.WriteLine("\n");
            // }
        }
    }

    public struct FitnessRecord
    {
        public FitnessRecord(int[] gene, int fitness)
        {
            Gene = gene;
            Fitness = fitness;
        }

        public int[] Gene { get; }
        public int Fitness { get; }
    }

    class GA
    {
        private Population population;
        private FitnessFunction fitnessFunction;
        public List<FitnessRecord> records { get;}

        public GA(Population population, FitnessFunction fitnessFunction) {
            this.population = population;
            this.fitnessFunction = fitnessFunction;
            this.records = new List<FitnessRecord>();
        }

        public void Fitness() {
            records.Clear();
            foreach (int[] gene in this.population.genes) {
                this.records.Add(new FitnessRecord(gene, fitnessFunction.CalculateFitness(gene)));
            }
            int nRecordsHalf = records.Count / 2;
            this.records.Sort((x,y) => x.Fitness.CompareTo(y.Fitness));
            this.records.RemoveRange((nRecordsHalf - 1), nRecordsHalf);
        }

        public void Cross() {

        }

        public void Mutate() {

        }
    }

    class Population
    {
        public List<int[]> genes {get;}

        public Population(int popSize, int geneSize) {
            this.genes = new List<int[]>();
            Init(popSize, geneSize);
        }

        public void Init(int popSize, int geneSize) {
            for (int i = 0; i < popSize; i++) {
                generateGenes(geneSize);
            }
        }

        private void generateGenes(int size) {
            Random rn = new Random();
            int[] pop = new int[size];
            for(int i = 0; i < size; i++) {
                pop[i] =  rn.Next() % 2;
            }
            this.genes.Add(pop);
        }

        public void updateGenes() {
            
        }
    }

    abstract class FitnessFunction
    {
        public abstract int CalculateFitness(int[] gene);
    }

    class BasicFitnessFunction : FitnessFunction
    {
        int fitness;

        public BasicFitnessFunction() {
            fitness = 0;
        }

        public override int CalculateFitness(int[] genes) {
            this.fitness = genes.Length;
            foreach (int gene in genes) {
                this.fitness = gene == 1 ? --this.fitness : this.fitness;
            }
            return this.fitness;
        }
    }
}