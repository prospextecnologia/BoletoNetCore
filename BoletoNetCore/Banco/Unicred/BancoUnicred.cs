using BoletoNetCore.Exceptions;
using System;
using System.Collections.Generic;

namespace BoletoNetCore
{
    internal sealed partial class BancoUnicred : BancoFebraban<BancoUnicred>, IBanco
    {
        public BancoUnicred()
        {
            Codigo = 136;
            Nome = "UNICRED DO BRASIL";
            Digito = "8";
            IdsRetornoCnab400RegistroDetalhe = new List<string> { "1" };
            RemoveAcentosArquivoRemessa = true;
        }

        public void FormataBeneficiario()
        {
            var contaBancaria = Beneficiario.ContaBancaria;

            if (!CarteiraFactory<BancoUnicred>.CarteiraEstaImplementada(contaBancaria.CarteiraPadrao))
                throw BoletoNetCoreException.CarteiraNaoImplementada(contaBancaria.CarteiraPadrao);

            contaBancaria.FormatarDados("PAGÁVEL EM QUALQUER BANCO ATÉ O VENCIMENTO.", "", "", 9);
            Beneficiario.CodigoFormatado = $"{Beneficiario.ContaBancaria.Agencia}/{Beneficiario.ContaBancaria.Conta}-{Beneficiario.ContaBancaria.DigitoConta}";
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            var carteira = CarteiraFactory<BancoUnicred>.ObterCarteira(boleto.Carteira);
            carteira.FormataNossoNumero(boleto);
        }

        public override string FormataCodigoBarraCampoLivre(Boleto boleto)
        {
            var carteira = CarteiraFactory<BancoUnicred>.ObterCarteira(boleto.Carteira);
            return carteira.FormataCodigoBarraCampoLivre(boleto);
        }


        public override string FormatarNomeArquivoRemessa(TipoArquivo TipoArquivo, IBanco Banco, int sequencial)
        {
            sequencial = sequencial % 100;

            if (sequencial < 0)
                throw BoletoNetCoreException.NumeroSequencialInvalido(sequencial);

            if (sequencial > 10)
            {
                sequencial = sequencial % 10;
            }

            //número máximos de arquivos enviados no dia são 10 
            return string.Format("{0}_UNICRED_{1}_{2}_{3}_{4}.REM",
                TipoArquivo == TipoArquivo.CNAB240 ? "CNAB240" : "CNAB400",
                Beneficiario.Codigo.PadLeft(10, '0'),
                Beneficiario.ContaBancaria.Agencia.PadLeft(4, '0'),
                DateTime.Now.ToString("ddMMyyyyy"),
                $"{(sequencial == 10 ? 0 : sequencial).ToString().PadLeft(2, '0')}");
        }

        public string GerarMensagemRemessa(TipoArquivo tipoArquivo, Boleto boleto, ref int numeroRegistro)
        {
            return null;
        }


        public override void LerHeaderRetornoCNAB240(ArquivoRetorno arquivoRetorno, string registro)
        {
            arquivoRetorno.Banco.Beneficiario = new Beneficiario();

            if (registro.Substring(17, 1) == "1")
                arquivoRetorno.Banco.Beneficiario.CPFCNPJ = registro.Substring(21, 11);
            else
                arquivoRetorno.Banco.Beneficiario.CPFCNPJ = registro.Substring(18, 14);

            arquivoRetorno.Banco.Beneficiario.Nome = registro.Substring(72, 30).Trim();


            arquivoRetorno.Banco.Beneficiario.ContaBancaria = new ContaBancaria();

            arquivoRetorno.Banco.Beneficiario.Codigo = Utils.ToInt32(registro.Substring(58, 14)).ToString();
            arquivoRetorno.Banco.Beneficiario.ContaBancaria.Agencia = registro.Substring(52, 5);
            arquivoRetorno.Banco.Beneficiario.ContaBancaria.DigitoAgencia = registro.Substring(57, 1);

            //arquivoRetorno.Banco.Beneficiario.ContaBancaria.Conta = registro.Substring(58, 12);
            //arquivoRetorno.Banco.Beneficiario.ContaBancaria.DigitoConta = registro.Substring(70, 1);

            arquivoRetorno.DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(143, 8)).ToString("##-##-####"));
            arquivoRetorno.NumeroSequencial = Utils.ToInt32(registro.Substring(157, 6));
        }
    }
}