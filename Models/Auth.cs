using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using MySqlX.XDevAPI;

namespace WebApp.Models
{
	public class Auth
	{
		public static int SignIn(string login, string pass)
		{
			var user = UserContext.GetUser(login);
			if (user == null)
				return -1;
			
			var saltPass = GetHashMD5(pass + user.Salt);
			if (saltPass != user.Pass)
				return -2;

			StartAuthSession(user);
			return 1;
		}
		public static int SignUp(string login, string pass, string name, string Role)
		{
			var salt = GenSalt(32);
			pass = GetHashMD5(pass + salt);
			if (!UserContext.Create(login, pass, name, Role, salt))
				return -1;
			return 1;
		}
		public static bool LogOut()
		{
			EndAuthSession();
			return true;
		}
		private static string GetHashMD5(string input)
		{
			var md5 = MD5.Create();
			var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
 
			return Convert.ToBase64String(hash);
		}
		private static string GenSalt(int length)
		{
			RNGCryptoServiceProvider p = new RNGCryptoServiceProvider();
			var salt = new byte[length];
			p.GetBytes(salt);
			return Convert.ToBase64String(salt);
		}
		private static string GetHashSHA1(string input)
		{
			var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
			return string.Concat(hash.Select(b => b.ToString("x2")));
		}
		private static void StartAuthSession(User user)
		{
			HttpContext.Current.Session["User"] = user;
		}
		private static void EndAuthSession()
		{
			HttpContext.Current.Session.Remove("User");
		}
		public static User GetUserSession() => (User) HttpContext.Current.Session["User"];
	}
}