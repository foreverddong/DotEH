using DotEH.Model;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotEH.Services
{
    public class TagStorageService
    {
        private string jsonstr = $"{FileSystem.AppDataDirectory}/tags.json";

        public GalleryTag Storage { get; set; } = new();

        public async Task LoadTagsAsync()
        {
            if (!File.Exists(jsonstr))
            {
                return;
            }
            using (var file = File.OpenRead(jsonstr))
            {
                try
                {
                    this.Storage = await JsonSerializer.DeserializeAsync<GalleryTag>(file);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public async Task AddTagOrSkip(string newTag)
        {
            if (this.Storage.Tags.Any(t => t == newTag))
            {
                return;
            }
            this.Storage.Tags.Add(newTag);
            await this.SaveTagsAsync();
        }

        public async Task SaveTagsAsync()
        {
            using var file = File.Open(jsonstr, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(file, this.Storage);
        }
    }
}
