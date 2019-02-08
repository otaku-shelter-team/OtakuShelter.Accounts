using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace OtakuShelter.Account
{
	[DataContract]
	public class CreateTokenViewModel
	{
		[DataMember(Name = "username")]
		public string Username { get; set; }

		[DataMember(Name = "password")]
		public string Password { get; set; }

		public async Task<TokenViewModel> Create(AccountContext context,
			IPasswordHasher<Account> hasher,
			AccountWebConfiguration configuration,
			HttpContext httpContext)
		{
			var account = await context.Accounts.FirstAsync(a => a.Username == Username);

			var result = hasher.VerifyHashedPassword(account, account.PasswordHash, Password);

			if (result != PasswordVerificationResult.Success)
				throw new UnauthorizedAccessException();
			
			var secret = Encoding.ASCII.GetBytes(configuration.Secret);
			var tokenHandler = new JwtSecurityTokenHandler();
			
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new [] 
				{
					new Claim(ClaimTypes.Name, account.Id.ToString()),
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				Issuer = configuration.Issuer,
				Audience = configuration.Audience,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
			};

			var access = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

			var refresh = string.Join("",
				Enumerable.Repeat(0, 5)
					.Select(_ => new Guid().ToString("N")));
			
			var token = new Token
			{
				Account = account,
				DateTime = DateTime.Now,
				IpAddress = httpContext.Connection.RemoteIpAddress.ToString(),
				RefreshToken = refresh
			};

			await context.Tokens.AddAsync(token);
			
			return new TokenViewModel(access, refresh);
		}
	}
}