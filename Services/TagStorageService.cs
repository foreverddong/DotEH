using DotEH.Model;
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
        public GallaryTags tags { get; set; }
        private readonly string jsonDir = $"{FileSystem.Current.AppDataDirectory}/tags.json";

        public async Task LoadLocalTags()
        {
            if (!File.Exists(jsonDir))
            {
                return;
            }
            using var jsonfile = File.OpenRead(jsonDir);
            this.tags = await JsonSerializer.DeserializeAsync<GallaryTags>(jsonfile);

        }

        public async Task SaveLocalTags()
        {
            using var jsonfile = File.Open(jsonDir, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(jsonfile , this.tags);
        }
    }
}
