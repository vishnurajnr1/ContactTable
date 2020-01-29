using System.Collections.Generic;
using System.Threading.Tasks;
using ContactsCoreMVC.Models.Abstract;
using ContactsCoreMVC.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContactsCoreMVC.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ContactApiController : ControllerBase
  {
    private readonly IContactRepository _contactRepository;
    private readonly ILogger<ContactApiController> _logger;

    public ContactApiController(IContactRepository contactRepository, ILogger<ContactApiController> logger)
    {
      _contactRepository = contactRepository;
      _logger = logger;
    }

    [Route("/contacts")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactTable>>> Get()
    {
      return await _contactRepository.GetAllContactsAsync();
    }
  }
}