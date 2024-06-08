using Application.Features.Exams.Commands.Create;
using Application.Features.Exams.Commands.Delete;
using Application.Features.Exams.Commands.Update;
using Application.Features.Exams.Queries.GetById;
using Application.Features.Exams.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExamsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedExamResponse>> Add([FromBody] CreateExamCommand command)
    {
        CreatedExamResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedExamResponse>> Update([FromBody] UpdateExamCommand command)
    {
        UpdatedExamResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedExamResponse>> Delete([FromRoute] int id)
    {
        DeleteExamCommand command = new() { Id = id };

        DeletedExamResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdExamResponse>> GetById([FromRoute] int id)
    {
        GetByIdExamQuery query = new() { Id = id };

        GetByIdExamResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListExamQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListExamQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListExamListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}