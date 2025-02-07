using System;
using System.IO;

namespace BoletoNetCore.QuestPDF.AppTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                Console.Write("Informe o diretório para gerar o pdf: ");
                currentDir = Console.ReadLine();
                currentDir = String.IsNullOrEmpty(currentDir) ? Directory.GetCurrentDirectory() : currentDir;
                if (!Directory.Exists(currentDir))
                    throw new Exception("O diretório informado não existe: " + currentDir);

                Console.WriteLine("Aguarde, gerando pdf...");

                var contaBancaria = new ContaBancaria
                {
                    Agencia = "081",
                    DigitoAgencia = "3",
                    Conta = "240004499",
                    DigitoConta = "0",
                    CarteiraPadrao = "COB",
                    TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                    //VariacaoCarteiraPadrao = "A",
                    TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa
                    //OperacaoConta = "05"

                };
                var banco = Banco.Instancia(Bancos.BRB);
                banco.Beneficiario = Utils.GerarBeneficiario("240004499", "0", "", contaBancaria);
                banco.FormataBeneficiario();

                var boletos = Utils.GerarBoletos(banco, 4, "N", 10);
                var bytes = boletos.ImprimirCarnePdf();
                Console.WriteLine("Pdf gerado, salvando arquivo...");
                var fileName = Path.Combine(currentDir, "carne.pdf");
                File.WriteAllBytes(fileName, bytes);
                Console.WriteLine("Pdf gerado com sucesso: " + fileName);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
