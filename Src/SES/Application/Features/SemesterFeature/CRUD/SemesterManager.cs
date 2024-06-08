using Application.Repositories;
using AutoMapper;
using Entity.Entities.Infos;

namespace Application.Features.SemesterFeature.CRUD;

public class SemesterManager : ISemesterService
{
    private readonly ISemesterRepository _SemesterRepository;
    private readonly IMapper _mapper;

    public SemesterManager(ISemesterRepository semesterRepository, IMapper mapper)
    {
        _SemesterRepository = semesterRepository;
        _mapper = mapper;
    }

    public void AddSemester(CreateSemesterDTO createSemesterDTO)
    {
        Semester semester = _mapper.Map<Semester>(createSemesterDTO);

        _SemesterRepository.Add(semester);
    }

    public IList<RequestSemesterDTO> GetRequestSemesters()
    {
        IList<Semester> semesters = _SemesterRepository.GetList(size:int.MaxValue).Items;
        return _mapper.Map<IList<RequestSemesterDTO>>(semesters);
    }

    public void UpdateSemester(UpdateSemesterDTO createSemesterDTO)
    {
        throw new NotImplementedException();
    }
}