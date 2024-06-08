using Application.Features.Semesters.Commands.Create;
using Application.Features.Semesters.Commands.Delete;
using Application.Features.Semesters.Commands.Update;
using Application.Features.Semesters.Queries.GetById;
using Application.Features.Semesters.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SemestersController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedSemesterResponse>> Add([FromBody] CreateSemesterCommand command)
    {
        CreatedSemesterResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedSemesterResponse>> Update([FromBody] UpdateSemesterCommand command)
    {
        UpdatedSemesterResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedSemesterResponse>> Delete([FromRoute] int id)
    {
        DeleteSemesterCommand command = new() { Id = id };

        DeletedSemesterResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdSemesterResponse>> GetById([FromRoute] int id)
    {
        GetByIdSemesterQuery query = new() { Id = id };

        GetByIdSemesterResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListSemesterQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListSemesterQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListSemesterListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}