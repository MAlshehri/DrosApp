using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dros.Data.Models;
using Newtonsoft.Json;
using static System.Console;

namespace DrosDbCreator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var wpPosts = await File.ReadAllTextAsync(@"C:\Dev\dros\wp_posts.json");
            var wpTermRelationships = await File.ReadAllTextAsync(@"C:\Dev\dros\wp_term_relationships.json");
            var wpTermTaxonomy = await File.ReadAllTextAsync(@"C:\Dev\dros\wp_term_taxonomy.json");
            var wpTerms = await File.ReadAllTextAsync(@"C:\Dev\dros\wp_terms.json");

            var posts = JsonConvert.DeserializeObject<IList<wp_posts>>(wpPosts);
            var termRelationships = JsonConvert.DeserializeObject<IList<wp_term_relationships>>(wpTermRelationships);
            var termTaxonomies = JsonConvert.DeserializeObject<IList<wp_term_taxonomy>>(wpTermTaxonomy);
            var terms = JsonConvert.DeserializeObject<IList<wp_terms>>(wpTerms);

            var regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
            using (var context = new DrosDbContext())
            {
                foreach (var post in posts)
                {

                    var material = new Material
                    {
                        Title = post.post_title,
                        Links = new List<Link>(),
                        Authors = new List<MaterialAuthor>(),
                        Tags = new List<MaterialTag>(),
                        Categories = new List<MaterialCategory>()
                    };

                    context.Materials.Add(material);

                    var matches = regx.Matches(post.post_content);
                    int mp3Order = 1, oggOrder = 1, wmaOrder = 1, rmOrder = 1;

                    foreach (Match match in matches)
                    {
                        var link = new Link();
                        if (match.Value.ToLower().Contains(".rm"))
                        {
                            link.Content = match.Value;
                            link.AudioType = AudioType.Rm;
                            link.Order = rmOrder++;
                        }
                        else if (match.Value.ToLower().Contains(".mp3"))
                        {
                            link.Content = match.Value;
                            link.AudioType = AudioType.Mp3;
                            link.Order = mp3Order++;
                        }
                        else if (match.Value.ToLower().Contains(".ogg"))
                        {
                            link.Content = match.Value;
                            link.AudioType = AudioType.Ogg;
                            link.Order = oggOrder++;
                        }
                        else if (match.Value.ToLower().Contains(".wma"))
                        {
                            link.Content = match.Value;
                            link.AudioType = AudioType.Wma;
                            link.Order = wmaOrder++;
                        }

                        if (link.AudioType > 0)
                            material.Links.Add(link);
                    }

                    var termRelationshipList = termRelationships.Where(x => x.object_id == post.ID).ToList();
                    foreach (var termRelationship in termRelationshipList)
                    {
                        var termTaxonomie =
                            termTaxonomies.FirstOrDefault(x => x.term_taxonomy_id == termRelationship.term_taxonomy_id);
                        var term = terms.FirstOrDefault(x => int.Parse(x.term_id) == termTaxonomie?.term_id);
                        if (termTaxonomie?.taxonomy == "category")
                        {
                            var author = new Author {Name = term?.name};
                            var materialAuthor = new MaterialAuthor {Material = material, Author = author};
                            material.Authors.Add(materialAuthor);
                        }

                        else if (termTaxonomie?.taxonomy == "post_tag")
                        {
                            var tag = new Tag {Name = term?.name};
                            var materialTag = new MaterialTag {Material = material, Tag = tag};
                            material.Tags.Add(materialTag);
                        }
                    }

                    WriteLine($"{material.Title}");
                    foreach (var link in material.Links.OrderBy(x => x.AudioType).ThenBy(x => x.Order))
                    {
                        WriteLine($"{link.Order} {link.Content} {link.AudioType.ToString()}");
                    }

                    WriteLine($"{material.Authors.FirstOrDefault()?.Author.Name}");
                    foreach (var tag in material.Tags)
                    {
                        WriteLine($"{tag.Tag.Name} ");
                    }
                }
            }

            ReadKey();
        }
    }
}
