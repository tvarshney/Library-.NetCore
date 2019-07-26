namespace MvcMovie.Models
{
using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
//using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using MySql.Data.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.LuceneVersion;
using Lucene.Net.Search.Highlight;

using Highlighter = Lucene.Net.Search.Highlight.Highlighter;
using Scorer =Lucene.Net.Search.Scorer;
using NullFragmenter = Lucene.Net.Search.Highlight.NullFragmenter;
using QueryScorer = Lucene.Net.Search.Highlight.QueryScorer;
using SimpleFragmenter = Lucene.Net.Search.Highlight.SimpleFragmenter;
using SimpleHTMLEncoder = Lucene.Net.Search.Highlight.SimpleHTMLEncoder;
using SimpleHTMLFormatter = Lucene.Net.Search.Highlight.SimpleHTMLFormatter;
using TextFragment = Lucene.Net.Search.Highlight.TextFragment;
using TokenGroup = Lucene.Net.Search.Highlight.TokenGroup;
using WeightedTerm = Lucene.Net.Search.Highlight.WeightedTerm;
    using static Lucene.Net.Search.SimpleFacetedSearch;

    public class Search 
    {
        //private DataContext context;
        static private FSDirectory Directory { get; set; }
        static private DirectoryReader  Reader{ get; set; }
        static public  IndexSearcher Searcher { get; set; }

       
        public Search()
        {
            InitializeComponent();
        }

       void InitializeComponent()
        {
            if(Directory==null)
            Directory=FSDirectory.Open(
                             AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LuceneIndexes"
                              //AppDomain.CurrentDomain.BaseDirectory + @"/App_Data/LuceneIndexes" //For Linux
                          );
            if(Reader==null)
            Reader = DirectoryReader.Open(Directory);
            if(Searcher==null)
             Searcher = new IndexSearcher(Reader);

        }

        public void createIndex(LibDocument rec)
        {
            // FSDirectory directory = FSDirectory.Open(
            //                  AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LuceneIndexes"
            //                   //AppDomain.CurrentDomain.BaseDirectory + @"/App_Data/LuceneIndexes" //For Linux
            //               );

                          //var recs=db.Documents.ToList();


            using (Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_48))
           {
            //IndexWriter writer = new IndexWriter(index, config);
             IndexWriterConfig config = new IndexWriterConfig(Version.LUCENE_48, analyzer);
           using(var writer = new IndexWriter(Directory, config))
            {    
             // the writer and analyzer will popuplate the directory with documents

                // foreach (var rec in recs)
                // {
                    var document = new Document();
                    // if(rec.Desc2==null)
                    // rec.Desc2="";

                    document.Add(new Field("Id",rec.id.ToString(),StringField.Store.YES,Field.Index.ANALYZED));
                    document.Add(new Field("Name",rec.Name.ToString(),StringField.Store.YES,Field.Index.ANALYZED));
                    document.Add(new Field("Path", rec.Path.ToString(), StringField.Store.YES,Field.Index.ANALYZED));
                    document.Add(new Field("Desc2", rec.Desc2.ToString(), StringField.Store.YES,Field.Index.ANALYZED));
                    document.Add(new Field("Desc1", rec.Desc1.ToString(), StringField.Store.YES,Field.Index.ANALYZED));
////

                     //document.Add(new StringField("Id",rec.id.ToString(),Field.Store.YES,));
                    // document.Add(new StringField("Name",rec.Name.ToString(),Field.Store.YES));
                    // document.Add(new StringField("Path", rec.Path.ToString(), Field.Store.YES));
                    // document.Add(new StringField("Desc2", rec.Desc2.ToString(), Field.Store.YES));
                    //  document.Add(new StringField("FullText",
                    //     string.Format("{0} {1} {2}",rec.Name, rec.Path, rec.Desc2)
                    //     , Field.Store.YES));
////

                    //document.Add(new StringField("Desc2", rec.Desc2.ToString(), StringField.Store.YES, Field.Index.ANALYZED));
                    //document.Add(new Field("BirthDate", row["BirthDate"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    //document.Add(new Field("ID", row["ID"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                    //var date = row["BirthDate"].ToString();

                    //date = string.Format("{0} {1}", date, date.Replace("/", " / "));

                    document.Add(new Field("FullText",
                        string.Format("{0} {1} {2} {3}",rec.Name, rec.Path, rec.Desc2, rec.Desc1)
                        , Field.Store.YES,Field.Index.ANALYZED
                        ));

                    writer.AddDocument(document);
               // }

                //writer.Optimize();
                writer.Commit();
           }
           }
        }

        public List<SearchDoc> searchfield(string textSearch)
        {
            
            //var table = Sample.Clone();

            //  var directory = FSDirectory.Open(
            //                  AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LuceneIndexes"
            //               );
                          //IndexSearcher searcher = new IndexSearcher(directory);

                        //IndexReader indexReader = IndexReader.Open(directory, true);
                        //Searcher indexSearch = new IndexSearcher(indexReader);  

                        List<SearchDoc>list=new List<SearchDoc>();


            // using (var reader = DirectoryReader.Open(Directory))
            // {
              //IndexSearcher searcher = new IndexSearcher(Reader);
            
                using (Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_48))
                {
                     var queryParser = new QueryParser(Version.LUCENE_48, "FullText", analyzer);

                    queryParser.AllowLeadingWildcard = true;

                    var query = queryParser.Parse(textSearch);
                    //var query=queryParser.Parse("FullText:"+textSearch);
                    //var query = new TermQuery(new Term("FullText", textSearch));
                    //SortField sortField = new SortField("FullText",SortFieldType.STRING);
                    Sort sort = new Sort(SortField.FIELD_SCORE);
                    

                   // var collector = TopScoreDocCollector.Create(1000,true);
                   // searcher.Search(query,collector);
                   // searcher.Search(query,10,sort);

                    //var matches = collector.GetTopDocs(0,30).ScoreDocs;
                    
                    var matches=Searcher.Search(query, null, 100, sort,true,true).ScoreDocs;
                    matches.OrderBy(de=>de.Score);

                //     #region  Test
                //     Stopwatch timer=new Stopwatch();
                //     timer.Start();
                //      TermQuery query1 = new TermQuery(new Term("FullText",textSearch));
                //    ScoreDoc[] hits = Searcher.Search(query1, 400).ScoreDocs;
                //     if (hits.Length > 0) {
                //         for (int i = 0; i < hits.Length; i++)
                //         {
                //             Document hitDoc = Searcher.Doc(hits[i].Doc); 
                //             string c=hitDoc.Get("Id");
                //         }
                //     }
                //     timer.Stop();
                //     TimeSpan ts = timer.Elapsed;

                //    string b=ts.Seconds.ToString();
                //     #endregion


                    foreach (var item in matches)
                    {
                        
                        var id = item.Doc;
                        var doc = Searcher.Doc(id);
                        //var d=doc.GetField("Name").ToString();

                        SearchDoc searchDoc=new SearchDoc();
 
                        searchDoc.id=doc.Get("Id");
                        searchDoc.Name=doc.GetField("Name").GetStringValue();
                        //searchDoc.Desc2=doc.GetField("Desc2").GetStringValue();
                        searchDoc.Path=doc.GetField("Path").GetStringValue(); 
                        searchDoc.Desc1=doc.GetField("Desc1").GetStringValue(); 
                        

                        list.Add(searchDoc);
                        //string name=
                        //string Desc2=doc.GetField("Desc2").ToString();
                      // doc.GetField("Name").GetStringValue;
                        // row["LastName"] = doc.GetField("Desc2").GetStringValue;
                        // row["JobTitle"] = doc.GetField("Path").GetStringValue;
                        // row["JobTitle"] = doc.GetField("JobTitle").GetStringValue;
                        // row["BirthDate"] = doc.GetField("BirthDate").GetStringValue;
                        // row["ID"] = doc.GetField("ID").GetStringValue;

                    }
                   
                   
                }
                
            //}     

         return   list;                               

        }


        

        public void DeleteFromIndex(LibDocument doc)
        {
            var directory = FSDirectory.Open(
                             AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LuceneIndexes"
                          );
            IndexReader indexReader = DirectoryReader.Open(directory);

             using (Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_48))
           {
            //IndexWriter writer = new IndexWriter(index, config);
             IndexWriterConfig config = new IndexWriterConfig(Version.LUCENE_48, analyzer);
             using(var writer = new IndexWriter(directory, config))
            { 
                var term = new Term("Id", doc.id.ToString());
                writer.DeleteDocuments(term);
                writer.DeleteUnusedFiles();
                writer.ForceMergeDeletes();
                writer.Commit();
            }
           }
        }

    }
}