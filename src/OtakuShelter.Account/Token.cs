using System;

namespace OtakuShelter.Account
{
	public class Token
	{
		public int Id { get; set; }
		public string RefreshToken { get; set; }
		public string IpAddress { get; set; }
		public DateTime DateTime { get; set; }

		public int AccountId { get; set; }
		public virtual Account Account { get; set; }
	}
}