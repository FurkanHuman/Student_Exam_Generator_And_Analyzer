using Application.Features.ExamConfigurations.Commands.Create;
using Application.Features.ExamConfigurations.Commands.Delete;
using Application.Features.ExamConfigurations.Commands.Update;
using Application.Features.ExamConfigurations.Queries.GetById;
using Application.Features.ExamConfigurations.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.ExamConfigurations.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateExamConfigurationCommand, ExamConfiguration>();
        CreateMap<ExamConfiguration, CreatedExamConfigurationResponse>();

        CreateMap<UpdateExamConfigurationCommand, ExamConfiguration>();
        CreateMap<ExamConfiguration, UpdatedExamConfigurationResponse>();

        CreateMap<DeleteExamConfigurationCommand, ExamConfiguration>();
        CreateMap<ExamConfiguration, DeletedExamConfigurationResponse>();

        CreateMap<ExamConfiguration, GetByIdExamConfigurationResponse>();

        CreateMap<ExamConfiguration, GetListExamConfigurationListItemDto>();
        CreateMap<IPaginate<ExamConfiguration>, GetListResponse<GetListExamConfigurationListItemDto>>();
    }
}