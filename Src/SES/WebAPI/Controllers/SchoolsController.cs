using Application.Features.Schools.Commands.Create;
using Application.Features.Schools.Commands.Delete;
using Application.Features.Schools.Commands.Update;
using Application.Features.Schools.Queries.GetById;
using Application.Features.Schools.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SchoolsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedSchoolResponse>> Add([FromBody] CreateSchoolCommand command)
    {
        CreatedSchoolResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedSchoolResponse>> Update([FromBody] UpdateSchoolCommand command)
    {
        UpdatedSchoolResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedSchoolResponse>> Delete([FromRoute] int id)
    {
        DeleteSchoolCommand command = new() { Id = id };

        DeletedSchoolResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdSchoolResponse>> GetById([FromRoute] int id)
    {
        GetByIdSchoolQuery query = new() { Id = id };

        GetByIdSchoolResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListSchoolQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListSchoolQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListSchoolListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}