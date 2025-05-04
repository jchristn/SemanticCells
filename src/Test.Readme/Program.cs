using System;
using System.Collections.Generic;
using System.Linq;
using SemanticCells;

class Program
{
    static void Main()
    {
        //
        // Create a document with hierarchical semantic cells
        //
        var document = new SemanticCell
        {
            Content = "AI Document Analysis",
            Metadata = new { Author = "John Doe", Created = DateTime.Now }
        };

        //
        // Add a section with paragraph
        //
        var section = new SemanticCell
        {
            Content = "Introduction to Semantic Analysis",
            Position = 1
        };
        document.Children.Add(section);

        //
        // Add paragraph with content
        //
        var paragraph = new SemanticCell
        {
            Content = "Semantic analysis is the process of understanding the meaning of text. " +
                     "It involves analyzing context and relationships between words.",
            Position = 1
        };
        section.Children.Add(paragraph);

        //
        // Split paragraph into chunks (e.g., for LLM processing with token limits)
        //
        paragraph.Chunks.Add(new SemanticChunk
        {
            Content = "Semantic analysis is the process of understanding the meaning of text.",
            Position = 0,
            Start = 0,
            End = 70
        });

        paragraph.Chunks.Add(new SemanticChunk
        {
            Content = "It involves analyzing context and relationships between words.",
            Position = 1,
            Start = 71,
            End = 130
        });

        //
        // Add a list to the section
        //
        var listCell = new SemanticCell
        {
            Position = 2,
            OrderedList = new List<string>
            {
                "Lexical Analysis - Word structure",
                "Syntactic Analysis - Grammar structure",
                "Semantic Analysis - Meaning extraction"
            }
        };
        section.Children.Add(listCell);

        //
        // Use built-in helper methods to work with the document structure
        //
        Console.WriteLine($"Total cells: {SemanticCell.CountSemanticCells(new List<SemanticCell> { document })}");
        Console.WriteLine($"Total chunks: {SemanticCell.CountSemanticChunks(new List<SemanticCell> { document })}");

        //
        // Find all cells in the hierarchy
        //
        foreach (var cell in SemanticCell.AllCells(new List<SemanticCell> { document }))
        {
            Console.WriteLine($"Cell: {cell.CellType}, Length: {cell.Length}");
        }

        //
        // Find all chunks in the document
        //
        foreach (var chunk in SemanticCell.AllChunks(new List<SemanticCell> { document }))
        {
            Console.WriteLine($"Chunk: {chunk.Content.Substring(0, Math.Min(40, chunk.Content.Length))}...");
        }

        //
        // Find only cells containing chunks
        //
        var cellsWithChunks = SemanticCell.FindCellsWithChunks(new List<SemanticCell> { document });
        Console.WriteLine($"Found {cellsWithChunks.Count()} cells with chunks");
    }
}