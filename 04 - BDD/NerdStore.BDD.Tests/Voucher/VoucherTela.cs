using NerdStore.BDD.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.BDD.Tests.Voucher
{
    public class VoucherTela : PageObjectModel
    {
        public VoucherTela(SeleniumHelper helper) : base(helper)
        { }


        public void PreecnherCodigoDoVoucher()
        {
            Helper.PreencherTextBoxPorId("VoucherCodigo", "PROMO-15-REAIS");
        }
        public void ClicarBotaoAplicarVoucher()
        {
            Helper.ClicarPorXPath("/html/body/div/main/div/div/div/table/tbody/tr[3]/td[2]/form/div[2]/button");
        }

        public decimal ObterValorTotal()
        {
            return Convert.ToDecimal(Helper.ObterTextoElementoPorId("valorTotal")
                         .Replace("R$", string.Empty).Replace(",", string.Empty).Trim());
            
        }
        public decimal ObterValorTotalCarrinho()
        {
            return Convert.ToDecimal(Helper.ObterTextoElementoPorId("valorTotalCarrinho")
                        .Replace("R$", string.Empty).Replace(",", string.Empty).Trim());
        }

        public decimal ObterValorDescontoVoucher()
        {
           return Convert.ToDecimal(Helper.ObterElementoPorXPath("/html/body/div/main/div/div/div/table/tbody/tr[3]/td[4]/h5").Text
                        .Replace("R$", string.Empty).Replace(",", string.Empty).Trim());
        }

    }
}
