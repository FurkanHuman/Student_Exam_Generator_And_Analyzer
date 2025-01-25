using Application.Features.ReferenceBenefits.Commands.Create;
using Application.Features.ReferenceBenefits.Commands.Delete;
using Application.Features.ReferenceBenefits.Commands.Update;
using Application.Features.ReferenceBenefits.Queries.GetById;
using Application.Features.ReferenceBenefits.Queries.GetList;
using Application.Services.PdfReaderService.Dtos;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.ReferenceBenefits.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, CreatedReferenceBenefitResponse>();

        CreateMap<ReferenceBenefit, ReferenceBenefitDto>()
            .ForMember(destinationMember: rb => rb.RBName, memberOptions: opt => opt.MapFrom(rbd => rbd.ReferenceBenefitName))
            .ForMember(destinationMember: rb => rb.LessonId, memberOptions: opt => opt.MapFrom(rbd => rbd.LessonId))
            .ForMember(destinationMember: rb => rb.SchoolId, memberOptions: opt => opt.MapFrom(rbd => rbd.SchoolId))
            .ForMember(destinationMember: rb => rb.SemesterId, memberOptions: opt => opt.MapFrom(rbd => rbd.SemesterId));

        CreateMap<ReferenceBenefitDto, ReferenceBenefit>()
            .ForMember(destinationMember: rb => rb.ReferenceBenefitName, memberOptions: opt => opt.MapFrom(rbd => rbd.RBName));

        CreateMap<UpdateReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, UpdatedReferenceBenefitResponse>();

        CreateMap<DeleteReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, DeletedReferenceBenefitResponse>();

        CreateMap<ReferenceBenefit, GetByIdReferenceBenefitResponse>();

        CreateMap<ReferenceBenefit, GetListReferenceBenefitListItemDto>();
        CreateMap<IPaginate<ReferenceBenefit>, GetListResponse<GetListReferenceBenefitListItemDto>>();
    }
}