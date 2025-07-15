using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ChatMessageModel
    {
        public FAQQuestion FAQQuestion { get; set; }
        public object ChatWithAi { get; set; }
    }

    public class FAQQuestion
    {
        public List<FAQItem> BiddingProcess { get; set; }
        public List<FAQItem> AccountAndSecurity { get; set; }
        public List<FAQItem> SellingItems { get; set; }
        public List<FAQItem> Payments { get; set; }
        public List<FAQItem> ShippingAndDelivery { get; set; }
    }

    public class FAQItem
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }


}
