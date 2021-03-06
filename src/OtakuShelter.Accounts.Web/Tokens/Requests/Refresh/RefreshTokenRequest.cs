using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace OtakuShelter.Accounts
{
	[DataContract]
	public class RefreshTokenRequest
	{
		[DataMember(Name = "refreshToken")]
		public string RefreshToken { get; set; }
		
		public async ValueTask<TokenRequest> Refresh(AccountsContext context, AccountsJwtConfiguration configuration, HttpContext httpContext)
		{
			var tokenToRemove = await context.Tokens
				.Include(t => t.Account)
				.FirstAsync(t => t.RefreshToken == RefreshToken);

			var account = tokenToRemove.Account;
			
			var tokenHandler = new JwtSecurityTokenHandler();
			
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new [] 
				{
					new Claim(ClaimTypes.Name, account.Id.ToString()),
					new Claim(ClaimTypes.Role, account.Role)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				Issuer = configuration.Issuer,
				Audience = configuration.Audience,
				SigningCredentials = new SigningCredentials(
					configuration.SymmetricSecurityKey,
					SecurityAlgorithms.HmacSha256Signature)
			};

			var access = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

			var data = new byte[100];
			new Random().NextBytes(data);

			var refresh = Convert.ToBase64String(data);
			
			var token = new Token
			{
				Account = account,
				Created = DateTime.UtcNow,
				IpAddress = httpContext.Connection.RemoteIpAddress.ToString(),
				UserAgent = httpContext.Request.Headers[HeaderNames.UserAgent],
				RefreshToken = refresh
			};

			await context.Tokens.AddAsync(token);
			context.Tokens.Remove(tokenToRemove);
			
			return new TokenRequest(access, refresh);
		}
	}
}