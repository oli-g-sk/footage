namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Footage.Model;
    using Microsoft.EntityFrameworkCore;

    public class ProjectsRepository : RepositoryBase
    {
        public async Task<Project> CreateNewProject(string name)
        {
            var project = new Project();
            project.Name = name;
            
            using var dao = GetDao();
            await dao.Insert(project);

            return project;
        }

        public async Task RenameProject(int projectId, string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentException(nameof(newName));
            }
            
            using var dao = GetDao();
            var project = await dao.Get<Project>(projectId);

            if (project == null)
            {
                // TODO use NotFoundException
                throw new ArgumentException(nameof(projectId));
            }

            project.Name = newName;
            await dao.Update(project);
        }
        
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            using var dao = GetDao();
            return await dao.Query<Project>().ToListAsync();
        }
    }
}