using Application.Features.Students.Commands.Create;
using Application.Features.Students.Commands.Delete;
using Application.Features.Students.Commands.Update;
using Application.Features.Students.Queries.GetById;
using Application.Features.Students.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedStudentResponse>> Add([FromBody] CreateStudentCommand command)
    {
        CreatedStudentResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedStudentResponse>> Update([FromBody] UpdateStudentCommand command)
    {
        UpdatedStudentResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedStudentResponse>> Delete([FromRoute] int id)
    {
        DeleteStudentCommand command = new() { Id = id };

        DeletedStudentResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdStudentResponse>> GetById([FromRoute] int id)
    {
        GetByIdStudentQuery query = new() { Id = id };

        GetByIdStudentResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListStudentQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListStudentQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListStudentListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}