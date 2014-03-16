﻿using Lucene.Net.Documents;
using Lucene.Net.Index;
using ProjectExplorer.DocHierarchy.HierarchyLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectExplorer.DocHierarchy.FullText
{
    public class DocumentCreator
    {
        private IndexWriter IndexWriter { get; set; }

        public DocumentCreator(IndexWriter indexWriter)
        {
            this.IndexWriter = indexWriter;
        }

        public void AddDirectoryEntry(string path, string segment)
        {
            this.CreateDocument(path, segment, string.Empty);
            
        }

        public void AddFileEntry(FileSystemParser parser)
        {
            this.CreateDocument(parser.Path, parser.FileName, parser.FileContents);
        }

        private void CreateDocument(string path, string name, string contents)
        {
            Document doc = new Document();
            Field field = new Field(FullTextFieldType.PATH.ToString(), path, Field.Store.YES, Field.Index.NO);
            doc.Add(field);
            field = new Field(FullTextFieldType.NAME.ToString(), name, Field.Store.NO, Field.Index.ANALYZED);
            field.OmitTermFreqAndPositions = true;
            doc.Add(field);
            field = new Field(FullTextFieldType.CATCH_ALL.ToString(), name + " " + contents, Field.Store.NO, Field.Index.ANALYZED);
            field.OmitTermFreqAndPositions = true;
            doc.Add(field);
            this.IndexWriter.AddDocument(doc);
            this.IndexWriter.Commit();
        }
    }
}