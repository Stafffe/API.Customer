using API.Customer.Business.Interfaces;
using API.Customer.Web.DTOs;
using API.Customer.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace API.Customer.Controllers
{
  //[Authorize] //Should be added for a production API
  [ApiController]
  [Route("[controller]")]
  public class CustomerController : ControllerBase
  {

    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerBusiness _customerBusiness;
    private readonly ICustomerMapper _mapper;

    public CustomerController(ILogger<CustomerController> logger, ICustomerBusiness customerBusiness, ICustomerMapper mapper)
    {
      _logger = logger;
      _customerBusiness = customerBusiness;
      _mapper = mapper;
    }

    [HttpGet("/officialId")] //This should be encrypted in a production API
    public async Task<ActionResult> Get(string officialId)
    {
      try
      {
        var result = await _customerBusiness.GetCustomerInformation(officialId);

        var customerInformation = _mapper.Map(result);
        return Ok(customerInformation);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Something went wrong trying to get customer information.");
        return Ok(500);
      }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CustomerInformation updateCustomerInfo)
    {
      try
      {
        var internalCustomerInfo = _mapper.Map(updateCustomerInfo);
        var result = await _customerBusiness.CreateCustomer(internalCustomerInfo);

        var customerInformation = _mapper.Map(result);
        return Ok(customerInformation);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Something went wrong trying to create customer information.");
        return Ok(500);
      }
    }

    [HttpPut] 
    public async Task<ActionResult> Update([FromBody] CustomerInformation updateCustomerInfo)
    {
      try
      {
        var internalCustomerInfo = _mapper.Map(updateCustomerInfo);
        var result = await _customerBusiness.UpdateCustomer(internalCustomerInfo);

        var customerInformation = _mapper.Map(result);
        return Ok(customerInformation);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Something went wrong trying to update customer information.");
        return Ok(500);
      }
    }

    [HttpDelete("/officialId")] //Same here, this should be encrypted                               
    public async Task<ActionResult> Delete(string officialId)
    {
      try
      {
        await _customerBusiness.DeleteCustomer(officialId);
        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Something went wrong trying to create customer information.");
        return Ok(500);
      }
    }
  }
}
