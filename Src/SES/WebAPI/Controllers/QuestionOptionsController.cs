using Application.Features.QuestionOptions.Commands.Create;
using Application.Features.QuestionOptions.Commands.Delete;
using Application.Features.QuestionOptions.Commands.Update;
using Application.Features.QuestionOptions.Queries.GetById;
using Application.Features.QuestionOptions.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionOptionsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedQuestionOptionResponse>> Add([FromBody] CreateQuestionOptionCommand command)
    {
        CreatedQuestionOptionResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedQuestionOptionResponse>> Update([FromBody] UpdateQuestionOptionCommand command)
    {
        UpdatedQuestionOptionResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedQuestionOptionResponse>> Delete([FromRoute] Guid id)
    {
        DeleteQuestionOptionCommand command = new() { Id = id };

        DeletedQuestionOptionResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdQuestionOptionResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdQuestionOptionQuery query = new() { Id = id };

        GetByIdQuestionOptionResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListQuestionOptionQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListQuestionOptionQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListQuestionOptionListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}