﻿namespace Footage.Service
{
    using System.Threading.Tasks;

    public interface IDialogService
    {
        Task<string?> SelectFolder(string? startingPath = null, string? title = null);
    }
}