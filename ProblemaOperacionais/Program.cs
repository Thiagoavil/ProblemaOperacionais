using System;
using System.Collections.Generic;

namespace AlgoritmoGenetico
{
    class Program
    {
        static int TAM_POPULACAO = 5;
        static int NUM_GERACOES = 10;
        static double TAXA_MUTACAO = 0.1;
        static double TAXA_CRUZAMENTO = 0.7;
        static int TAM_TORNEIO = 3;

        static int TAM_CROMOSSOMO = 4;
        static int[] ESPACO_MINIMO = { 2, 3, 4, 5 };
        static int ESPACO_TOTAL = 20;

        static double[] CUSTO_PLANTIO = { 1000, 2000, 3000, 4000 };
        static double[] VALOR_MERCADO = { 10000, 20000, 30000, 40000 };
        static int[] TEMPO_CRESCIMENTO = { 3, 4, 5, 6 };

        static Random rand = new Random();

        static void Main(string[] args)
        {
            List<int[]> populacao = InicializarPopulacao();

            for (int geracao = 0; geracao < NUM_GERACOES; geracao++)
            {
                Console.WriteLine($"Geração {geracao + 1}");
                populacao = GerarNovaPopulacao(populacao);
            }

            Console.WriteLine("Melhor solução encontrada:");
            int[] melhorSolucao = ObterMelhorSolucao(populacao);
            Console.WriteLine($"Árvore A: {melhorSolucao[0]} hectares");
            Console.WriteLine($"Árvore B: {melhorSolucao[1]} hectares");
            Console.WriteLine($"Árvore C: {melhorSolucao[2]} hectares");
            Console.WriteLine($"Árvore D: {melhorSolucao[3]} hectares");
        }

        static List<int[]> InicializarPopulacao()
        {
            List<int[]> populacao = new List<int[]>();

            for (int i = 0; i < TAM_POPULACAO; i++)
            {
                int[] cromossomo = new int[TAM_CROMOSSOMO];

                for (int j = 0; j < TAM_CROMOSSOMO; j++)
                {
                    cromossomo[j] = rand.Next(ESPACO_MINIMO[j], ESPACO_TOTAL + 1);
                }

                populacao.Add(cromossomo);
            }

            return populacao;
        }

        static List<int[]> GerarNovaPopulacao(List<int[]> populacaoAtual)
        {
            List<int[]> novaPopulacao = new List<int[]>();

            while (novaPopulacao.Count < TAM_POPULACAO)
            {
                int[] pai1 = SelecionarIndividuo(populacaoAtual);
                int[] pai2 = SelecionarIndividuo(populacaoAtual);

                if (rand.NextDouble() < TAXA_CRUZAMENTO)
                {
                    int[] filho1, filho2;
                    Cruzamento(pai1, pai2, out filho1, out filho2);
                    Mutacao(filho1);
                    Mutacao(filho2);

                    novaPopulacao.Add(filho1);
                    novaPopulacao.Add(filho2);
                }
                else
                {
                    novaPopulacao.Add(pai1);
                    novaPopulacao.Add(pai2);
                }
            }

            return novaPopulacao;
        }

        static int[] SelecionarIndividuo(List<int[]> populacao)
        {
            int[] vencedor = null;

            for (int i = 0; i < TAM_TORNEIO; i++)
            {
                int[] individuo = populacao[rand.Next(TAM_POPULACAO)];

                if (vencedor == null || AvaliarIndividuo(individuo) > AvaliarIndividuo(vencedor))
                {
                    vencedor = individuo;
                }
            }

            return vencedor;
        }

        static void Cruzamento(int[] pai1, int[] pai2, out int[] filho1, out int[] filho2)
        {
            int pontoCorte = rand.Next(TAM_CROMOSSOMO);
            filho1 = new int[TAM_CROMOSSOMO];
            filho2 = new int[TAM_CROMOSSOMO];

            for (int i = 0; i < TAM_CROMOSSOMO; i++)
            {
                if (i < pontoCorte)
                {
                    filho1[i] = pai1[i];
                    filho2[i] = pai2[i];
                }
                else
                {
                    filho1[i] = pai2[i];
                    filho2[i] = pai1[i];
                }
            }
        }

        static void Mutacao(int[] individuo)
        {
            for (int i = 0; i < TAM_CROMOSSOMO; i++)
            {
                if (rand.NextDouble() < TAXA_MUTACAO)
                {
                    individuo[i] = rand.Next(ESPACO_MINIMO[i], ESPACO_TOTAL + 1);
                }
            }
        }

        static int AvaliarIndividuo(int[] individuo)
        {
            int areaTotal = 0;
            double custoTotal = 0;
            double valorTotal = 0;

            for (int i = 0; i < TAM_CROMOSSOMO; i++)
            {
                areaTotal += individuo[i];
                custoTotal += CUSTO_PLANTIO[i] * individuo[i];
                valorTotal += VALOR_MERCADO[i] * individuo[i] * Math.Pow(1.1, TEMPO_CRESCIMENTO[i]);
            }

            if (areaTotal > ESPACO_TOTAL)
            {
                return 0;
            }
            else
            {
                return (int)(valorTotal - custoTotal);
            }
        }

        static int[] ObterMelhorSolucao(List<int[]> populacao)
        {
            int[] melhorSolucao = null;
            int melhorAvaliacao = int.MinValue;

            foreach (int[] individuo in populacao)
            {
                int avaliacao = AvaliarIndividuo(individuo);

                if (melhorSolucao == null || avaliacao > melhorAvaliacao)
                {
                    melhorSolucao = individuo;
                    melhorAvaliacao = avaliacao;
                }
            }

            return melhorSolucao;
        }
    }
}

