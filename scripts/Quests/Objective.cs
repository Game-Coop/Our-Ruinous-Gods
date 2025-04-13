using System;
public class Objective {
    public bool Complete; { get; set; };
    public Guid Id { get; };
    public string DisplayName { get; };
    public Objective(string displayName)
        this.Complete = false;
        this.DisplayName = displayName;
        this.Id = Guid.NewGuid();
    }
}