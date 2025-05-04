namespace Test.SemanticCells
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using global::SemanticCells;
    using global::SerializableDataTables;
    using System.Runtime.Serialization.Json;

    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("SemanticCells Test Application");
            Console.WriteLine("============================");
            Console.WriteLine();

            Console.WriteLine("This application tests the SemanticCell and SemanticChunk classes");
            Console.WriteLine("from the SemanticCells library. It demonstrates their capabilities");
            Console.WriteLine("for organizing and storing hierarchical data with semantic meaning.");
            Console.WriteLine();

            // Test 1: Create and serialize a basic text cell
            TestBasicTextCell();

            // Test 2: Create and serialize a cell with binary data
            TestBinaryCell();

            // Test 3: Create and serialize a cell with an unordered list
            TestUnorderedListCell();

            // Test 4: Create and serialize a cell with an ordered list
            TestOrderedListCell();

            // Test 5: Create and serialize a cell with a table
            TestTableCell();

            // Test 6: Create and serialize a cell with a JSON object
            TestObjectCell();

            // Test 7: Create and serialize a cell with a JSON array
            TestArrayCell();

            // Test 8: Create a hierarchical structure with parent-child relationships
            TestHierarchicalCells();

            // Test 9: Create a cell with chunks and embeddings
            TestCellWithChunks();

            // Test 10: Exercise complex operations like counting, searching, etc.
            TestComplexOperations();

            // Demonstrate using static methods to analyze collections of cells
            TestStaticAnalysisMethods();

            Console.WriteLine("\nTests completed successfully.");
        }

        private static void TestBasicTextCell()
        {
            Console.WriteLine("Test 1: Basic Text Cell");
            Console.WriteLine("-----------------------");

            var cell = new SemanticCell
            {
                Content = "This is a basic text cell used to store simple textual content.",
                Metadata = new { Source = "User Input", Category = "Documentation" }
            };

            // Add a chunk to demonstrate how a cell can be chunked
            var chunk = new SemanticChunk
            {
                Content = cell.Content,
                Position = 0,
                Start = 0,
                End = cell.Content.Length - 1
            };

            // Add a simple embedding (normally this would come from an embedding model)
            float[] embeddingValues = new float[4] { 0.1f, 0.2f, 0.3f, 0.4f };
            chunk.Embeddings.AddRange(embeddingValues);

            cell.Chunks.Add(chunk);

            Console.WriteLine($"Added a chunk with {chunk.Embeddings.Count} embedding dimensions");

            SerializeAndPrint(cell);
        }

        private static void TestBinaryCell()
        {
            Console.WriteLine("\nTest 2: Binary Cell");
            Console.WriteLine("------------------");

            // Create some sample binary data (e.g., a small image or file)
            byte[] binaryData = Encoding.UTF8.GetBytes("Sample binary data representing an image or document");

            var cell = new SemanticCell
            {
                Binary = binaryData,
                BoundingBox = new BoundingBox
                {
                    UpperLeft = new Point(10, 10),
                    UpperRight = new Point(110, 10),
                    LowerLeft = new Point(10, 60),
                    LowerRight = new Point(110, 60)
                },
                Metadata = new { FileType = "Document", Size = binaryData.Length }
            };

            SerializeAndPrint(cell);
        }

        private static void TestUnorderedListCell()
        {
            Console.WriteLine("\nTest 3: Unordered List Cell");
            Console.WriteLine("--------------------------");

            var listItems = new List<string>
            {
                "First item in the unordered list",
                "Second item with some additional details",
                "Third item describing another aspect",
                "Fourth item with concluding information"
            };

            var cell = new SemanticCell
            {
                UnorderedList = listItems,
                Metadata = new { ListType = "Bullet Points", Purpose = "Feature Summary" }
            };

            SerializeAndPrint(cell);
        }

        private static void TestOrderedListCell()
        {
            Console.WriteLine("\nTest 4: Ordered List Cell");
            Console.WriteLine("------------------------");

            var listItems = new List<string>
            {
                "First step: Initialize the system",
                "Second step: Configure the parameters",
                "Third step: Process the input data",
                "Fourth step: Analyze the results",
                "Fifth step: Generate the report"
            };

            var cell = new SemanticCell
            {
                OrderedList = listItems,
                Metadata = new { ListType = "Numbered", Purpose = "Procedure" }
            };

            SerializeAndPrint(cell);
        }

        private static void TestTableCell()
        {
            Console.WriteLine("\nTest 5: Table Cell");
            Console.WriteLine("-----------------");

            // Create a sample DataTable
            DataTable dataTable = new DataTable("SampleData");
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Value", typeof(double));
            dataTable.Columns.Add("Date", typeof(DateTime));

            dataTable.Rows.Add(1, "Item A", 123.45, DateTime.Now.AddDays(-5));
            dataTable.Rows.Add(2, "Item B", 67.89, DateTime.Now.AddDays(-3));
            dataTable.Rows.Add(3, "Item C", 456.78, DateTime.Now.AddDays(-1));
            dataTable.Rows.Add(4, "Item D", 901.23, DateTime.Now);

            // Convert to SerializableDataTable
            SerializableDataTable serializableTable = SerializableDataTable.FromDataTable(dataTable);

            var cell = new SemanticCell
            {
                Table = serializableTable,
                Metadata = new { Source = "Database Query", Timestamp = DateTime.UtcNow }
            };

            // Display the table as Markdown as well
            Console.WriteLine("\nTable as Markdown:");
            Console.WriteLine(serializableTable.ToMarkdown());

            SerializeAndPrint(cell);
        }

        private static void TestObjectCell()
        {
            Console.WriteLine("\nTest 6: Object Cell");
            Console.WriteLine("------------------");

            // Create a sample JSON-serializable object
            var sampleObject = new
            {
                Id = Guid.NewGuid(),
                Name = "Sample Object",
                Properties = new
                {
                    Color = "Blue",
                    Size = "Medium",
                    Weight = 12.5,
                    Dimensions = new[] { 10, 20, 30 }
                },
                Tags = new[] { "Sample", "Test", "Demo" },
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var cell = new SemanticCell
            {
                Object = sampleObject,
                Metadata = new { ObjectType = "Configuration", Format = "JSON" }
            };

            SerializeAndPrint(cell);
        }

        private static void TestArrayCell()
        {
            Console.WriteLine("\nTest 7: Array Cell");
            Console.WriteLine("-----------------");

            // Create a sample JSON-serializable array
            var items = new List<object>
            {
                new { Id = 1, Name = "Item 1", Value = 100 },
                new { Id = 2, Name = "Item 2", Value = 200 },
                new { Id = 3, Name = "Item 3", Value = 300 },
                new { Id = 4, Name = "Item 4", Value = 400 }
            };

            var cell = new SemanticCell
            {
                Array = items,
                Metadata = new { ArrayType = "Items", Count = items.Count }
            };

            SerializeAndPrint(cell);
        }

        private static void TestHierarchicalCells()
        {
            Console.WriteLine("\nTest 8: Hierarchical Cell Structure");
            Console.WriteLine("----------------------------------");

            // Create a parent cell with various child cells
            var parentCell = new SemanticCell
            {
                Content = "This is a parent cell with multiple child cells of different types.",
                Metadata = new { Type = "Document", Author = "Test Application" }
            };

            // Add child cells of different types
            var childTextCell = new SemanticCell
            {
                Content = "This is a child text cell with some content.",
                Position = 0,
                Metadata = new { Type = "Section", Title = "Introduction" }
            };

            var childListCell = new SemanticCell
            {
                UnorderedList = new List<string>
                {
                    "First key point in this section",
                    "Second key point with more details",
                    "Third key point summarizing the concept"
                },
                Position = 1,
                Metadata = new { Type = "Section", Title = "Key Points" }
            };

            // Add a nested structure (grandchild cells)
            var nestedCell = new SemanticCell
            {
                Content = "This is a nested cell with its own children.",
                Position = 2,
                Metadata = new { Type = "Section", Title = "Details" }
            };

            var grandchildCell1 = new SemanticCell
            {
                Content = "First subsection with detailed information.",
                Position = 0,
                Metadata = new { Type = "Subsection", Title = "Detail 1" }
            };

            var grandchildCell2 = new SemanticCell
            {
                Content = "Second subsection with additional context.",
                Position = 1,
                Metadata = new { Type = "Subsection", Title = "Detail 2" }
            };

            // Add the grandchild cells to the nested cell
            nestedCell.Children.Add(grandchildCell1);
            nestedCell.Children.Add(grandchildCell2);

            // Add all child cells to the parent
            parentCell.Children.Add(childTextCell);
            parentCell.Children.Add(childListCell);
            parentCell.Children.Add(nestedCell);

            // Create a complex cell with multiple levels of nesting
            SerializeAndPrint(parentCell);

            // Count the total number of cells in the hierarchy
            Console.WriteLine($"Total cells in hierarchy: {parentCell.CountSemanticCells()}");
        }

        private static void TestCellWithChunks()
        {
            Console.WriteLine("\nTest 9: Cell with Chunks and Embeddings");
            Console.WriteLine("--------------------------------------");

            // Create a cell with a long piece of text that's chunked
            string longText = "This is a lengthy piece of text that would typically be broken into multiple chunks for processing by language models. " +
                "Each chunk would maintain its semantic meaning while being appropriately sized for model processing. " +
                "Chunks can be created based on natural breaks in the content, such as paragraphs, sentences, or other semantic boundaries. " +
                "The chunking process is important for working with large documents that exceed model context windows. " +
                "Each chunk can then have its own embeddings generated, which allows for semantic search and retrieval.";

            var cell = new SemanticCell
            {
                Content = longText,
                Metadata = new { Type = "Document", Length = longText.Length }
            };

            // Create chunks for this cell
            // Chunk 1
            var chunk1 = new SemanticChunk
            {
                Content = "This is a lengthy piece of text that would typically be broken into multiple chunks for processing by language models.",
                Position = 0,
                Start = 0,
                End = 104
            };
            // Add mock embeddings
            chunk1.Embeddings.AddRange(GenerateMockEmbeddings(384));

            // Chunk 2
            var chunk2 = new SemanticChunk
            {
                Content = "Each chunk would maintain its semantic meaning while being appropriately sized for model processing.",
                Position = 1,
                Start = 105,
                End = 197
            };
            chunk2.Embeddings.AddRange(GenerateMockEmbeddings(384));

            // Chunk 3
            var chunk3 = new SemanticChunk
            {
                Content = "Chunks can be created based on natural breaks in the content, such as paragraphs, sentences, or other semantic boundaries.",
                Position = 2,
                Start = 198,
                End = 311
            };
            chunk3.Embeddings.AddRange(GenerateMockEmbeddings(384));

            // Chunk 4
            var chunk4 = new SemanticChunk
            {
                Content = "The chunking process is important for working with large documents that exceed model context windows.",
                Position = 3,
                Start = 312,
                End = 406
            };
            chunk4.Embeddings.AddRange(GenerateMockEmbeddings(384));

            // Chunk 5
            var chunk5 = new SemanticChunk
            {
                Content = "Each chunk can then have its own embeddings generated, which allows for semantic search and retrieval.",
                Position = 4,
                Start = 407,
                End = 503
            };
            chunk5.Embeddings.AddRange(GenerateMockEmbeddings(384));

            // Add chunks to the cell
            cell.Chunks.Add(chunk1);
            cell.Chunks.Add(chunk2);
            cell.Chunks.Add(chunk3);
            cell.Chunks.Add(chunk4);
            cell.Chunks.Add(chunk5);

            // Display information about the chunks
            Console.WriteLine("\nChunk details:");
            foreach (var chunk in cell.Chunks)
            {
                Console.WriteLine($"  Chunk {chunk.Position}:");
                Console.WriteLine($"    Start: {chunk.Start}, End: {chunk.End}");
                Console.WriteLine($"    Length: {chunk.Length}");
                Console.WriteLine($"    Content: {chunk.Content}");
                Console.WriteLine($"    Embedding dimensions: {chunk.Embeddings.Count}");
                Console.WriteLine($"    SHA256: {chunk.SHA256Hash.ToHexString()}");
            }

            SerializeAndPrint(cell);

            // Display some statistics about the chunks
            Console.WriteLine($"\nNumber of chunks: {cell.Chunks.Count}");
            Console.WriteLine($"Number of embeddings: {cell.CountEmbeddings()}");
            Console.WriteLine($"Total bytes in chunks: {cell.CountBytes()}");

            // Demonstrate searching for a chunk by SHA256
            Console.WriteLine("\nDemonstrating chunk search by SHA256:");
            byte[] targetHash = cell.Chunks[2].SHA256Hash;
            Console.WriteLine($"Searching for chunk with SHA256: {targetHash.ToHexString()}");
            var foundChunks = SemanticCell.AllChunksBySHA256(new List<SemanticCell> { cell }, targetHash).ToList();
            Console.WriteLine($"Found {foundChunks.Count} chunks with matching SHA256 hash");
            if (foundChunks.Count > 0)
            {
                Console.WriteLine($"Found chunk content: {foundChunks[0].Content}");
            }
        }

        private static void TestComplexOperations()
        {
            Console.WriteLine("\nTest 10: Complex Operations");
            Console.WriteLine("--------------------------");

            // Create a complex hierarchical structure with various types of cells and chunks
            var rootCell = new SemanticCell
            {
                Content = "Root document with multiple sections and types of content",
                Metadata = new { DocumentType = "Report", Title = "Comprehensive Analysis", Version = "1.0" }
            };

            // Add some sections as child cells
            var introductionCell = new SemanticCell
            {
                Content = "Introduction to the topic and overview of key findings.",
                Position = 0,
                Metadata = new { Section = "Introduction", Order = 1 }
            };

            // Display hash values for the introduction cell
            Console.WriteLine("\nHash values for introduction cell content:");
            Console.WriteLine($"MD5: {introductionCell.MD5Hash.ToHexString()}");
            Console.WriteLine($"SHA1: {introductionCell.SHA1Hash.ToHexString()}");
            Console.WriteLine($"SHA256: {introductionCell.SHA256Hash.ToHexString()}");

            // Add chunks to introduction
            var introChunk1 = new SemanticChunk
            {
                Content = "Introduction to the topic",
                Position = 0,
                Start = 0,
                End = 24
            };
            introChunk1.Embeddings.AddRange(GenerateMockEmbeddings(384));

            var introChunk2 = new SemanticChunk
            {
                Content = "overview of key findings",
                Position = 1,
                Start = 26,
                End = 51
            };
            introChunk2.Embeddings.AddRange(GenerateMockEmbeddings(384));

            introductionCell.Chunks.Add(introChunk1);
            introductionCell.Chunks.Add(introChunk2);

            // Data section with a table
            DataTable dataTable = new DataTable("AnalysisResults");
            dataTable.Columns.Add("Category", typeof(string));
            dataTable.Columns.Add("Value", typeof(double));
            dataTable.Columns.Add("Change", typeof(double));

            dataTable.Rows.Add("Category A", 125.7, 10.3);
            dataTable.Rows.Add("Category B", 89.2, -5.1);
            dataTable.Rows.Add("Category C", 203.4, 15.8);
            dataTable.Rows.Add("Category D", 56.9, 2.2);

            var dataSection = new SemanticCell
            {
                Table = SerializableDataTable.FromDataTable(dataTable),
                Position = 1,
                Metadata = new { Section = "Data Analysis", Order = 2 }
            };

            // Findings section with lists
            var findingsSection = new SemanticCell
            {
                Content = "Key findings from the analysis",
                Position = 2,
                Metadata = new { Section = "Findings", Order = 3 }
            };

            var positiveFindings = new SemanticCell
            {
                UnorderedList = new List<string>
                {
                    "Strong performance in Category A with 10.3% growth",
                    "Exceptional results in Category C with 15.8% increase",
                    "Slight improvement in Category D"
                },
                Position = 0,
                Metadata = new { Subsection = "Positive Findings" }
            };

            var negativeFindings = new SemanticCell
            {
                UnorderedList = new List<string>
                {
                    "Decline in Category B by 5.1%",
                    "Below target performance in Category D despite growth"
                },
                Position = 1,
                Metadata = new { Subsection = "Areas for Improvement" }
            };

            findingsSection.Children.Add(positiveFindings);
            findingsSection.Children.Add(negativeFindings);

            // Conclusion section with binary attachment
            var conclusionSection = new SemanticCell
            {
                Content = "Conclusion and recommendations based on the analysis.",
                Position = 3,
                Metadata = new { Section = "Conclusion", Order = 4 }
            };

            var recommendation = new SemanticCell
            {
                OrderedList = new List<string>
                {
                    "Focus resources on expanding Category C",
                    "Investigate causes for decline in Category B",
                    "Develop strategy to improve Category D performance",
                    "Maintain current approach for Category A"
                },
                Position = 0,
                Metadata = new { Subsection = "Recommendations" }
            };

            // Add sample binary data as an attachment
            var attachment = new SemanticCell
            {
                Binary = Encoding.UTF8.GetBytes("Mock data for a PDF report attachment"),
                Position = 1,
                Metadata = new { Subsection = "Attachment", FileType = "PDF", FileName = "detailed_analysis.pdf" }
            };

            conclusionSection.Children.Add(recommendation);
            conclusionSection.Children.Add(attachment);

            // Add all sections to the root cell
            rootCell.Children.Add(introductionCell);
            rootCell.Children.Add(dataSection);
            rootCell.Children.Add(findingsSection);
            rootCell.Children.Add(conclusionSection);

            // Serialize the complex structure
            SerializeAndPrint(rootCell);

            // Perform operations on the structure
            Console.WriteLine($"Total cells: {SemanticCell.CountSemanticCells(new List<SemanticCell> { rootCell })}");
            Console.WriteLine($"Total chunks: {SemanticCell.CountSemanticChunks(new List<SemanticCell> { rootCell })}");
            Console.WriteLine($"Total embeddings: {SemanticCell.CountEmbeddings(new List<SemanticCell> { rootCell })}");

            // Find all cells with chunks
            var cellsWithChunks = SemanticCell.FindCellsWithChunks(new List<SemanticCell> { rootCell });
            Console.WriteLine($"Cells with chunks: {cellsWithChunks.Count()}");

            // Get all chunks
            var allChunks = SemanticCell.AllChunks(new List<SemanticCell> { rootCell });
            Console.WriteLine($"All chunks: {allChunks.Count()}");

            // Demonstrate getting distinct SHA256 hashes
            var distinctHashes = rootCell.GetDistinctSHA256Hashes();
            Console.WriteLine($"Distinct SHA256 hashes: {distinctHashes.Count()}");

            // Test equality between cells
            var cellCopy = JsonSerializer.Deserialize<SemanticCell>(
                JsonSerializer.Serialize(introductionCell, GetJsonSerializerOptions())
            );

            Console.WriteLine($"Cell equality test (same content): {introductionCell.Equals(cellCopy)}");
            Console.WriteLine($"Cell equality test (different content): {introductionCell.Equals(findingsSection)}");
        }

        private static void TestStaticAnalysisMethods()
        {
            Console.WriteLine("\nTest 11: Static Analysis Methods");
            Console.WriteLine("-------------------------------");

            // Create a collection of cells for testing
            var cells = new List<SemanticCell>();

            // Add some test cells
            var cell1 = new SemanticCell { Content = "First test cell" };
            var chunk1 = new SemanticChunk { Content = "First chunk", Position = 0 };
            chunk1.Embeddings.AddRange(GenerateMockEmbeddings(4));
            cell1.Chunks.Add(chunk1);

            var cell2 = new SemanticCell { Content = "Second test cell" };
            var chunk2 = new SemanticChunk { Content = "Second chunk", Position = 0 };
            chunk2.Embeddings.AddRange(GenerateMockEmbeddings(4));
            cell2.Chunks.Add(chunk2);

            // Add a child to cell2
            var childCell = new SemanticCell { Content = "Child cell of second cell" };
            var childChunk = new SemanticChunk { Content = "Child chunk", Position = 0 };
            childChunk.Embeddings.AddRange(GenerateMockEmbeddings(4));
            childCell.Chunks.Add(childChunk);
            cell2.Children.Add(childCell);

            // Add cells to collection
            cells.Add(cell1);
            cells.Add(cell2);

            // Debug validation - print GUID of each cell to verify uniqueness
            Console.WriteLine("\nDebug - Cell GUIDs:");
            Console.WriteLine($"cell1 GUID: {cell1.GUID}");
            Console.WriteLine($"cell2 GUID: {cell2.GUID}");
            Console.WriteLine($"childCell GUID: {childCell.GUID}");

            // Test static methods
            Console.WriteLine("\nStatic Analysis Results:");
            Console.WriteLine($"Total cells (including children): {SemanticCell.CountSemanticCells(cells)}");
            Console.WriteLine($"Total chunks: {SemanticCell.CountSemanticChunks(cells)}");
            Console.WriteLine($"Total embeddings: {SemanticCell.CountEmbeddings(cells)}");
            Console.WriteLine($"Total bytes: {SemanticCell.CountBytes(cells)}");

            // Test AllCells method with detailed output
            var allCells = SemanticCell.AllCells(cells).ToList();
            Console.WriteLine($"\nAllCells count: {allCells.Count}");
            Console.WriteLine("AllCells contents:");
            foreach (var cell in allCells)
            {
                Console.WriteLine($"  Cell GUID: {cell.GUID}, Content: {cell.Content}");
            }

            // Test AllCellsWithChunks method
            var cellsWithChunks = SemanticCell.AllCellsWithChunks(cells).ToList();
            Console.WriteLine($"\nCellsWithChunks count: {cellsWithChunks.Count}");
            Console.WriteLine("CellsWithChunks contents:");
            foreach (var cell in cellsWithChunks)
            {
                Console.WriteLine($"  Cell GUID: {cell.GUID}, Content: {cell.Content}, Chunks: {cell.Chunks.Count}");
            }

            // Test AllChunks method
            var allChunks = SemanticCell.AllChunks(cells).ToList();
            Console.WriteLine($"\nAllChunks count: {allChunks.Count}");
            Console.WriteLine("AllChunks contents:");
            foreach (var chunk in allChunks)
            {
                Console.WriteLine($"  Chunk GUID: {chunk.GUID}, Content: {chunk.Content}");
            }

            // Test GetDistinctSHA256Hashes method
            var distinctHashes = SemanticCell.GetDistinctSHA256Hashes(cells).ToList();
            Console.WriteLine($"\nDistinct SHA256 hashes: {distinctHashes.Count}");

            // Save the test cells for inspection
            SerializeAndPrint(cell2);
        }

        private static List<float> GenerateMockEmbeddings(int dimensions)
        {
            // Generate mock embedding vector for testing
            var random = new Random();
            var embeddings = new List<float>();

            for (int i = 0; i < dimensions; i++)
            {
                embeddings.Add((float)random.NextDouble() * 2 - 1); // Values between -1 and 1
            }

            return embeddings;
        }

        private static void SerializeAndPrint(SemanticCell cell)
        {
            // Serialize the cell to JSON
            var options = GetJsonSerializerOptions();
            string json = JsonSerializer.Serialize(cell, options);

            Console.WriteLine("JSON:");
            Console.WriteLine(json);

            // Print additional information
            Console.WriteLine($"Cell Type: {cell.CellType}");
            Console.WriteLine($"Created UTC: {cell.CreatedUtc}");
            Console.WriteLine($"Length (from getter): {cell.Length}");
            Console.WriteLine($"Children Count: {cell.Children.Count}");
            Console.WriteLine($"Chunks Count: {cell.Chunks.Count}");

            if (cell.SHA256Hash != null)
            {
                Console.WriteLine($"SHA256 Hash: {cell.SHA256Hash.ToHexString()}");
            }
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter() }
            };
        }
    }
}