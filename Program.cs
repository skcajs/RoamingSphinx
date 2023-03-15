namespace RoamingSphynx
{
    class Program
    {
        static void Main(string[] args)
        {
            FitnessFunction fitness = new BasicFitnessFunction();
            Population pop = new Population(100);
            GA ga = new GA(pop, fitness);
            // ga.sort();
        }
    }

    class GA
    {
        Population population;
        FitnessFunction fitness;

        public GA(Population population, FitnessFunction fitness) {
            this.population = population;
            this.fitness = fitness;
        }

        public void InitPopulation(int popSize, int geneSize) {
            for (int i = 0; i < popSize; i++) {
                this.population.generateGenes(geneSize);
            }
        }

        public void cost() {
            
        }

        public void cross() {

        }

        public void mutate() {

        }
    }

    class Population
    {
        private List<int[]> population;

        public Population(int size) {
            this.population = new List<int[]>();
        }

        public void generateGenes(int size) {
            Random rn = new Random();
            int[] pop = new int[size];
            for(int i = 0; i < size; i++) {
                pop[i] =  rn.Next() % 2;
            }
            this.population.Add(pop);
        }
    }

    abstract class FitnessFunction
    {
        public abstract int CalculateFitness(int[] genes);
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