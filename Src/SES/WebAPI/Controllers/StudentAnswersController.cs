using Application.Features.StudentAnswers.Commands.Create;
using Application.Features.StudentAnswers.Commands.Delete;
using Application.Features.StudentAnswers.Commands.Update;
using Application.Features.StudentAnswers.Queries.GetById;
using Application.Features.StudentAnswers.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentAnswersController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedStudentAnswerResponse>> Add([FromBody] CreateStudentAnswerCommand command)
    {
        CreatedStudentAnswerResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedStudentAnswerResponse>> Update([FromBody] UpdateStudentAnswerCommand command)
    {
        UpdatedStudentAnswerResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedStudentAnswerResponse>> Delete([FromRoute] Guid id)
    {
        DeleteStudentAnswerCommand command = new() { Id = id };

        DeletedStudentAnswerResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdStudentAnswerResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdStudentAnswerQuery query = new() { Id = id };

        GetByIdStudentAnswerResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListStudentAnswerQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListStudentAnswerQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListStudentAnswerListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}