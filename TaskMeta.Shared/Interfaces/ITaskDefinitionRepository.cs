﻿using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;
public interface ITaskDefinitionRepository : IRepositoryBase<TaskDefinition>
{
    List<TaskDefinition> GetList();
    List<TaskDefinition> GetList(ApplicationUser user);
    List<TaskDefinition> GetListByUser(ApplicationUser user);
}