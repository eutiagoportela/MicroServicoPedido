using MicroServicoPedido.Domain.Entities;

public class EmbalagemService
{
    public (List<string> ProdutosNaCaixa, List<Produto> ProdutosQueNaoCabem) EmbalarProdutos(List<Produto> produtosRestantes, Caixa caixa)
    {
        var produtosNaCaixa = new List<string>();
        var produtosQueNaoCabem = new List<Produto>();

        double volumeTotal = caixa.Altura * caixa.Largura * caixa.Comprimento;
        double volumeUsado = 0;
        double larguraUsada = 0;

        foreach (var produto in produtosRestantes)
        {
            double volumeProduto = produto.Dimensoes.Altura * produto.Dimensoes.Largura * produto.Dimensoes.Comprimento;

            if (ProdutoCaberNaCaixa(produto, caixa) &&
                (volumeUsado + volumeProduto <= volumeTotal) &&
                (larguraUsada + produto.Dimensoes.Largura <= caixa.Largura))
            {
                produtosNaCaixa.Add(produto.ProdutoId);
                volumeUsado += volumeProduto;
                larguraUsada += produto.Dimensoes.Largura;
            }
            else
            {
                produtosQueNaoCabem.Add(produto);
            }
        }

        return (produtosNaCaixa, produtosQueNaoCabem);
    }

    private bool ProdutoCaberNaCaixa(Produto produto, Caixa caixa)
    {
        return produto.Dimensoes.Altura <= caixa.Altura &&
               produto.Dimensoes.Largura <= caixa.Largura &&
               produto.Dimensoes.Comprimento <= caixa.Comprimento;
    }
}
