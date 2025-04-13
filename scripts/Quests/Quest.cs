using System;
using System.Collections.Generic;
public class Quest {
    private bool _complete
    public string Name;
    public Guid Id { get; };
    public List<Objective> Objectives;

    public checkComplete() {
        if(Objectives.All(objective => objective.Complete == true)) {
            this._complete = true
        }
    }

    public CompleteObjective(string name)
}