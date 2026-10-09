using System.Globalization;
using System.Text.Json;

var cultura = new CultureInfo("pt-BR");

// Lê o arquivo JSON e converte para objetos C#, ignorando maiúsculas/minúsculas nos nomes dos campos.
var json = File.ReadAllText("vendas.json");
var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var dados = JsonSerializer.Deserialize<DadosVendas>(json, opcoes)
            ?? throw new InvalidOperationException("Não foi possível ler o JSON.");

// A comissão é calculada venda a venda e depois somada por vendedor.
var comissoes = dados.Vendas
    .GroupBy(v => v.Vendedor)
    .Select(g => new
    {
        Vendedor = g.Key,
        Comissao = g.Sum(v => CalcularComissao(v.Valor))
    });

foreach (var c in comissoes)
{
    Console.WriteLine($"{c.Vendedor}: {c.Comissao.ToString("C2", cultura)}");
}

// Regra de comissão de uma única venda. Usa decimal por ser o tipo adequado para dinheiro.
static decimal CalcularComissao(decimal valor)
{
    if (valor < 100m) return 0m;          // abaixo de R$100: sem comissão
    if (valor < 500m) return valor * 0.01m; // de R$100 até R$499,99: 1%
    return valor * 0.05m;                 // a partir de R$500: 5%
}

// Modelos que espelham a estrutura do JSON.
record Venda(string Vendedor, decimal Valor);
record DadosVendas(List<Venda> Vendas);
