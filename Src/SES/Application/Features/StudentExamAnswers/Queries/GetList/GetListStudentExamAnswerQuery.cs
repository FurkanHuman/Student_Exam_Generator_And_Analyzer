using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;

namespace Application.Features.StudentExamAnswers.Queries.GetList;

public class GetListStudentExamAnswerQuery : IRequest<GetListResponse<GetListStudentExamAnswerListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public class GetListStudentExamAnswerQueryHandler : IRequestHandler<GetListStudentExamAnswerQuery, GetListResponse<GetListStudentExamAnswerListItemDto>>
    {
        private readonly IStudentExamAnswerRepository _studentExamAnswerRepository;
        private readonly IMapper _mapper;

        public GetListStudentExamAnswerQueryHandler(IStudentExamAnswerRepository studentExamAnswerRepository, IMapper mapper)
        {
            _studentExamAnswerRepository = studentExamAnswerRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListStudentExamAnswerListItemDto>> Handle(GetListStudentExamAnswerQuery request, CancellationToken cancellationToken)
        {
            IPaginate<StudentExamAnswer> studentExamAnswers = await _studentExamAnswerRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListStudentExamAnswerListItemDto> response = _mapper.Map<GetListResponse<GetListStudentExamAnswerListItemDto>>(studentExamAnswers);
            return response;
        }
    }
}