using BoletoNetCore.Exceptions;
using System.Collections.Generic;

namespace BoletoNetCore
{
    internal sealed partial class BancoBrasil : BancoFebraban<BancoBrasil>, IBanco
    {
        public BancoBrasil()
        {
            Codigo = 1;
            Nome = "Banco do Brasil";
            Digito = "9";
            IdsRetornoCnab400RegistroDetalhe = new List<string> { "7" };
            RemoveAcentosArquivoRemessa = true;
        }


        public void FormataBeneficiario()
        {
            var contaBancaria = Beneficiario.ContaBancaria;

            if (!CarteiraFactory<BancoBrasil>.CarteiraEstaImplementada(contaBancaria.CarteiraComVariacaoPadrao))
                throw BoletoNetCoreException.CarteiraNaoImplementada(contaBancaria.CarteiraComVariacaoPadrao);

            contaBancaria.FormatarDados(string.IsNullOrEmpty(contaBancaria.LocalPagamento) ? "PAGÁVEL EM QUALQUER BANCO." : contaBancaria.LocalPagamento, "", "", 8);

            if (Beneficiario.Codigo.Length != 7)
                throw BoletoNetCoreException.CodigoBeneficiarioInvalido(Beneficiario.Codigo, 7);

            Beneficiario.CodigoFormatado = $"{contaBancaria.Agencia}{(string.IsNullOrEmpty(contaBancaria.DigitoAgencia) ? "" : "-" + contaBancaria.DigitoAgencia)} / {contaBancaria.Conta}{(string.IsNullOrEmpty(contaBancaria.DigitoConta) ? "" : "-" + contaBancaria.DigitoConta)}";
        }

        public string GerarMensagemRemessa(TipoArquivo tipoArquivo, Boleto boleto, ref int numeroRegistro)
        {
            return null;
        }

    }
}