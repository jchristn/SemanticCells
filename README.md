![alt tag](https://raw.githubusercontent.com/jchristn/SemanticCells/main/SemanticCells/assets/logo.ico)

# SemanticCells

[![NuGet Version](https://img.shields.io/nuget/v/SemanticCells.svg?style=flat)](https://www.nuget.org/packages/SemanticCells/) [![NuGet](https://img.shields.io/nuget/dt/SemanticCells.svg)](https://www.nuget.org/packages/SemanticCells) 

Semantic cells are data structures useful for parsing and organizing content for use in AI and analytics

## New in v0.1.x

- Initial alpha release

## Help or feedback

First things first - do you need help or have feedback?  File an issue here!

## Definition

A semantic cell (`SemanticCell`) is a content region containing correlated data from within a data asset, generally of a type like text, list, table, binary, or others.  Cells have metadata (such as ordinal positions, hash values, lengths, and bounding boxes) and their content is typically broken into smaller, more manageable pieces, usually due to application constraints or model input limitations.  These smaller pieces are often referred to ask semantic chunks (`SemanticChunk`, found within `SemanticCell.Chunks`), and are child nodes of the semantic cell.  Like cells, chunks also have similar metadata (ordinal positions, hash values, lengths, and bounding boxes).  

Further, a semantic cell might be large in size and contain several smaller, subordinate semantic cells, as children (`SemanticCell.Children`).  For example, an entire PowerPoint slide might be a semantic cell, the title within the slight might be another, a bulleted list might be another, and an image within the slide might yet be another.

The intent of this library is to provide a flexible data structure for holding cells, their children, and the chunks of data therein.

## Simple Example

```csharp
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
```

