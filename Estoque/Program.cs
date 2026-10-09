using System.Text.Json;

// Carrega o estoque inicial a partir do JSON.
var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var dados = JsonSerializer.Deserialize<DadosEstoque>(File.ReadAllText("estoque.json"), opcoes)
            ?? throw new InvalidOperationException("Não foi possível ler o estoque.json.");

var produtos = dados.Estoque;
var movimentacoes = new List<Movimentacao>(); // histórico, mantido apenas em memória

// Menu principal: repete até o usuário escolher sair.
while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Controle de Estoque ===");
    Console.WriteLine("1 - Listar estoque");
    Console.WriteLine("2 - Lançar movimentação");
    Console.WriteLine("3 - Histórico de movimentações");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": ListarEstoque(); break;
        case "2": LancarMovimentacao(); break;
        case "3": ListarHistorico(); break;
        case "0": return;
        default: Console.WriteLine("Opção inválida."); break;
    }
}

void ListarEstoque()
{
    Console.WriteLine();
    foreach (var p in produtos)
        Console.WriteLine($"{p.CodigoProduto} - {p.DescricaoProduto}: {p.Estoque} un.");
}

void LancarMovimentacao()
{
    ListarEstoque();
    Console.WriteLine();

    int codigo = LerInteiroPositivo("Código do produto: ");
    var produto = produtos.FirstOrDefault(p => p.CodigoProduto == codigo);
    if (produto is null)
    {
        Console.WriteLine("Produto não encontrado.");
        return;
    }

    // Pergunta até receber "E" ou "S"; qualquer outra resposta mantém tipo nulo e repete.
    TipoMovimentacao? tipo = null;
    while (tipo is null)
    {
        Console.Write("Tipo (E = entrada, S = saída): ");
        tipo = Console.ReadLine()?.Trim().ToUpperInvariant() switch
        {
            "E" => TipoMovimentacao.Entrada,
            "S" => TipoMovimentacao.Saida,
            _ => null
        };
        if (tipo is null) Console.WriteLine("Digite E ou S.");
    }

    int quantidade = LerInteiroPositivo("Quantidade: ");

    // Impede que o estoque fique negativo.
    if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
    {
        Console.WriteLine($"Estoque insuficiente. Disponível: {produto.Estoque} un.");
        return;
    }

    string descricao;
    do
    {
        Console.Write("Descrição da movimentação (ex.: Compra, Venda, Devolução, Ajuste): ");
        descricao = Console.ReadLine()?.Trim() ?? "";
    } while (descricao.Length == 0);

    produto.Estoque += tipo == TipoMovimentacao.Entrada ? quantidade : -quantidade;

    // O ID é sequencial: total de movimentações já registradas + 1.
    var mov = new Movimentacao(
        Id: movimentacoes.Count + 1,
        CodigoProduto: produto.CodigoProduto,
        Tipo: tipo.Value,
        Descricao: descricao,
        Quantidade: quantidade,
        DataHora: DateTime.Now);
    movimentacoes.Add(mov);

    Console.WriteLine();
    Console.WriteLine($"Movimentação #{mov.Id} registrada ({mov.Tipo} - {mov.Descricao}).");
    Console.WriteLine($"Estoque final de {produto.DescricaoProduto}: {produto.Estoque} un.");
}

void ListarHistorico()
{
    Console.WriteLine();
    if (movimentacoes.Count == 0)
    {
        Console.WriteLine("Nenhuma movimentação lançada ainda.");
        return;
    }

    foreach (var m in movimentacoes)
    {
        string sinal = m.Tipo == TipoMovimentacao.Entrada ? "+" : "-";
        Console.WriteLine($"#{m.Id} | {m.DataHora:dd/MM/yyyy HH:mm} | produto {m.CodigoProduto} | {m.Tipo} | {sinal}{m.Quantidade} | {m.Descricao}");
    }
}

// Repete a pergunta até o usuário digitar um inteiro maior que zero.
int LerInteiroPositivo(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);
        if (int.TryParse(Console.ReadLine(), out int valor) && valor > 0)
            return valor;
        Console.WriteLine("Digite um número inteiro maior que zero.");
    }
}

// Enum evita erros de digitação com textos soltos como "entrada" ou "Entrada ".
enum TipoMovimentacao { Entrada, Saida }

record Movimentacao(int Id, int CodigoProduto, TipoMovimentacao Tipo, string Descricao, int Quantidade, DateTime DataHora);

// Classe (e não record) porque o campo Estoque precisa ser alterado a cada movimentação.
class Produto
{
    public int CodigoProduto { get; set; }
    public string DescricaoProduto { get; set; } = "";
    public int Estoque { get; set; }
}

// Espelha o JSON, que tem a lista de produtos dentro da chave "estoque".
class DadosEstoque
{
    public List<Produto> Estoque { get; set; } = new();
}
