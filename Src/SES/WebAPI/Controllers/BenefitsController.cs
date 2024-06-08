using Application.Features.Benefits.Commands.Create;
using Application.Features.Benefits.Commands.Delete;
using Application.Features.Benefits.Commands.Update;
using Application.Features.Benefits.Queries.GetById;
using Application.Features.Benefits.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BenefitsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedBenefitResponse>> Add([FromBody] CreateBenefitCommand command)
    {
        CreatedBenefitResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedBenefitResponse>> Update([FromBody] UpdateBenefitCommand command)
    {
        UpdatedBenefitResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedBenefitResponse>> Delete([FromRoute] int id)
    {
        DeleteBenefitCommand command = new() { Id = id };

        DeletedBenefitResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdBenefitResponse>> GetById([FromRoute] int id)
    {
        GetByIdBenefitQuery query = new() { Id = id };

        GetByIdBenefitResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListBenefitQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListBenefitQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListBenefitListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}