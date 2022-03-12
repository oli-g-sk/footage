namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Footage.Model;
    using Microsoft.EntityFrameworkCore;
    using NLog;

    public class ProjectsRepository : RepositoryBase
    {
        private static ILogger Log => LogManager.GetCurrentClassLogger();
        
        public async Task<Project> CreateNewProject(string name)
        {
            Log.Info($"Saving new project named '{name}'.");
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
            var projects = await dao.Query<Project>().ToListAsync();
            Log.Debug($"Retrieved list of all projects; count: {projects}.");
            return projects;
        }
    }
}