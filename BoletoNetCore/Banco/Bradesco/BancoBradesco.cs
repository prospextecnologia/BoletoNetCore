using BoletoNetCore.Exceptions;
using System.Collections.Generic;

namespace BoletoNetCore
{
    internal sealed partial class BancoBradesco : BancoFebraban<BancoBradesco>, IBanco
    {
        public BancoBradesco()
        {
            Codigo = 237;
            Nome = "Bradesco";
            Digito = "2";
            IdsRetornoCnab400RegistroDetalhe = new List<string> { "1" };
            RemoveAcentosArquivoRemessa = true;
        }


        public void FormataBeneficiario()
        {
            var contaBancaria = Beneficiario.ContaBancaria;

            if (!CarteiraFactory<BancoBradesco>.CarteiraEstaImplementada(contaBancaria.CarteiraComVariacaoPadrao))
                throw BoletoNetCoreException.CarteiraNaoImplementada(contaBancaria.CarteiraComVariacaoPadrao);

            contaBancaria.FormatarDados("PAGÁVEL PREFERENCIALMENTE NA REDE BRADESCO OU BRADESCO EXPRESSO.", "", "", 7);

            var codigoBeneficiario = Beneficiario.Codigo;
            Beneficiario.Codigo = codigoBeneficiario.Length <= 20 ? codigoBeneficiario.PadLeft(20, '0') : throw BoletoNetCoreException.CodigoBeneficiarioInvalido(codigoBeneficiario, 20);

            Beneficiario.CodigoFormatado = $"{contaBancaria.Agencia}-{contaBancaria.DigitoAgencia} / {contaBancaria.Conta}-{contaBancaria.DigitoConta}";
        }

        public string GerarMensagemRemessa(TipoArquivo tipoArquivo, Boleto boleto, ref int numeroRegistro)
        {
            return null;
        }
    }
}