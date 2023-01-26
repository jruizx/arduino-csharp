using Arduino.DomainModel.ProjectAggregate;

namespace Arduino.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ArduinoContext context;

    public ProjectRepository(ArduinoContext context)
    {
        this.context = context;
    }

    public void Add(Project project)
    {
        this.context.Projects.Add(project);
    }
}