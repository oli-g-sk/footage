namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            await dao.Commit();
            
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
            await dao.Commit();
        }
        
        public async Task ArchiveProject(int projectId)
        {
            using var dao = GetDao();
            var project = await dao.Get<Project>(projectId);

            if (project == null)
            {
                // TODO use NotFoundException
                throw new ArgumentException(nameof(projectId));
            }

            project.IsArchived = true;
            await dao.Update(project);
            await dao.Commit();
        }
        
        public async Task<IEnumerable<Project>> GetAllProjects(bool includeArchived)
        {
            using var dao = GetDao();
            var projects = dao.Query<Project>();
            
            if (!includeArchived)
            {
                projects = projects.Where(p => !p.IsArchived);
            }

            var result = await projects.ToListAsync();
            Log.Debug($"Retrieved list of projects; includeArchived? {includeArchived}, count: {result.Count}.");
            return result;
        }
    }
}