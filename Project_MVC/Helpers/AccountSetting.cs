using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Demo.PL.Helpers
{
	public static class AccountSetting
	{
		public static  void SendEmailToResetPassword(Email email)
		{
			var smtp = new SmtpClient("smtp.gmail.com", 587);
			smtp.EnableSsl = true;
			smtp.Credentials = new NetworkCredential("sabreenmagdy973@gmail.com", "fravhyrpmiekoenz");
			smtp.Send("sabreenmagdy973@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
