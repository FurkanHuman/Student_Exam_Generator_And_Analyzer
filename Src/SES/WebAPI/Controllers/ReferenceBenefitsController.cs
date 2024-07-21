using Application.Features.ReferenceBenefits.Commands.Create;
using Application.Features.ReferenceBenefits.Commands.Delete;
using Application.Features.ReferenceBenefits.Commands.Update;
using Application.Features.ReferenceBenefits.Queries.GetById;
using Application.Features.ReferenceBenefits.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReferenceBenefitsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedReferenceBenefitResponse>> Add([FromBody] CreateReferenceBenefitCommand command)
    {
        CreatedReferenceBenefitResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedReferenceBenefitResponse>> Update([FromBody] UpdateReferenceBenefitCommand command)
    {
        UpdatedReferenceBenefitResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedReferenceBenefitResponse>> Delete([FromRoute] int id)
    {
        DeleteReferenceBenefitCommand command = new() { Id = id };

        DeletedReferenceBenefitResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdReferenceBenefitResponse>> GetById([FromRoute] int id)
    {
        GetByIdReferenceBenefitQuery query = new() { Id = id };

        GetByIdReferenceBenefitResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListReferenceBenefitQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListReferenceBenefitQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListReferenceBenefitListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}