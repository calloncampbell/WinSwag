﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Storage;
using WinSwag.Models;
using WinSwag.ViewModels;

namespace WinSwag.Services
{
    public interface ISessionManagerVM : INotifyPropertyChanged
    {
        IReadOnlyList<SwaggerSessionInfo> StoredSessions { get; }

        SwaggerDocumentViewModel CurrentDocument { get; }

        bool IsSessionLoaded { get; }

        bool IsCurrentSessionFavorite { get; }

        bool IsntCurrentSessionFavorite { get; }

        Task CreateSessionAsync(string url);

        Task CreateSessionAsync(StorageFile file);

        Task DeleteSessionAsync(SwaggerSessionInfo sessionInfo);

        Task LoadSessionAsync(SwaggerSessionInfo sessionInfo);

        Task SaveCurrentSessionAsync(string displayName);

        Task DeleteCurrentSessionAsync();

        Task UnloadCurrentSessionAsync();
    }
}
