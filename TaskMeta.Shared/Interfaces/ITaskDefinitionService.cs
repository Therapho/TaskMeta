﻿using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;
public interface ITaskDefinitionService : IEntityService<TaskDefinition>
{
    IQueryable<TaskDefinition> GetTaskDefinitionsQuery();
}