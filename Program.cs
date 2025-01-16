using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1) Calcular soma de números");
            Console.WriteLine("2) Calcular Fibonacci e verificar se pertence");
            Console.WriteLine("3) Analisar faturamento mensal (JSON/XML)");
            Console.WriteLine("4) Calcular percentual de faturamento por estado");
            Console.WriteLine("5) Inverter string");
            Console.WriteLine("6) Sair");
            Console.Write("\nDigite a opção (1-6): ");

            string escolha = Console.ReadLine();

            switch (escolha)
            {
                case "1":
                    CalcularSoma();
                    break;
                case "2":
                    CalcularFibonacci();
                    break;
                case "3":
                    AnalisarFaturamento();
                    break;
                case "4":
                    CalcularPercentualFaturamento();
                    break;
                case "5":
                    InverterString();
                    break;
                case "6":
                    Console.WriteLine("Saindo...");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    Console.WriteLine("Aperte para continuar:");
                    Console.ReadLine(); 
                    break;
            }
        }
    }

    static void CalcularSoma()
    {
        int INDICE = 13, SOMA = 0, K = 0;
        while (K < INDICE)
        {
            K = K + 1;
            SOMA = SOMA + K;
        }
        Console.WriteLine($"Resultado da soma: {SOMA}");
        Console.ReadKey();
    }

    static void AnalisarFaturamento()
    {
        Console.WriteLine("Digite o caminho completo do arquivo JSON ou XML:");
        string caminhoArquivo = Console.ReadLine();

        if (!File.Exists(caminhoArquivo))
        {
            Console.WriteLine("Arquivo não encontrado. Tente novamente.");
            return;
        }

        string extensao = Path.GetExtension(caminhoArquivo).ToLower();

        try
        {
            if (extensao == ".json")
            {
                ProcessarArquivoJson(caminhoArquivo);
            }
            else if (extensao == ".xml")
            {
                ProcessarArquivoXml(caminhoArquivo);
            }
            else
            {
                Console.WriteLine("Formato de arquivo inválido.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao processar o arquivo: {e.Message}");
        }
    }
    static void ProcessarArquivoJson(string caminhoArquivo)
    {
        var faturamento = JsonConvert.DeserializeObject<List<Faturamento>>(File.ReadAllText(caminhoArquivo));

        var diasComFaturamento = faturamento.Where(f => f.Valor > 0).ToList();
        double totalFaturamento = diasComFaturamento.Sum(f => f.Valor);
        double mediaFaturamento = totalFaturamento / diasComFaturamento.Count;

        double menorFaturamento = faturamento.Where(f => f.Valor > 0).Min(f => f.Valor);
        double maiorFaturamento = faturamento.Where(f => f.Valor > 0).Max(f => f.Valor);

        int diasAcimaDaMedia = faturamento.Count(f => f.Valor > mediaFaturamento);

        Console.WriteLine($"Menor faturamento: {menorFaturamento}");
        Console.WriteLine($"Maior faturamento: {maiorFaturamento}");
        Console.WriteLine($"Número de dias acima da média: {diasAcimaDaMedia}");
        Console.ReadKey();
    }

    static void ProcessarArquivoXml(string caminhoArquivo)
    {
        XDocument xmlDoc = XDocument.Load(caminhoArquivo);
        var faturamento = xmlDoc.Descendants("dia")
            .Select(x => new Faturamento
            {
                Dia = (int)x.Element("numero"),
                Valor = (double)x.Element("valor")
            }).ToList();

        var diasComFaturamento = faturamento.Where(f => f.Valor > 0).ToList();
        double totalFaturamento = diasComFaturamento.Sum(f => f.Valor);
        double mediaFaturamento = totalFaturamento / diasComFaturamento.Count;

        double menorFaturamento = faturamento.Where(f => f.Valor > 0).Min(f => f.Valor);
        double maiorFaturamento = faturamento.Where(f => f.Valor > 0).Max(f => f.Valor);

        int diasAcimaDaMedia = faturamento.Count(f => f.Valor > mediaFaturamento);

        Console.WriteLine($"Menor faturamento: {menorFaturamento}");
        Console.WriteLine($"Maior faturamento: {maiorFaturamento}");
        Console.WriteLine($"Número de dias acima da média: {diasAcimaDaMedia}");
        Console.ReadKey();
    }
    public class Faturamento
    {
        public int Dia { get; set; }
        public double Valor { get; set; }
    }

    static void InverterString()
    {
        Console.WriteLine("Digite a string a ser invertida:");
        string input = Console.ReadLine();
        char[] array = input.ToCharArray();
        Array.Reverse(array);
        string reversed = new string(array);
        Console.WriteLine($"String invertida: {reversed}");
        Console.ReadKey();
    }

    static void CalcularFibonacci()
    {
        Console.WriteLine("Digite um número para verificar se pertence à sequência Fibonacci:");
        int num = int.Parse(Console.ReadLine());

        List<int> fibonacci = new List<int> { 0, 1 };
        while (fibonacci.Last() < num)
        {
            int next = fibonacci[fibonacci.Count - 1] + fibonacci[fibonacci.Count - 2];
            fibonacci.Add(next);
        }

        if (fibonacci.Contains(num))
        {
            Console.WriteLine($"O número {num} pertence à sequência Fibonacci.");
        }
        else
        {
            Console.WriteLine($"O número {num} NÃO pertence à sequência Fibonacci.");
        }
        Console.ReadKey();
    }

    static void CalcularPercentualFaturamento()
    {
        var faturamentoPorEstado = new Dictionary<string, double>
        {
            { "SP", 67836.43 },
            { "RJ", 36678.66 },
            { "MG", 29229.88 },
            { "ES", 27165.48 },
            { "Outros", 19849.53 }
        };

        double totalFaturamento = faturamentoPorEstado.Values.Sum();

        Console.WriteLine("Percentual de faturamento por estado:");
        foreach (var estado in faturamentoPorEstado)
        {
            double percentual = (estado.Value / totalFaturamento) * 100;
            Console.WriteLine($"{estado.Key}: {percentual:F2}%");
        }

        Console.ReadKey();
    }
}
