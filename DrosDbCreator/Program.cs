using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dros.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Console;

namespace DrosDbCreator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //await CreateDb();
            await UpdateUrlContentLength();
            ReadKey();
        }

        private static async Task CreateDb()
        {
            string wpPosts;
            string wpTermRelationships;
            string wpTermTaxonomy;
            string wpTerms;
            var context = new DrosDbContext();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                wpPosts = await File.ReadAllTextAsync(@"C:\Dev\dros\wp_posts.json");
                wpTermRelationships = await File.ReadAllTextAsync(@"C:\Dev\dros\wp_term_relationships.json");
                wpTermTaxonomy = await File.ReadAllTextAsync(@"C:\Dev\dros\wp_term_taxonomy.json");
                wpTerms = await File.ReadAllTextAsync(@"C:\Dev\dros\wp_terms.json");
            }
            else
            {
                wpPosts = await File.ReadAllTextAsync(@"/Users/malshehri/Desktop/dros/wp_posts.json");
                wpTermRelationships = await File.ReadAllTextAsync(@"/Users/malshehri/Desktop/dros/wp_term_relationships.json");
                wpTermTaxonomy = await File.ReadAllTextAsync(@"/Users/malshehri/Desktop/dros/wp_term_taxonomy.json");
                wpTerms = await File.ReadAllTextAsync(@"/Users/malshehri/Desktop/dros/wp_terms.json");
            }

            var counter = 1;

            var posts = JsonConvert.DeserializeObject<IList<wp_posts>>(wpPosts);
            var termRelationships = JsonConvert.DeserializeObject<IList<wp_term_relationships>>(wpTermRelationships);
            var termTaxonomies = JsonConvert.DeserializeObject<IList<wp_term_taxonomy>>(wpTermTaxonomy);
            var terms = JsonConvert.DeserializeObject<IList<wp_terms>>(wpTerms);

            var regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);

            foreach (var term in terms)
            {
                var termTaxonomy = termTaxonomies.First(x => x.term_id == int.Parse(term.term_id));
                if (termTaxonomy?.taxonomy == "category")
                {
                    if (context.Authors.FirstOrDefault(x => x.Name == term.name) == null)
                        context.Authors.Add(new Author { Id = Guid.NewGuid().ToString(), Name = term.name });
                }

                else if (termTaxonomy?.taxonomy == "post_tag")
                {
                    if (context.Tags.FirstOrDefault(x => x.Name == term.name) == null)
                        context.Tags.Add(new Tag { Id = Guid.NewGuid().ToString(), Name = term.name });
                }
            }
            var termTaxonomyResult = await context.SaveChangesAsync();

            posts = posts.Where(x => x.post_status == "publish").ToList();
            WriteLine($"published posts = {posts.Count}");


            WriteLine($"TaxonomyResult {termTaxonomyResult}");
            foreach (var post in posts)
            {
                var material = new Material
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = post.post_title,
                    Links = new List<Url>(),
                    Authors = new List<MaterialAuthor>(),
                    Tags = new List<MaterialTag>(),
                    Categories = new List<MaterialCategory>()
                };

                context.Materials.Add(material);

                var matches = regx.Matches(post.post_content);
                int mp3Order = 1, oggOrder = 1, wmaOrder = 1, rmOrder = 1, isoOrder = 1;

                foreach (Match match in matches)
                {
                    var url = new Url { Id = Guid.NewGuid().ToString() };
                    var newLink = match.Value;//.Replace("http://", "https://");
                    if (match.Value.ToLower().Contains(".rm"))
                    {
                        url.Link = newLink;
                        url.AudioType = AudioType.Rm;
                        url.Order = rmOrder++;
                    }
                    else if (match.Value.ToLower().Contains(".mp3"))
                    {
                        url.Link = newLink;
                        url.AudioType = AudioType.Mp3;
                        url.Order = mp3Order++;
                    }
                    else if (match.Value.ToLower().Contains(".ogg"))
                    {
                        url.Link = newLink;
                        url.AudioType = AudioType.Ogg;
                        url.Order = oggOrder++;
                    }
                    else if (match.Value.ToLower().Contains(".wma"))
                    {
                        url.Link = newLink;
                        url.AudioType = AudioType.Wma;
                        url.Order = wmaOrder++;
                    }

                    else if (match.Value.ToLower().Contains(".iso"))
                    {
                        url.Link = newLink;
                        url.AudioType = AudioType.Iso;
                        url.Order = isoOrder++;
                    }

                    if (url.AudioType > 0)
                        material.Links.Add(url);
                }

                var termRelationshipList = termRelationships.Where(x => x.object_id == post.ID).ToList();
                foreach (var termRelationship in termRelationshipList)
                {
                    var termTaxonomie =
                        termTaxonomies.FirstOrDefault(x => x.term_taxonomy_id == termRelationship.term_taxonomy_id);
                    var term = terms.FirstOrDefault(x => int.Parse(x.term_id) == termTaxonomie?.term_id);
                    if (termTaxonomie?.taxonomy == "category")
                    {
                        var author = context.Authors.FirstOrDefault(x => x.Name == term.name);
                        var materialAuthor = new MaterialAuthor { Material = material, Author = author };
                        material.Authors.Add(materialAuthor);
                    }

                    else if (termTaxonomie?.taxonomy == "post_tag")
                    {
                        var tag = context.Tags.FirstOrDefault(x => x.Name == term.name);
                        var materialTag = new MaterialTag { Material = material, Tag = tag };
                        material.Tags.Add(materialTag);
                    }
                }
                WriteLine($"{counter}");
                counter += 1;
            }

            await context.SaveChangesAsync();
            WriteLine("Finished creating the database!");
            context.Dispose();
        }
        private static async Task UpdateUrlContentLength()
        {
            using (var context = new DrosDbContext())
            {
                var urls = await context.Urls.ToArrayAsync();
                const int increment = 20;
                WriteLine($"Total: {urls.Length}");
                for (var i = 0; i < urls.Length; i += increment)
                {
                    if (i + 20 > urls.Length)
                    {
                        for (; i < urls.Length; i++)
                        {
                            urls[i] = await GetContentLengthAsync(urls[i]);
                            context.Entry(urls[i]).State = EntityState.Modified;
                            WriteLine(i);
                        }
                    }
                    else
                    {
                        var task0 = GetContentLengthAsync(urls[i + 0]);
                        var task1 = GetContentLengthAsync(urls[i + 1]);
                        var task2 = GetContentLengthAsync(urls[i + 2]);
                        var task3 = GetContentLengthAsync(urls[i + 3]);
                        var task4 = GetContentLengthAsync(urls[i + 4]);
                        var task5 = GetContentLengthAsync(urls[i + 5]);
                        var task6 = GetContentLengthAsync(urls[i + 6]);
                        var task7 = GetContentLengthAsync(urls[i + 7]);
                        var task8 = GetContentLengthAsync(urls[i + 8]);
                        var task9 = GetContentLengthAsync(urls[i + 9]);
                        var task10 = GetContentLengthAsync(urls[i + 10]);
                        var task11 = GetContentLengthAsync(urls[i + 11]);
                        var task12 = GetContentLengthAsync(urls[i + 12]);
                        var task13 = GetContentLengthAsync(urls[i + 13]);
                        var task14 = GetContentLengthAsync(urls[i + 14]);
                        var task15 = GetContentLengthAsync(urls[i + 15]);
                        var task16 = GetContentLengthAsync(urls[i + 16]);
                        var task17 = GetContentLengthAsync(urls[i + 17]);
                        var task18 = GetContentLengthAsync(urls[i + 18]);
                        var task19 = GetContentLengthAsync(urls[i + 19]);

                        urls[i + 0] = await task0;
                        urls[i + 1] = await task1;
                        urls[i + 2] = await task2;
                        urls[i + 3] = await task3;
                        urls[i + 4] = await task4;
                        urls[i + 5] = await task5;
                        urls[i + 6] = await task6;
                        urls[i + 7] = await task7;
                        urls[i + 8] = await task8;
                        urls[i + 9] = await task9;
                        urls[i + 10] = await task10;
                        urls[i + 11] = await task11;
                        urls[i + 12] = await task12;
                        urls[i + 13] = await task13;
                        urls[i + 14] = await task14;
                        urls[i + 15] = await task15;
                        urls[i + 16] = await task16;
                        urls[i + 17] = await task17;
                        urls[i + 18] = await task18;
                        urls[i + 19] = await task19;

                        context.Entry(urls[i + 0]).State = EntityState.Modified;
                        context.Entry(urls[i + 1]).State = EntityState.Modified;
                        context.Entry(urls[i + 2]).State = EntityState.Modified;
                        context.Entry(urls[i + 3]).State = EntityState.Modified;
                        context.Entry(urls[i + 4]).State = EntityState.Modified;
                        context.Entry(urls[i + 5]).State = EntityState.Modified;
                        context.Entry(urls[i + 6]).State = EntityState.Modified;
                        context.Entry(urls[i + 7]).State = EntityState.Modified;
                        context.Entry(urls[i + 8]).State = EntityState.Modified;
                        context.Entry(urls[i + 9]).State = EntityState.Modified;
                        context.Entry(urls[i + 10]).State = EntityState.Modified;
                        context.Entry(urls[i + 11]).State = EntityState.Modified;
                        context.Entry(urls[i + 12]).State = EntityState.Modified;
                        context.Entry(urls[i + 13]).State = EntityState.Modified;
                        context.Entry(urls[i + 14]).State = EntityState.Modified;
                        context.Entry(urls[i + 15]).State = EntityState.Modified;
                        context.Entry(urls[i + 16]).State = EntityState.Modified;
                        context.Entry(urls[i + 17]).State = EntityState.Modified;
                        context.Entry(urls[i + 18]).State = EntityState.Modified;
                        context.Entry(urls[i + 19]).State = EntityState.Modified;
                        WriteLine(i);
                    }
                }

                WriteLine("Finished updating the content length.");

                await context.SaveChangesAsync();
                WriteLine("Finished");

            }
        }
        private static async Task<Url> GetContentLengthAsync(Url url)
        {
            try
            {
                var webRequest = WebRequest.Create(url.Link);
                webRequest.Method = "HEAD";
                using (var webResponse = await webRequest.GetResponseAsync())
                {
                    url.ContentLength = Convert.ToInt64(webResponse.Headers.Get("Content-Length"));
                }
            }
            catch (Exception)
            {
                url.ContentLength = 0;
            }

            return url;
        }
    }
}


