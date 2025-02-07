using BoletoNetCore.Extensions;
using System;
using static System.String;

namespace BoletoNetCore
{
    [CarteiraCodigo("COB")]
    internal class BancoBRBCarteiraCOB : ICarteira<BancoBRB>
    {
        internal static Lazy<ICarteira<BancoBRB>> Instance { get; } = new Lazy<ICarteira<BancoBRB>>(() => new BancoBRBCarteiraCOB());

        private string incrementoCampoLivreNossoNumero = "000";

        private BancoBRBCarteiraCOB()
        {
        }

        public void FormataNossoNumero(Boleto boleto)
        {
            if (IsNullOrWhiteSpace(boleto.NossoNumero))
                throw new Exception("Nosso Número não informado.");

            //
            if (boleto.NossoNumero?.Length <= 6)
            {
                boleto.NossoNumero = $"1{boleto.NossoNumero.PadLeft(6, '0')}{boleto.Banco.Codigo.ToString().PadLeft(3, '0')}";
            }
            else
            {
                if (boleto.NossoNumero?.Length != 10 || boleto.NossoNumero?.Length != 12)
                    throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve não deve conter mais de 10 dígitos ou 12 com os digito verificador.");

                if (!'1'.Equals(boleto.NossoNumero[0]) && !'2'.Equals(boleto.NossoNumero[0]))
                {
                    throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve iniciar com digito 1 ou 2.");
                }

                if (boleto.NossoNumero?.Length == 10)
                {
                    if (!boleto.NossoNumero.EndsWith(boleto.Banco.Codigo.ToString().PadLeft(3, '0')))
                    {
                        throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve finalizar com \"{boleto.Banco.Codigo.ToString().PadLeft(3, '0')}\".");
                    }
                }
                else
                {
                    if (!boleto.NossoNumero.Substring(boleto.NossoNumero.Length - 5, 3).Equals(boleto.Banco.Codigo.ToString().PadLeft(3, '0')))
                    {
                        throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve finalizar com \"{boleto.Banco.Codigo.ToString().PadLeft(3, '0')}\".");
                    }
                }
            }

            var beneficiario = boleto.Banco.Beneficiario;
            var codBeneficiario = (beneficiario.Codigo + beneficiario.CodigoDV ?? "").PadLeft(10, '0');
            var CampoLivreSemDv = $"{incrementoCampoLivreNossoNumero}{codBeneficiario}{boleto.NossoNumero}";
            string dv1 = CampoLivreSemDv.CalcularDVMod10BRB();
            var aux = $"{CampoLivreSemDv}{dv1}";
            string dv2 = aux.CalcularDVMod11BRB(ref dv1);

            boleto.NossoNumeroDV = $"{dv1}{dv2}";
            boleto.NossoNumeroFormatado = $"{boleto.NossoNumero}{boleto.NossoNumeroDV}";
        }

        public string FormataCodigoBarraCampoLivre(Boleto boleto)
        {
            var beneficiario = boleto.Banco.Beneficiario;
            var codBeneficiario = (beneficiario.Codigo + beneficiario.CodigoDV ?? "").PadLeft(10, '0');
            return $"{incrementoCampoLivreNossoNumero}{codBeneficiario}{boleto.NossoNumero}{boleto.NossoNumeroDV}";
        }
    }
}
