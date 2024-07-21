using Application.Features.StudentClasses.Commands.Create;
using Application.Features.StudentClasses.Commands.Delete;
using Application.Features.StudentClasses.Commands.Update;
using Application.Features.StudentClasses.Queries.GetById;
using Application.Features.StudentClasses.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentClassesController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedStudentClassResponse>> Add([FromBody] CreateStudentClassCommand command)
    {
        CreatedStudentClassResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedStudentClassResponse>> Update([FromBody] UpdateStudentClassCommand command)
    {
        UpdatedStudentClassResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedStudentClassResponse>> Delete([FromRoute] int id)
    {
        DeleteStudentClassCommand command = new() { Id = id };

        DeletedStudentClassResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdStudentClassResponse>> GetById([FromRoute] int id)
    {
        GetByIdStudentClassQuery query = new() { Id = id };

        GetByIdStudentClassResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListStudentClassQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListStudentClassQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListStudentClassListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}