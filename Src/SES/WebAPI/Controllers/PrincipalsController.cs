using Application.Features.Principals.Commands.Create;
using Application.Features.Principals.Commands.Delete;
using Application.Features.Principals.Commands.Update;
using Application.Features.Principals.Queries.GetById;
using Application.Features.Principals.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrincipalsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedPrincipalResponse>> Add([FromBody] CreatePrincipalCommand command)
    {
        CreatedPrincipalResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedPrincipalResponse>> Update([FromBody] UpdatePrincipalCommand command)
    {
        UpdatedPrincipalResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedPrincipalResponse>> Delete([FromRoute] int id)
    {
        DeletePrincipalCommand command = new() { Id = id };

        DeletedPrincipalResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdPrincipalResponse>> GetById([FromRoute] int id)
    {
        GetByIdPrincipalQuery query = new() { Id = id };

        GetByIdPrincipalResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListPrincipalQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListPrincipalQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListPrincipalListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}