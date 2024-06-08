using Application.Features.QuizQuestions.Commands.Create;
using Application.Features.QuizQuestions.Commands.Delete;
using Application.Features.QuizQuestions.Commands.Update;
using Application.Features.QuizQuestions.Queries.GetById;
using Application.Features.QuizQuestions.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizQuestionsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedQuizQuestionResponse>> Add([FromBody] CreateQuizQuestionCommand command)
    {
        CreatedQuizQuestionResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedQuizQuestionResponse>> Update([FromBody] UpdateQuizQuestionCommand command)
    {
        UpdatedQuizQuestionResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedQuizQuestionResponse>> Delete([FromRoute] int id)
    {
        DeleteQuizQuestionCommand command = new() { Id = id };

        DeletedQuizQuestionResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdQuizQuestionResponse>> GetById([FromRoute] int id)
    {
        GetByIdQuizQuestionQuery query = new() { Id = id };

        GetByIdQuizQuestionResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListQuizQuestionQuery>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListQuizQuestionQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListQuizQuestionListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}