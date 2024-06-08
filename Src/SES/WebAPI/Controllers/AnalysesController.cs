using Application.Features.Analyses.Commands.Create;
using Application.Features.Analyses.Commands.Delete;
using Application.Features.Analyses.Commands.Update;
using Application.Features.Analyses.Queries.GetById;
using Application.Features.Analyses.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnalysesController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedAnalysisResponse>> Add([FromBody] CreateAnalysisCommand command)
    {
        CreatedAnalysisResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedAnalysisResponse>> Update([FromBody] UpdateAnalysisCommand command)
    {
        UpdatedAnalysisResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedAnalysisResponse>> Delete([FromRoute] int id)
    {
        DeleteAnalysisCommand command = new() { Id = id };

        DeletedAnalysisResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdAnalysisResponse>> GetById([FromRoute] int id)
    {
        GetByIdAnalysisQuery query = new() { Id = id };

        GetByIdAnalysisResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListAnalysisQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListAnalysisQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListAnalysisListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}