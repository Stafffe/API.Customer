using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace API.Customer.Web.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AuthorizationController : Controller
  {

    [HttpGet]
    public string Get()
    {
      return GetAccessToken();
    }

    private string GetAccessToken()
    {
      // get the certificate
      //var files = Directory.GetFiles(@"\");
      var certificate = new X509Certificate2(@"API.Customer.pfx", "Password");

      // create a header
      var header = new { alg = "RS256" };

      // create a claimset
      var expiryDate = GetExpiryDate();
      var claimset = new
      {
        typ= "JWT",
        //alg= "RS256",
        //x5t= "iBjL1Rcqzhiy4fpxIxdZqohM2Yk",
        //kid= "iBjL1Rcqzhiy4fpxIxdZqohM2Yk",
        //iss = "xxxxx",
        //prn = "xxxxx",
        aud = "11111111-1111-1111-11111111111111111",
        exp = expiryDate
      };

    // encoded header
    var headerSerialized = JsonConvert.SerializeObject(header);
    var headerBytes = Encoding.UTF8.GetBytes(headerSerialized);
    var headerEncoded = ToBase64UrlString(headerBytes);

    // encoded claimset
    var claimsetSerialized = JsonConvert.SerializeObject(claimset);
    var claimsetBytes = Encoding.UTF8.GetBytes(claimsetSerialized);
    var claimsetEncoded = ToBase64UrlString(claimsetBytes);

    // input
    var input = headerEncoded + "." + claimsetEncoded;
    //var inputBytes = Encoding.UTF8.GetBytes(input);

    var signingCredentials = new X509SigningCredentials(certificate, "RS256");
    var signature = JwtTokenUtilities.CreateEncodedSignature(input, signingCredentials);

    // jwt
    var jwt = headerEncoded + "." + claimsetEncoded + "." + signature;

      return jwt;
    }

  static int GetExpiryDate()
  {
    var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    var currentUtcTime = DateTime.UtcNow;

    var exp = (int)currentUtcTime.AddMinutes(4).Subtract(utc0).TotalSeconds;

    return exp;
  }

  static string ToBase64UrlString(byte[] input)
  {
    return Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
  }
}
}
