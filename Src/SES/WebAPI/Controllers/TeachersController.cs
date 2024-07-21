using Application.Features.Teachers.Commands.Create;
using Application.Features.Teachers.Commands.Delete;
using Application.Features.Teachers.Commands.Update;
using Application.Features.Teachers.Queries.GetById;
using Application.Features.Teachers.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedTeacherResponse>> Add([FromBody] CreateTeacherCommand command)
    {
        CreatedTeacherResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedTeacherResponse>> Update([FromBody] UpdateTeacherCommand command)
    {
        UpdatedTeacherResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedTeacherResponse>> Delete([FromRoute] int id)
    {
        DeleteTeacherCommand command = new() { Id = id };

        DeletedTeacherResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdTeacherResponse>> GetById([FromRoute] int id)
    {
        GetByIdTeacherQuery query = new() { Id = id };

        GetByIdTeacherResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListTeacherQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListTeacherQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListTeacherListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}