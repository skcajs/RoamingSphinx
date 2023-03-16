namespace RoamingSphynx
{
    class Program
    {
        static void Main(string[] args)
        {
            FitnessFunction fitness = new BasicFitnessFunction();
            Population pop = new Population(100, 16);
            GA ga = new GA(pop, fitness);
            for (int i = 0; i < 1000; i++)
            {
                ga.Fitness();
                ga.Crossover();
                ga.Mutate();
            }
            ga.Score();

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
        public List<FitnessRecord> records { get; }
        private Random rn;

        public GA(Population population, FitnessFunction fitnessFunction)
        {
            this.population = population;
            this.fitnessFunction = fitnessFunction;
            this.records = new List<FitnessRecord>();
            this.rn = new Random();
        }

        public void Fitness()
        {
            records.Clear();
            foreach (int[] gene in this.population.genes)
            {
                this.records.Add(new FitnessRecord(gene, fitnessFunction.CalculateFitness(gene)));
            }
            int nRecordsHalf = records.Count / 2;
            this.records.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
            this.records.RemoveRange((nRecordsHalf - 1), nRecordsHalf);
            this.population.genes = this.population.genes.Where(g => records.Any(a => a.Gene.Equals(g))).ToList();
        }

        public void Crossover()
        {
            int crossPoint = (int)(this.population.shape.Item2 / 3);
            int count = this.population.genes.Count / 2;
            this.population.genes.OrderBy(a => this.rn.Next());

            List<int[]> pool1 = this.population.genes.Take(count).ToList();
            List<int[]> pool2 = this.population.genes.Skip(count).ToList();

            for (int i = 0; i < count; ++i)
            {
                population.genes.Add(pool1[i].Take(crossPoint).Concat(pool2[i].Skip(crossPoint)).ToArray());
                population.genes.Add(pool2[i].Take(crossPoint).Concat(pool1[i].Skip(crossPoint)).ToArray());
            }
        }

        public void Mutate()
        {
            int count = this.population.genes.Count / 2;
            for (int i = 0; i < count; ++i)
            {
                int mutateIndex = rn.Next() % this.population.shape.Item2;
                bool mutateChromosone = !Convert.ToBoolean(this.population.genes[count + 1][mutateIndex]);
                this.population.genes[count + 1][mutateIndex] = Convert.ToInt32(mutateChromosone);
            }
        }

        public void Score()
        {
            Console.WriteLine(records);
        }
    }

    class Population
    {
        public List<int[]> genes { get; set; }
        public (int, int) shape { get; }

        public Population(int popSize, int geneSize)
        {
            this.shape = (popSize, geneSize);
            this.genes = new List<int[]>();
            Init(popSize, geneSize);
        }

        public void Init(int popSize, int geneSize)
        {
            for (int i = 0; i < popSize; i++)
            {
                generateGenes(geneSize);
            }
        }

        private void generateGenes(int size)
        {
            Random rn = new Random();
            int[] pop = new int[size];
            for (int i = 0; i < size; i++)
            {
                pop[i] = rn.Next() % 2;
            }
            this.genes.Add(pop);
        }
    }

    abstract class FitnessFunction
    {
        public abstract int CalculateFitness(int[] gene);
    }

    class BasicFitnessFunction : FitnessFunction
    {
        int fitness;

        public BasicFitnessFunction()
        {
            fitness = 0;
        }

        public override int CalculateFitness(int[] genes)
        {
            this.fitness = genes.Length;
            foreach (int gene in genes)
            {
                this.fitness = gene == 1 ? --this.fitness : this.fitness;
            }
            return this.fitness;
        }
    }
}