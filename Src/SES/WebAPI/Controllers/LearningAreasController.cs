using Application.Features.LearningAreas.Commands.Create;
using Application.Features.LearningAreas.Commands.Delete;
using Application.Features.LearningAreas.Commands.Update;
using Application.Features.LearningAreas.Queries.GetById;
using Application.Features.LearningAreas.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LearningAreasController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedLearningAreaResponse>> Add([FromBody] CreateLearningAreaCommand command)
    {
        CreatedLearningAreaResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedLearningAreaResponse>> Update([FromBody] UpdateLearningAreaCommand command)
    {
        UpdatedLearningAreaResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedLearningAreaResponse>> Delete([FromRoute] int id)
    {
        DeleteLearningAreaCommand command = new() { Id = id };

        DeletedLearningAreaResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdLearningAreaResponse>> GetById([FromRoute] int id)
    {
        GetByIdLearningAreaQuery query = new() { Id = id };

        GetByIdLearningAreaResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListLearningAreaQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListLearningAreaQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListLearningAreaListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}