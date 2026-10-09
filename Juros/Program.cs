using System.Globalization;

const decimal TaxaDiaria = 0.025m; // 2,5% ao dia

var cultura = new CultureInfo("pt-BR");

decimal valor = LerValor();
DateTime vencimento = LerData();
DateTime hoje = DateTime.Today;

// Positivo = conta vencida; zero ou negativo = ainda no prazo.
int diasAtraso = (hoje - vencimento).Days;

Console.WriteLine();
Console.WriteLine($"Valor original: {valor.ToString("C2", cultura)}");
Console.WriteLine($"Vencimento: {vencimento:dd/MM/yyyy} | Hoje: {hoje:dd/MM/yyyy}");

if (diasAtraso <= 0)
{
    Console.WriteLine("A conta não está em atraso, portanto não há juros.");
    return;
}

// Juros simples: valor x taxa diária x dias de atraso, arredondado em centavos.
decimal juros = Math.Round(valor * TaxaDiaria * diasAtraso, 2);
decimal total = valor + juros;

Console.WriteLine($"Dias em atraso: {diasAtraso}");
Console.WriteLine($"Juros: {juros.ToString("C2", cultura)}");
Console.WriteLine($"Valor atualizado: {total.ToString("C2", cultura)}");

// Repete até receber um valor válido; a cultura pt-BR aceita "1500,50" e "1.500,50".
decimal LerValor()
{
    while (true)
    {
        Console.Write("Valor (ex.: 1500,50): ");
        if (decimal.TryParse(Console.ReadLine(), NumberStyles.Number, cultura, out decimal v) && v > 0)
            return v;
        Console.WriteLine("Valor inválido. Digite um número maior que zero.");
    }
}

// Exige o formato exato dd/MM/yyyy e rejeita datas inexistentes, como 31/02.
DateTime LerData()
{
    while (true)
    {
        Console.Write("Data de vencimento (dd/MM/yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine()?.Trim(), "dd/MM/yyyy", cultura, DateTimeStyles.None, out DateTime d))
            return d;
        Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy.");
    }
}
