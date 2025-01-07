using Application.Features.Semesters.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Semesters.Constants.SemestersOperationClaims;

namespace Application.Features.Semesters.Queries.GetById;

public class GetByIdSemesterQuery : IRequest<GetByIdSemesterResponse>
{
    public int Id { get; set; }

    

    public class GetByIdSemesterQueryHandler : IRequestHandler<GetByIdSemesterQuery, GetByIdSemesterResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISemesterRepository _semesterRepository;
        private readonly SemesterBusinessRules _semesterBusinessRules;

        public GetByIdSemesterQueryHandler(IMapper mapper, ISemesterRepository semesterRepository, SemesterBusinessRules semesterBusinessRules)
        {
            _mapper = mapper;
            _semesterRepository = semesterRepository;
            _semesterBusinessRules = semesterBusinessRules;
        }

        public async Task<GetByIdSemesterResponse> Handle(GetByIdSemesterQuery request, CancellationToken cancellationToken)
        {
            Semester? semester = await _semesterRepository.GetAsync(predicate: s => s.Id == request.Id, cancellationToken: cancellationToken);
            await _semesterBusinessRules.SemesterShouldExistWhenSelected(semester);

            GetByIdSemesterResponse response = _mapper.Map<GetByIdSemesterResponse>(semester);
            return response;
        }
    }
}