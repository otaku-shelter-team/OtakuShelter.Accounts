using Phema.Configuration;

namespace OtakuShelter.Accounts
{
	[Configuration]
	public class AccountsRabbitMqConfiguration
	{
		public string Hostname { get; set; }
		public string InstanceName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public int Port { get; set; }
		public string VirtualHost { get; set; }
		
		public string ConnectionString => $"amqp://{Username}:{Password}@{Hostname}:{Port}/{VirtualHost}";
	}
}