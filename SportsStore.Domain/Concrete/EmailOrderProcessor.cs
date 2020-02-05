using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entitiess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmptPassword";
        public string ServerName = "smpt.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"c:\sports_store_emails";
    }
    public class EmailOrderProcessor : IorderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }
        public void ProcessorOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using(var smptClient = new SmtpClient())
            {
                smptClient.EnableSsl = emailSettings.UseSsl;
                smptClient.Host = emailSettings.ServerName;
                smptClient.Port = emailSettings.ServerPort;
                smptClient.UseDefaultCredentials = false;
                smptClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if(emailSettings.WriteAsFile)
                {
                    smptClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smptClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smptClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder().AppendLine("A new order has been submitted").AppendLine("---").AppendLine("Items");
                foreach(var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal : {2:c})", line.Quantity, line.Product.Name, subtotal);

                }

                body.AppendFormat("Total order value :{0:c}", cart.ComputeTotalValue()).AppendLine("---").AppendLine("Ship to:").AppendLine(shippingDetails.Name).AppendLine(shippingDetails.Line1).AppendLine(shippingDetails.Line2 ?? "").AppendLine(shippingDetails.Line3 ?? "").AppendLine(shippingDetails.City).AppendLine(shippingDetails.State ?? "").AppendLine(shippingDetails.Country).AppendLine(shippingDetails.Zip).AppendLine("---").AppendFormat("Gift wrap :{0}", shippingDetails.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(emailSettings.MailFromAddress, emailSettings.MailToAddress, "New order submitted!", body.ToString());
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smptClient.Send(mailMessage);

            }
        }
    }
}
