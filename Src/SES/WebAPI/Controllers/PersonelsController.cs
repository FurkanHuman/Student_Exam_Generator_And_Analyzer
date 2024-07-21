using Application.Features.Personels.Commands.Create;
using Application.Features.Personels.Commands.Delete;
using Application.Features.Personels.Commands.Update;
using Application.Features.Personels.Queries.GetById;
using Application.Features.Personels.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonelsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedPersonelResponse>> Add([FromBody] CreatePersonelCommand command)
    {
        CreatedPersonelResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedPersonelResponse>> Update([FromBody] UpdatePersonelCommand command)
    {
        UpdatedPersonelResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedPersonelResponse>> Delete([FromRoute] Guid id)
    {
        DeletePersonelCommand command = new() { Id = id };

        DeletedPersonelResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdPersonelResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdPersonelQuery query = new() { Id = id };

        GetByIdPersonelResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListPersonelQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListPersonelQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListPersonelListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}