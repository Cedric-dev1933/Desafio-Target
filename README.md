# Desafios de Programação em C#

Este repositório reúne a resolução de três desafios de programação, desenvolvidos em C# como aplicações de console. Cada desafio é um projeto independente, em sua própria pasta, e pode ser executado separadamente.

## Requisitos

É necessário ter o [.NET SDK](https://dotnet.microsoft.com/download) instalado. Os projetos foram desenvolvidos com o .NET 10. Para conferir a versão instalada, use `dotnet --version`.

## Como executar

Abra um terminal na pasta do desafio desejado e rode o projeto:

```
cd Comissoes
dotnet run
```

Basta trocar `Comissoes` por `Estoque` ou `Juros` para executar os outros desafios. Os projetos que leem arquivos JSON (`Comissoes` e `Estoque`) procuram o arquivo na própria pasta, então o comando deve ser executado de dentro dela.

## Estrutura

```
Comissoes/   Desafio 1: comissão de vendedores
Estoque/     Desafio 2: movimentação de estoque
Juros/       Desafio 3: juros por atraso
```

## Desafio 1: Comissões (`Comissoes`)

O programa lê um arquivo `vendas.json` com os registros de vendas de um time comercial e calcula a comissão total de cada vendedor. A regra é aplicada a cada venda individualmente: vendas abaixo de R$ 100,00 não geram comissão, vendas abaixo de R$ 500,00 geram 1% e vendas a partir de R$ 500,00 geram 5%. As comissões de cada venda são então somadas por vendedor e exibidas no console, formatadas em reais.

Os valores monetários usam o tipo `decimal`, mais adequado que `double` por evitar erros de arredondamento.

## Desafio 2: Estoque (`Estoque`)

O programa carrega o estoque inicial de um arquivo `estoque.json` e oferece um menu interativo no console para listar o estoque, lançar movimentações e consultar o histórico. Cada movimentação, de entrada ou de saída, recebe um identificador único e sequencial, tem uma descrição que identifica o tipo da operação (por exemplo, compra, venda, devolução ou ajuste) e registra a data e a hora do lançamento. Ao final de cada lançamento, o programa mostra a quantidade final em estoque do produto movimentado.

O programa valida as entradas: o produto precisa existir, a quantidade deve ser um inteiro maior que zero e uma saída maior que o saldo disponível é recusada, para que o estoque nunca fique negativo.

O estoque e o histórico ficam apenas em memória durante a execução. Ao fechar o programa, os dados voltam aos valores do JSON, e por isso o identificador é único dentro de cada execução.

## Desafio 3: Juros por atraso (`Juros`)

O programa pede um valor e uma data de vencimento e calcula os juros acumulados até a data de hoje, considerando uma taxa de 2,5% ao dia. Ele informa os dias em atraso, o valor dos juros e o valor total atualizado. Se a conta ainda não venceu, informa que não há juros.

O cálculo adota juros simples, ou seja, `valor × 0,025 × dias de atraso`. Por exemplo, uma conta de R$ 1.000,00 vencida há 10 dias gera R$ 250,00 de juros, resultando em R$ 1.250,00.

## Tecnologias

C#, .NET 10, `System.Text.Json` para leitura dos arquivos JSON e LINQ para o agrupamento e a soma das comissões.
