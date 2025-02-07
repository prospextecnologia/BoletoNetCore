using System;

namespace BoletoNetCore
{
    partial class BancoBRB : IBancoCNAB400
    {
        public string GerarDetalheRemessaCNAB400(Boleto boleto, ref int registro)
        {
            return GerarDetalheRemessaCNAB400Registro1(boleto, ref registro);
        }

        public string GerarHeaderRemessaCNAB400(ref int numeroArquivoRemessa, ref int numeroRegistroGeral, int numeroRegistrosRemessa = 0)
        {
            try
            {
                numeroRegistroGeral++;
                TRegistroEDI reg = new TRegistroEDI();
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 003, 0, "DCB", ' ');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 003, 0, "001", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0007, 003, 0, "077", '0');
                reg.Adicionar(TTiposDadoEDI.ediInteiro______________, 0010, 010, 0, $"{Beneficiario.ContaBancaria.Conta}{Beneficiario.ContaBancaria.DigitoConta ?? ""}", '0');
                reg.Adicionar(TTiposDadoEDI.ediDataAAAAMMDD_________, 0020, 008, 0, DateTime.Now, ' ');
                reg.Adicionar(TTiposDadoEDI.ediHoraHHMMSS___________, 0028, 006, 0, DateTime.Now, ' ');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0034, 006, 0, numeroRegistroGeral + numeroRegistrosRemessa, '0');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0040, 361, 0, " ", ' ');
                reg.CodificarLinha();
                return reg.LinhaRegistro;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarTrailerRemessaCNAB400(int numeroRegistroGeral, decimal valorBoletoGeral, int numeroRegistroCobrancaSimples, decimal valorCobrancaSimples, int numeroRegistroCobrancaVinculada, decimal valorCobrancaVinculada, int numeroRegistroCobrancaCaucionada, decimal valorCobrancaCaucionada, int numeroRegistroCobrancaDescontada, decimal valorCobrancaDescontada)
        {
            return null;
        }

        private string GerarDetalheRemessaCNAB400Registro1(Boleto boleto, ref int numeroRegistroGeral)
        {
            try
            {
                numeroRegistroGeral++;
                TRegistroEDI reg = new TRegistroEDI();
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 002, 0, "01", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0003, 003, 0, "000", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0006, 010, 0, $"{boleto.Banco.Beneficiario.ContaBancaria.Conta}{boleto.Banco.Beneficiario.ContaBancaria.DigitoConta ?? ""}", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0016, 012, 0, $"{boleto.NossoNumero}{boleto.NossoNumeroDV}", '0');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0028, 025, 0, boleto.NumeroControleParticipante, ' ');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0053, 045, 0, boleto.Pagador.Nome, ' ');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0098, 060, 0, boleto.Pagador.Endereco.FormataLogradouro(37), ' ');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0158, 015, 0, boleto.Pagador.Endereco.Bairro, ' ');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0173, 022, 0, boleto.Pagador.Endereco.Cidade, ' ');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0195, 002, 0, boleto.Pagador.Endereco.UF, ' ');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0197, 008, 0, boleto.Pagador.Endereco.CEP.Replace("-", ""), '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0205, 001, 0, boleto.Pagador.TipoCPFCNPJ("0"), '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 014, 0, boleto.Pagador.CPFCNPJ, '0');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0220, 004, 0, "", ' ');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0224, 001, 0, "0", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0225, 002, 0, AjustaEspecieCnab400(boleto.EspecieDocumento), '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0227, 002, 0, "02", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0229, 001, 0, "2", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0230, 004, 0, $"{boleto.Banco.Beneficiario.ContaBancaria.Agencia}", '0');
                reg.Adicionar(TTiposDadoEDI.ediDataDDMMAAAA_________, 0234, 008, 0, boleto.DataEmissao, ' ');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0242, 001, 0, "0", '0');
                reg.Adicionar(TTiposDadoEDI.ediDataDDMMAAAA_________, 0243, 008, 0, boleto.DataVencimento, ' ');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0251, 015, 2, boleto.ValorTitulo, '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0266, 027, 0, "0", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0293, 003, 0, "1", '0');
                if (boleto.PercentualJurosDia > 0)
                {
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0296, 002, 0, "22", '0');
                }
                else if (boleto.ValorJurosDia > 0)
                {
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0296, 002, 0, "12", '0');
                }
                else
                {
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0296, 002, 0, "00", '0');
                }
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0298, 010, 2, boleto.PercentualJurosDia > 0 ? boleto.PercentualJurosDia :
                    boleto.ValorJurosDia > 0 ? boleto.ValorJurosDia : 0, '0');

                if (boleto.ValorDesconto > 0)
                {
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0308, 001, 0, "1", '0');
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0309, 010, 2, boleto.ValorDesconto, '0');
                    reg.Adicionar(TTiposDadoEDI.ediDataDDMMAAAA_________, 0319, 008, 0, boleto.DataDesconto, '0');
                }
                else
                {
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0308, 001, 0, "1", '0');
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0309, 010, 0, "0", '0');
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0319, 008, 0, "0", '0');
                }
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 036, 0, "0", '0');
                if (boleto.PercentualMulta > 0 || boleto.ValorMulta > 0)
                {
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0363, 002, 0, boleto.PercentualMulta > 0 ? "03" : "02", '0');
                }
                else
                {
                    reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0363, 002, 0, "00", '0');
                }
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0365, 003, 0, "001", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0368, 010, 0, "0", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0378, 008, 2, boleto.PercentualMulta > 0 ? boleto.PercentualMulta : boleto.ValorMulta > 0 ? boleto.ValorMulta : 0, '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0386, 003, 0, "0", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0389, 002, 0, "07", '0');
                reg.Adicionar(TTiposDadoEDI.ediNumericoSemSeparador_, 0391, 001, 0, "0", '0');
                reg.Adicionar(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 009, 0, " ", ' ');
                reg.CodificarLinha();
                return reg.LinhaRegistro;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        private string OcorrenciasCnab400(string codigo)
        {
            switch (codigo)
            {
                case "00":
                    return "Baixa por devolução do títulos";
                case "02":
                    return "Confirmação de Entrada de Título";
                case "05":
                    return "Liquidação sem registro";
                case "06":
                    return "Liquidação normal";
                case "09":
                    return "Liquidação/Instrução Rejeitada";
                case "15":
                    return "Liquidação Regularizada S/Registro ou Modalidade";
                case "16":
                    return "Liquidação Regularizada C/Registro ou Modalidade 2 e 3";
                default:
                    return "";
            }
        }
        private TipoEspecieDocumento AjustaEspecieCnab400(string codigoEspecie)
        {
            switch (codigoEspecie)
            {
                case "21":
                    return TipoEspecieDocumento.DM;
                case "22":
                    return TipoEspecieDocumento.NP;
                case "25":
                    return TipoEspecieDocumento.RC;
                case "31":
                    return TipoEspecieDocumento.DS;
                default:
                    return TipoEspecieDocumento.OU;
            }
        }
        private string AjustaEspecieCnab400(TipoEspecieDocumento especieDocumento)
        {
            switch (especieDocumento)
            {
                case TipoEspecieDocumento.DM:
                    return "21";
                case TipoEspecieDocumento.DS:
                    return "22";
                case TipoEspecieDocumento.NP:
                    return "25";
                case TipoEspecieDocumento.CC:
                    return "31";
                case TipoEspecieDocumento.BP:
                    return "32";
                default:
                    return "39";
            }
        }

        public override void LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                if (registro.Substring(0, 9) != "02RETORNO")
                {
                    throw new Exception("O arquivo não é do tipo \"02RETORNO\"");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler HEADER do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public void LerDetalheRetornoCNAB400Segmento1(ref Boleto boleto, string registro)
        {
            try
            {
                //Nº Controle do Participante
                boleto.NumeroControleParticipante = registro.Substring(46, 25);

                //Carteira
                boleto.Carteira = "COB";
                boleto.TipoCarteira = TipoCarteira.CarteiraCobrancaSimples;

                boleto.NossoNumero = registro.Substring(71, 10); //Sem o DV
                boleto.NossoNumeroDV = registro.Substring(81, 2); //DV
                boleto.NossoNumeroFormatado = $"{boleto.NossoNumero}-{boleto.NossoNumeroDV}";

                //Identificação de Ocorrência
                boleto.CodigoMovimentoRetorno = registro.Substring(109, 2);
                boleto.DescricaoMovimentoRetorno = OcorrenciasCnab400(boleto.CodigoMovimentoRetorno);
                boleto.CodigoMotivoOcorrencia = "";

                //Número do Documento
                boleto.NumeroDocumento = registro.Substring(46, 25);
                boleto.EspecieDocumento = AjustaEspecieCnab400(registro.Substring(178, 2));

                //Valores do Título
                boleto.ValorTitulo = Convert.ToDecimal(registro.Substring(157, 13)) / 100;
                boleto.ValorTarifas = (Convert.ToDecimal(registro.Substring(245, 13)) / 100);
                boleto.ValorIOF = Convert.ToDecimal(registro.Substring(219, 13)) / 100;
                boleto.ValorPago = Convert.ToDecimal(registro.Substring(258, 13)) / 100;
                // Data do Crédito
                boleto.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(119, 8)).ToString("##-##-####"));
                boleto.DataProcessamento = Utils.ToDateTime(Utils.ToInt32(registro.Substring(111, 8)).ToString("##-##-####"));
                boleto.ValorDesconto = Convert.ToDecimal(registro.Substring(245, 13)) / 100;
                boleto.ValorOutrasDespesas = (Convert.ToDecimal(registro.Substring(271, 13)) / 100);
                boleto.ValorOutrosCreditos = Convert.ToDecimal(registro.Substring(284, 13)) / 100;
                boleto.DataVencimento = Utils.ToDateTime(Utils.ToInt32(registro.Substring(249, 8)).ToString("##-##-####"));

                boleto.RegistroArquivoRetorno = boleto.RegistroArquivoRetorno + registro + Environment.NewLine;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public void LerDetalheRetornoCNAB400Segmento7(ref Boleto boleto, string registro)
        {

        }

        public void LerTrailerRetornoCNAB400(string registro)
        {
        }
    }
}
