// This source code is a part of project violet-server.
// Copyright (C) 2021. violet-team. Licensed under the MIT Licence.

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace UserBookmarkAnalyzer
{
    public class DataModel
    {
        public class _Record
        {
            public int Id { get; set; }
            public string Article { get; set; }
            public string DateTimeStart { get; set; }
            public string DateTimeEnd { get; set; }
            public int? LastPage { get; set; }
            public int Type { get; set; }
        }

        public class _Article
        {
            public int Id { get; set; }
            public string Article { get; set; }
            public string DateTime { get; set; }
            public int GroupId { get; set; }
        }

        public class _Artist
        {
            public int Id { get; set; }
            public string Artist { get; set; }
            public string DateTime { get; set; }
            public int GroupId { get; set; }
            public int IsGroup { get; set; }
        }

        public class _Group
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public long Color { get; set; }
            public int Gorder { get; set; }
        }

        public List<_Record> record { get; set; }
        public List<_Article> article { get; set; }
        public List<_Artist> artist { get; set; }
        public List<_Group> group { get; set; }

        public string UserAppId { get; set; }
    }

    class Program
    {
        /*
            {
                'record': [
                    {
                        "Id": 331,
                        "Article": "1701118",
                        "DateTimeStart": "2021-02-18 02:04:11.714748",
                        "DateTimeEnd": null,
                        "LastPage": null,
                        "Type": 0
                    }
                ],
                'article': [
                    {
                        "Id": 2,
                        "Article": "1772632",
                        "DateTime": "2020-11-12 07:08:36.601968",
                        "GroupId": 1
                    }
                ],
                'artist': [
                    {
                        "Id": 1,
                        "Artist": "homunculus",
                        "IsGroup": 0,
                        "DateTime": "2020-12-09 09:56:44.259153",
                        "GroupId": 1
                    }
                ],
                'group': [
                    {
                        "Id": 1,
                        "Name": "violet_default",
                        "DateTime": "2020-11-12 06:55:46.623079",
                        "Description": "Unclassified bookmarks.",
                        "Color": 4288585374,
                        "Gorder": 1
                    }
                ]
            }
        */
        static void Main(string[] args)
        {
            var datas = new List<DataModel>();
            foreach (var file in Directory.GetFiles("./data"))
            {
                if (!file.EndsWith(".json")) continue;

                try
                {
                    var data = JsonConvert.DeserializeObject<DataModel>(File.ReadAllText(file));
                    data.UserAppId = file.Split('/').Last().Split('.').First();
                    datas.Add(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(file);
                    break;
                }
            }

            /*datas.Sort((x,y) => x.group.Count.CompareTo(y.group.Count));
            datas = datas.Where(x => x.group.Count >= 3).ToList();

            var harder = 0;
            foreach (var data in datas) {
                Console.WriteLine(data.group.Count);
                Console.WriteLine(data.UserAppId);
                data.group.ForEach((x) => {
                    if (x.Name == "violet_default") return;
                    if (x.Name == "새로운 그룹" && x.Description == "새로운 그룹") return;
                    Console.WriteLine($"    {x.Name} ({x.Description})");
                });
                harder ++;
            }

            Console.WriteLine(harder);*/

            /*var x = (double)datas.Sum(x => x.article.Count) / datas.Count;
            var y = (double)datas.Sum(x => x.artist.Count) / datas.Count;
            var z = (double)datas.Sum(x => x.record.Count) / datas.Count;
            var w = (double)datas.Sum(x => x.group.Count) / datas.Count;

            Console.WriteLine($"{x} {y} {z} {w}");*/

            /*var group_data = new Dictionary<string, Tuple<int, int>>();

            foreach (var data in datas)
            {
                var article = new Dictionary<int, int>();
                var artist = new Dictionary<int, int>();

                data.article.ForEach((x) =>
                {
                    if (!article.ContainsKey(x.GroupId))
                        article.Add(x.GroupId, 0);
                    article[x.GroupId]++;
                });

                data.artist.ForEach((x) =>
                {
                    if (!artist.ContainsKey(x.GroupId))
                        artist.Add(x.GroupId, 0);
                    artist[x.GroupId]++;
                });

                data.group.ForEach((x) =>
                {
                    if (x.Name == "violet_default") return;
                    if (x.Name == "새로운 그룹" && x.Description == "새로운 그룹") return;

                    var key = $"{x.Name} ({x.Description}) ({data.UserAppId})";
                    if (group_data.ContainsKey(key))
                        group_data[key] = new Tuple<int, int>(
                            group_data[key].Item1 + article[x.Id],
                            group_data[key].Item2 + artist[x.Id]);
                    else
                        group_data.Add(key,  new Tuple<int, int>(
                            article.ContainsKey(x.Id) ? article[x.Id] : 0,
                            artist.ContainsKey(x.Id) ? artist[x.Id] : 0));
                });
            }

            var gdl = group_data.ToList();
            gdl.Sort((x, y) => x.Value.Item2.CompareTo(y.Value.Item2));

            gdl.ForEach((x) => {
                Console.WriteLine($"{x.Key}: {x.Value.Item1}, {x.Value.Item2}");
            });*/

            /*var article_bookmark = new Dictionary<string, int>();
            var artist_bookmark = new Dictionary<string, int>();

            datas.ForEach((x) => {
                x.article.ForEach(y => {
                    if (!article_bookmark.ContainsKey(y.Article))
                        article_bookmark.Add(y.Article, 0);
                    article_bookmark[y.Article]++;
                });
                x.artist.ForEach(y => {
                    if (!artist_bookmark.ContainsKey(y.Artist))
                        artist_bookmark.Add(y.Artist, 0);
                    artist_bookmark[y.Artist]++;
                });
            });

            var abl = article_bookmark.ToList();
            var rbl = artist_bookmark.ToList();

            abl.Sort((x, y) => x.Value.CompareTo(y.Value));
            rbl.Sort((x,y) => x.Value.CompareTo(y.Value));

            rbl.ForEach(x => Console.WriteLine($"{x.Key} ({x.Value})"));
            */

            var article_ref = new Dictionary<string, Dictionary<string, double>>();
            var artist_ref = new Dictionary<string, Dictionary<string, double>>();

            foreach (var data in datas)
            {
                var article = new Dictionary<int, List<string>>();
                var artist = new Dictionary<int, List<string>>();

                data.article.ForEach((x) =>
                {
                    if (!article.ContainsKey(x.GroupId))
                        article.Add(x.GroupId, new List<string>());
                    article[x.GroupId].Add(x.Article);
                });

                data.artist.ForEach((x) =>
                {
                    if (!artist.ContainsKey(x.GroupId))
                        artist.Add(x.GroupId,  new List<string>());
                    artist[x.GroupId].Add(x.Artist);
                });

                article.ToList().ForEach (x => {
                    for (int i = 0; i < x.Value.Count; i++) {
                        var xa = x.Value[i];
                        if (!article_ref.ContainsKey(xa))`
                            article_ref.Add(xa, new Dictionary<string, double>()); 
                        for (int j = i+1; j < x.Value.Count; j++) {
                            var ya = x.Value[j];
                            if (!article_ref[xa].ContainsKey(ya))
                                article_ref[xa].Add(ya, 0);
                            article_ref[xa][ya] += 1.0 / (x.Value.Count - 1);
                        }
                    }
                });

                artist.ToList().ForEach (x => {
                    for (int i = 0; i < x.Value.Count; i++) {
                        var xa = x.Value[i];
                        if (!artist_ref.ContainsKey(xa))
                            artist_ref.Add(xa, new Dictionary<string, double>()); 
                        for (int j = i+1; j < x.Value.Count; j++) {
                            var ya = x.Value[j];
                            if (!artist_ref[xa].ContainsKey(ya))
                                artist_ref[xa].Add(ya, 0);
                            artist_ref[xa][ya] += 1.0 / (x.Value.Count - 1);
                        }
                    }
                });
            }

            var arl = article_ref.ToList();
            var ll = new List<Tuple<string, string, double>>();
            arl.ForEach(x => ll.AddRange(
                x.Value.Where(y => x.Key != y.Key)
                .Select(y => new Tuple<string, string, double> (x.Key, y.Key, y.Value)).ToList()));
            
            ll.Sort((x,y) => y.Item3.CompareTo(x.Item3));

            foreach (var e in ll) {
                Console.WriteLine($"{e.Item1}, {e.Item2} ({e.Item3})");
            }
        }
    }
}
