using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class GiftCard : WalletPass
    {
        public string CurrencyCode  { get; set; }
        public string IssuerName    { get; set; }
        public string RecipientName { get; set; }
        public string SenderName    { get; set; }
        public string Message       { get; set; }
        public string CardNumber    { get; set; }
    }

}
