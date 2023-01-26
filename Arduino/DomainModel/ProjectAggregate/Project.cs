using Arduino.Common.DomainModel;

namespace Arduino.DomainModel.ProjectAggregate;

public class Project : Entity
{
    public Project(string name)
    {
        SetName(name);
    }

    private void SetName(string name)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("It cannot be empty", nameof(name)) : name;
    }

    public string Name { get; private set; }
}