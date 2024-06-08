using Application.Features.QuestionScores.Commands.Create;
using Application.Features.QuestionScores.Commands.Delete;
using Application.Features.QuestionScores.Commands.Update;
using Application.Features.QuestionScores.Queries.GetById;
using Application.Features.QuestionScores.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionScoresController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedQuestionScoreResponse>> Add([FromBody] CreateQuestionScoreCommand command)
    {
        CreatedQuestionScoreResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedQuestionScoreResponse>> Update([FromBody] UpdateQuestionScoreCommand command)
    {
        UpdatedQuestionScoreResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedQuestionScoreResponse>> Delete([FromRoute] int id)
    {
        DeleteQuestionScoreCommand command = new() { Id = id };

        DeletedQuestionScoreResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdQuestionScoreResponse>> GetById([FromRoute] int id)
    {
        GetByIdQuestionScoreQuery query = new() { Id = id };

        GetByIdQuestionScoreResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListQuestionScoreQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListQuestionScoreQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListQuestionScoreListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}