namespace SemanticCells
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using SerializableDataTables;

    /// <summary>
    /// A semantic cell is a node that contains semantically-correlated data.  
    /// Semantic cells can be nested into a hierarchy using the 'Children' property.
    /// Cell contents can be chunked and then stored in the 'Chunks' property.
    /// Semantic cell objects are not thread-safe.
    /// </summary>
    public class SemanticCell
    {
        #region Public-Members

        /// <summary>
        /// GUID.
        /// </summary>
        public Guid GUID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Timestamp in UTC time from creation.
        /// </summary>
        public DateTime CreatedUtc { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Semantic cell type.
        /// </summary>
        public SemanticCellTypeEnum CellType { get; private set; } = SemanticCellTypeEnum.Text;

        /// <summary>
        /// MD5 hash.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte[] MD5Hash { get; private set; } = null;

        /// <summary>
        /// SHA1 hash.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte[] SHA1Hash { get; private set; } = null;

        /// <summary>
        /// SHA256 hash.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte[] SHA256Hash { get; private set; } = null;

        /// <summary>
        /// Position.
        /// </summary>
        public int Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(Position));
                _Position = value;
            }
        }

        /// <summary>
        /// Length.
        /// </summary>
        public int Length
        {
            get
            {
                return _Length;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(Length));
                _Length = value;
            }
        }

        /// <summary>
        /// Bounding box.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BoundingBox BoundingBox { get; set; } = null;

        /// <summary>
        /// Binary data.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte[] Binary
        {
            get
            {
                return _Binary;
            }
            set
            {
                ResetValues();

                if (value != null)
                {
                    _Length = value.Length;

                    CellType = SemanticCellTypeEnum.Binary;
                    MD5Hash = HashHelper.MD5Hash(value);
                    SHA1Hash = HashHelper.SHA1Hash(value);
                    SHA256Hash = HashHelper.SHA256Hash(value);
                }

                _Binary = value;
            }
        }

        /// <summary>
        /// Content.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                ResetValues();

                if (!String.IsNullOrEmpty(value))
                {
                    _Length = value.Length;

                    CellType = SemanticCellTypeEnum.Text;
                    MD5Hash = HashHelper.MD5Hash(value);
                    SHA1Hash = HashHelper.SHA1Hash(value);
                    SHA256Hash = HashHelper.SHA256Hash(value);
                }

                _Content = value;
            }
        }

        /// <summary>
        /// Unordered list elements.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> UnorderedList
        {
            get
            {
                return _UnorderedList;
            }
            set
            {
                ResetValues();

                if (value != null)
                {
                    _Length = value.Sum(s => s.Length);

                    CellType = SemanticCellTypeEnum.UnorderedList;
                    MD5Hash = HashHelper.MD5Hash(value);
                    SHA1Hash = HashHelper.SHA1Hash(value);
                    SHA256Hash = HashHelper.SHA256Hash(value);
                }

                _UnorderedList = value;
            }
        }

        /// <summary>
        /// Ordered list elements.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> OrderedList
        {
            get
            {
                return _OrderedList;
            }
            set
            {
                ResetValues();

                if (value != null)
                {
                    _Length = value.Sum(s => s.Length);

                    CellType = SemanticCellTypeEnum.OrderedList;
                    MD5Hash = HashHelper.MD5Hash(value);
                    SHA1Hash = HashHelper.SHA1Hash(value);
                    SHA256Hash = HashHelper.SHA256Hash(value);
                }

                _OrderedList = value;
            }
        }

        /// <summary>
        /// Data table.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SerializableDataTable Table
        {
            get
            {
                return _Table;
            }
            set
            {
                ResetValues();

                if (value != null)
                {
                    DataTable dt = value.ToDataTable();

                    _Length = DataTableHelper.GetLength(dt);

                    CellType = SemanticCellTypeEnum.Table;
                    MD5Hash = HashHelper.MD5Hash(dt);
                    SHA1Hash = HashHelper.SHA1Hash(dt);
                    SHA256Hash = HashHelper.SHA256Hash(dt);
                }

                _Table = value;
            }
        }

        /// <summary>
        /// Object of semi-structured data such as JSON or XML.  The supplied object must be JSON serializable.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Object
        {
            get
            {
                return _Object;
            }
            set
            {
                ResetValues();

                if (value != null)
                {
                    string json = JsonSerializer.Serialize(value);

                    _Length = json.Length;

                    CellType = SemanticCellTypeEnum.Object;
                    MD5Hash = HashHelper.MD5Hash(json);
                    SHA1Hash = HashHelper.SHA1Hash(json);
                    SHA256Hash = HashHelper.SHA256Hash(json);
                }

                _Object = value;
            }
        }

        /// <summary>
        /// Array of semi-structured data such as JSON or XML.  The supplied list must contain objects that are JSON serializable.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<object> Array
        {
            get
            {
                return _Array;
            }
            set
            {
                ResetValues();

                if (value != null)
                {
                    string json = JsonSerializer.Serialize(value);

                    _Length = json.Length;

                    CellType = SemanticCellTypeEnum.Array;
                    MD5Hash = HashHelper.MD5Hash(json);
                    SHA1Hash = HashHelper.SHA1Hash(json);
                    SHA256Hash = HashHelper.SHA256Hash(json);
                }

                _Array = value;
            }
        }

        /// <summary>
        /// Chunks.
        /// </summary>
        public List<SemanticChunk> Chunks
        {
            get
            {
                return _Chunks;
            }
            set
            {
                if (value == null) value = new List<SemanticChunk>();
                _Chunks = value;
            }
        }

        /// <summary>
        /// Children.
        /// </summary>
        public List<SemanticCell> Children
        {
            get
            {
                return _Children;
            }
            set
            {
                if (value == null) value = new List<SemanticCell>();
                _Children = value;
            }
        }

        /// <summary>
        /// User-supplied metadata.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Metadata { get; set; } = null;

        #endregion

        #region Private-Members

        private int _Position = 0;
        private int _Length = 0;

        private byte[] _Binary = null;
        private string _Content = null;
        private List<string> _UnorderedList = null;
        private List<string> _OrderedList = null;
        private SerializableDataTable _Table = null;
        private object _Object = null;
        private List<object> _Array = null;

        private List<SemanticChunk> _Chunks = new List<SemanticChunk>();
        private List<SemanticCell> _Children = new List<SemanticCell>();

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// A semantic cell is a node that contains semantically-correlated data.  
        /// Semantic cells can be nested into a hierarchy using the 'Children' property.
        /// Cell contents can be chunked and then stored in the 'Chunks' property.
        /// </summary>
        public SemanticCell()
        {

        }
        
        /// <summary>
        /// Retrieve all semantic cells in a list through recursion.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>Semantic cells.</returns>
        public static IEnumerable<SemanticCell> AllCells(List<SemanticCell> cells)
        {
            if (cells != null && cells.Count > 0)
            {
                foreach (SemanticCell cell in cells)
                {
                    yield return cell;

                    if (cell.Children != null && cell.Children.Count > 0)
                    {
                        foreach (SemanticCell child in AllCells(cell.Children))
                        {
                            yield return child;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve all semantic cells with chunks from a list through recursion.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>Semantic cells.</returns>
        public static IEnumerable<SemanticCell> AllCellsWithChunks(List<SemanticCell> cells)
        {
            if (cells != null && cells.Count > 0)
            {
                foreach (SemanticCell cell in cells)
                {
                    if (cell.Chunks != null && cell.Chunks.Count > 0)
                    {
                        yield return cell;
                    }

                    if (cell.Children != null && cell.Children.Count > 0)
                    {
                        foreach (SemanticCell child in AllCellsWithChunks(cell.Children))
                        {
                            yield return child;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve all semantic chunks.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>Semantic chunks.</returns>
        public static IEnumerable<SemanticChunk> AllChunks(List<SemanticCell> cells)
        {
            if (cells == null) return Enumerable.Empty<SemanticChunk>();

            return cells.SelectMany(cell => GetCellChunks(cell));
        }

        /// <summary>
        /// Retrieve all semantic chunks matching a given SHA-256 value.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <param name="sha256Hash">SHA-256.</param>
        /// <returns>Semantic chunks.</returns>
        public static IEnumerable<SemanticChunk> AllChunksBySHA256(List<SemanticCell> cells, byte[] sha256Hash)
        {
            if (sha256Hash == null || sha256Hash.Length < 1) throw new ArgumentNullException(nameof(sha256Hash));
            if (cells == null) return Enumerable.Empty<SemanticChunk>();

            return AllChunks(cells).Where(chunk =>
                chunk.SHA256Hash != null
                && chunk.SHA256Hash.Equals(sha256Hash));
        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Determine equality with another cell.
        /// </summary>
        /// <param name="cell">Semantic cell.</param>
        /// <returns>True if equal.</returns>
        public bool Equals(SemanticCell cell)
        {
            if (cell == null) return false;

            return SHA256Hash != null
                && cell.SHA256Hash != null 
                && SHA256Hash.SequenceEqual(cell.SHA256Hash);
        }

        /// <summary>
        /// Determine equality with another object.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj) => Equals(obj as SemanticCell);

        /// <summary>
        /// Retrieve hash code.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return SHA256Hash != null ? BitConverter.ToInt32(SHA256Hash, 0) : 0;
        }

        /// <summary>
        /// Count the number of embeddings in a list of semantic cells.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>Number of embeddings.</returns>
        public static int CountEmbeddings(List<SemanticCell> cells)
        {
            int ret = 0;

            if (cells != null && cells.Count > 0)
            {
                foreach (SemanticCell cell in cells)
                {
                    ret += cell.CountEmbeddings();
                }
            }

            return ret;
        }

        /// <summary>
        /// Count the number of semantic cells in a list of semantic cells.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>Number of semantic cells.</returns>
        public static int CountSemanticCells(List<SemanticCell> cells)
        {
            int ret = 0;

            if (cells != null && cells.Count > 0)
            {
                foreach (SemanticCell cell in cells)
                {
                    ret += 1;

                    if (cell.Children != null)
                    {
                        ret += CountSemanticCells(cell.Children);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Count the number of semantic chunks in a list of semantic cells.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>Number of chunks.</returns>
        public static int CountSemanticChunks(List<SemanticCell> cells)
        {
            int ret = 0;

            if (cells != null && cells.Count > 0)
            {
                foreach (SemanticCell cell in cells)
                {
                    if (cell.Chunks != null) ret += cell.Chunks.Count;

                    if (cell.Children != null)
                    {
                        int childChunks = CountSemanticChunks(cell.Children);
                        ret += childChunks;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Count the number of bytes in chunks within a list of semantic cells.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>Number of bytes.</returns>
        public static int CountBytes(List<SemanticCell> cells)
        {
            int ret = 0;

            if (cells != null && cells.Count > 0)
            {
                foreach (SemanticCell cell in cells)
                {
                    ret += cell.CountBytes();
                }
            }

            return ret;
        }

        /// <summary>
        /// Count the number of semantic cells in this semantic cell.
        /// </summary>
        /// <returns>Number of semantic cells.</returns>
        public int CountSemanticCells()
        {
            int ret = 1;

            if (Children != null && Children.Count > 0)
            {
                foreach (SemanticCell child in Children)
                {
                    ret += child.CountSemanticCells();
                }
            }

            return ret;
        }

        /// <summary>
        /// Count the number of embeddings contained within the semantic cell.
        /// </summary>
        /// <returns></returns>
        public int CountEmbeddings()
        {
            int ret = 0;

            if (Chunks != null && Chunks.Count > 0)
            {
                foreach (SemanticChunk chunk in Chunks)
                {
                    ret += chunk.Embeddings.Count;
                }
            }

            if (Children != null && Children.Count > 0)
            {
                foreach (SemanticCell childCell in Children)
                {
                    int childEmbeddings = childCell.CountEmbeddings();
                    ret += childEmbeddings;
                }
            }

            return ret;
        }

        /// <summary>
        /// Count the number of bytes contained within chunks within the semantic cell.
        /// </summary>
        /// <returns>Number of bytes.</returns>
        public int CountBytes()
        {
            int ret = 0;

            ret += Length;

            if (Chunks != null && Chunks.Count > 0)
            {
                foreach (SemanticChunk chunk in Chunks)
                {
                    ret += chunk.Length;
                }
            }

            if (Children != null && Children.Count > 0)
            {
                foreach (SemanticCell child in Children)
                {
                    ret += child.CountBytes();
                }
            }

            return ret;
        }

        /// <summary>
        /// Retrieve the distinct SHA-256 hash values from the chunks contained within in this cell and its children.
        /// </summary>
        /// <returns>SHA-256 hash values.</returns>
        public IEnumerable<byte[]> GetDistinctSHA256Hashes()
        {
            return (Chunks ?? Enumerable.Empty<SemanticChunk>())
                .Select(chunk => chunk?.SHA256Hash)
                .Concat((Children ?? Enumerable.Empty<SemanticCell>())
                    .SelectMany(child => child?.GetDistinctSHA256Hashes() ?? Enumerable.Empty<byte[]>()))
                .Where(hash => hash != null && hash.Length > 0)
                .Distinct();
        }

        /// <summary>
        /// Retrieve the distinct SHA-256 hash values from the chunks contained within a list of semantic cells and their children.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>SHA-256 hash values.</returns>
        public static IEnumerable<byte[]> GetDistinctSHA256Hashes(IEnumerable<SemanticCell> cells)
        {
            return (cells ?? Enumerable.Empty<SemanticCell>())
                .SelectMany(cell => cell?.GetDistinctSHA256Hashes() ?? Enumerable.Empty<byte[]>())
                .Distinct();
        }

        /// <summary>
        /// Retrieve semantic cells that contain chunks.
        /// </summary>
        /// <param name="cells">Semantic cells.</param>
        /// <returns>Enumerable of cells that contain chunks.</returns>
        public static IEnumerable<SemanticCell> FindCellsWithChunks(List<SemanticCell> cells)
        {
            if (cells == null) return Enumerable.Empty<SemanticCell>();

            var results = new List<SemanticCell>();

            foreach (var cell in cells)
            {
                if (cell.Chunks?.Any() == true)
                {
                    results.Add(cell);
                }

                if (cell.Children?.Any() == true)
                {
                    results.AddRange(FindCellsWithChunks(cell.Children));
                }
            }

            return results;
        }

        #endregion

        #region Private-Methods

        private static IEnumerable<SemanticChunk> GetCellChunks(SemanticCell cell)
        {
            if (cell == null)
                return Enumerable.Empty<SemanticChunk>();

            var directChunks = cell.Chunks ?? Enumerable.Empty<SemanticChunk>();

            var childChunks = (cell.Children ?? Enumerable.Empty<SemanticCell>())
                .SelectMany(child => GetCellChunks(child));

            return directChunks.Concat(childChunks);
        }

        private void ResetValues()
        {
            MD5Hash = null;
            SHA1Hash = null;
            SHA256Hash = null;

            _Length = 0;

            _Content = null;
            _Binary = null;
            _UnorderedList = null;
            _OrderedList = null;
            _Table = null;
            _Object = null;
            _Array = null;
        }

        #endregion
    }
}
