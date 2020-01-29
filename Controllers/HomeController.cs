using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContactsCoreMVC.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;

namespace ContactsCoreMVC.Controllers
{
  public class HomeController : Controller
  {
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> Vault()
    {
      AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
      KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
      var secrets = await keyVaultClient.GetSecretsAsync(_configuration["MNKeyVault"]);

      Dictionary<string, string> secretValueList = new Dictionary<string, string>();
      foreach (var item in secrets)
      {
        var secret = await keyVaultClient.GetSecretAsync(item.Id);
        secretValueList.Add(item.Id, secret.Value);
      }
      return View(secretValueList);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    public IActionResult About()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
