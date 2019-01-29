using DataModels;
using Inspur.TaxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Commom
{
    public class EntityAdapter
    {
        public static TaxPayer TaxpayerJnfo2TaxPayer(TaxpayerJnfo taxpayerJnfo)
        {
            TaxPayer taxPayer = new TaxPayer
            {
                Tin = taxpayerJnfo.TaxpayerTin,
                Id = taxpayerJnfo.TaxpayerId.ToString(),
                Name = taxpayerJnfo.ShopName,
                Address = taxpayerJnfo.TaxpayerAddress,
                Telphone = taxpayerJnfo.TaxpayerTel,
                BankAccount = taxpayerJnfo.TaxpayerAccount,
            };
            return taxPayer;
        }
        public static void TaxpayerJnfo2TaxPayer(TaxpayerJnfo source, TaxPayer target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source can not null.");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target can not null.");
            }
            target.Tin = source.TaxpayerTin;
            target.Id = source.TaxpayerId.ToString();
            target.Name = source.ShopName;
            target.Address = source.TaxpayerAddress;
            target.Telphone = source.TaxpayerTel;
            target.BankAccount = source.TaxpayerAccount;
        }
        public static TaxpayerJnfo TaxPayer2TaxpayerJnfo(TaxPayer taxPayer)
        {
            TaxpayerJnfo taxpayerJnfo = new TaxpayerJnfo
            {
                TaxpayerTin = taxPayer.Tin,
                ShopName = taxPayer.Name,
                TaxpayerAddress = taxPayer.Address,
                TaxpayerTel = taxPayer.Telphone,
                TaxpayerAccount = taxPayer.BankAccount,
            };
            return taxpayerJnfo;
        }
        public static void BuyerInfo2Buyer(BuyerInfo source, Buyer target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source can not null.");
            }
            if (target == null)
            {
                target = new Buyer();
            }
            target.Id = source.BuyerId.ToString();
            target.Tin = source.BuyerTin;
            target.Name = source.BuyerName;
            target.Address = source.BuyerAddress;
            target.TelPhone = source.BuyerTel;
        }
        public static void GoodsInfo2ProductItem(GoodsInfo source, ProductItem target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source can not null.");
            }
            if (target == null)
            {
                target = new ProductItem();
            }
            target.No = source.GoodsId;
            target.BarCode = source.Barcode;
            target.Name = source.Description;
            target.Price = source.Price == null ? 0 : source.Price.Value;
            //target.Count = source.Quantity == null ? 0 : source.Quantity.Value;
            target.TaxInclusive = source.TaxInclusive;
        }
        public static void CodeTaxtype2TaxType(CodeTaxtype source, TaxType target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source can not null.");
            }
            if (target == null)
            {
                target = new TaxType();
            }
            target.Id = source.TaxtypeId == null ? 0 : source.TaxtypeId.Value;
            target.FixTaxAmount = source.FixedTaxAmount == null ? 0 : source.FixedTaxAmount.Value;
            target.CalculationMode = source.CalculationMode;
            target.Rate = source.TaxRate == null ? 0 : source.TaxRate.Value;
            target.Label = source.TaxItemLable;
            target.Name = source.TaxTypeCode;
        }
    }
}
