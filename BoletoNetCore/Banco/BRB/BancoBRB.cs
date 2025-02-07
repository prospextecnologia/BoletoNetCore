using BoletoNetCore.Exceptions;
using System;
using System.Collections.Generic;
using static System.String;

namespace BoletoNetCore
{
    internal sealed partial class BancoBRB : BancoFebraban<BancoBRB>, IBanco
    {
        public BancoBRB()
        {
            Codigo = 70;
            Nome = "BRB";
            Digito = "1";
            IdsRetornoCnab400RegistroDetalhe = new List<string> { "1" };
            RemoveAcentosArquivoRemessa = true;
        }

        public void FormataBeneficiario()
        {
            var contaBancaria = Beneficiario.ContaBancaria;

            if (!CarteiraFactory<BancoBRB>.CarteiraEstaImplementada(contaBancaria.CarteiraComVariacaoPadrao))
                throw BoletoNetCoreException.CarteiraNaoImplementada(contaBancaria.CarteiraComVariacaoPadrao);

            var codigoBeneficiario = Beneficiario.Codigo;
            if (Beneficiario.CodigoDV == Empty)
                throw new Exception($"Dígito do código do beneficiário ({codigoBeneficiario}) não foi informado.");

            contaBancaria.FormatarDados("PAGÁVEL PREFERENCIALMENTE NO SICOOB.", "", "", 9);

            Beneficiario.Codigo = codigoBeneficiario.Length <= 9 ? codigoBeneficiario.PadLeft(9, '0') : throw BoletoNetCoreException.CodigoBeneficiarioInvalido(codigoBeneficiario, 9);
            Beneficiario.CodigoFormatado = $"{codigoBeneficiario}-{Beneficiario.CodigoDV}";
        }


        public string GerarMensagemRemessa(TipoArquivo tipoArquivo, Boleto boleto, ref int numeroRegistro)
        {
            return null;
        }
    }
}
