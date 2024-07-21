using Application.Features.Lessons.Commands.Create;
using Application.Features.Lessons.Commands.Delete;
using Application.Features.Lessons.Commands.Update;
using Application.Features.Lessons.Queries.GetById;
using Application.Features.Lessons.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedLessonResponse>> Add([FromBody] CreateLessonCommand command)
    {
        CreatedLessonResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedLessonResponse>> Update([FromBody] UpdateLessonCommand command)
    {
        UpdatedLessonResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedLessonResponse>> Delete([FromRoute] int id)
    {
        DeleteLessonCommand command = new() { Id = id };

        DeletedLessonResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdLessonResponse>> GetById([FromRoute] int id)
    {
        GetByIdLessonQuery query = new() { Id = id };

        GetByIdLessonResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListLessonQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListLessonQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListLessonListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}