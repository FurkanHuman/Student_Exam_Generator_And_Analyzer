using Application.Features.SubLearningAreas.Commands.Create;
using Application.Features.SubLearningAreas.Commands.Delete;
using Application.Features.SubLearningAreas.Commands.Update;
using Application.Features.SubLearningAreas.Queries.GetById;
using Application.Features.SubLearningAreas.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubLearningAreasController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedSubLearningAreaResponse>> Add([FromBody] CreateSubLearningAreaCommand command)
    {
        CreatedSubLearningAreaResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedSubLearningAreaResponse>> Update([FromBody] UpdateSubLearningAreaCommand command)
    {
        UpdatedSubLearningAreaResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedSubLearningAreaResponse>> Delete([FromRoute] int id)
    {
        DeleteSubLearningAreaCommand command = new() { Id = id };

        DeletedSubLearningAreaResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdSubLearningAreaResponse>> GetById([FromRoute] int id)
    {
        GetByIdSubLearningAreaQuery query = new() { Id = id };

        GetByIdSubLearningAreaResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListSubLearningAreaQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListSubLearningAreaQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListSubLearningAreaListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}