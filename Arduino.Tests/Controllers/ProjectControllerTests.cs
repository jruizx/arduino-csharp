using Arduino.Controllers;
using Arduino.Controllers.AutoMapper;
using Arduino.Controllers.Requests;
using Arduino.DomainModel.ProjectAggregate;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Arduino.Tests.Controllers;

public class ProjectControllerTests
{
    private ProjectController sut;
    private Mock<IProjectRepository> projectRepository;

    public ProjectControllerTests()
    {
        var mapperConfiguration = new MapperConfiguration(config =>
        {
            config.AddProfile<ArduinoProfile>();
        });

        var mapper = new Mapper(mapperConfiguration);

        projectRepository = new Mock<IProjectRepository>();
        sut = new ProjectController(projectRepository.Object, mapper);
    }

    [Fact]
    public void Should_return_ok_when_add_project()
    {
        var request = new EditProjectDTO() { Name = "name" };
        
        var result = sut.CreateProject(request);

        Assert.IsType<OkObjectResult>(result);
    }
}