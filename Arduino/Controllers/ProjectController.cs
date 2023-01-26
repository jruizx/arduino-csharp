using Arduino.Common.Application.Transaction;
using Arduino.Controllers.Requests;
using Arduino.DomainModel.ProjectAggregate;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Arduino.Controllers;

[Route("api/projects")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository projectRepository;
    private readonly IMapper mapper;

    public ProjectController(IProjectRepository projectRepository, IMapper mapper)
    {
        this.projectRepository = projectRepository;
        this.mapper = mapper;
    }

    [HttpPost]
    [Transaction]
    public IActionResult CreateProject(EditProjectDTO request)
    {
        var project = new Project(request.Name);
        projectRepository.Add(project);

        return Ok(mapper.Map<ProjectDTO>(project));
    }
}