namespace SemanticCells
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    /// <summary>
    /// A semantic chunk is a portion of the data found within a semantic cell.
    /// Chunks are normally determined based on a variety of different situations including language model limitations, semantic boundaries, or logical sub-grouping within a cell.
    /// </summary>
    public class SemanticChunk
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
        /// Start position.
        /// </summary>
        public int Start
        {
            get
            {
                return _Start;
            }
            set
            {
                if (value < 0) throw new ArgumentException("Start must be zero or greater.");
                _Start = value;
            }
        }

        /// <summary>
        /// End position.
        /// </summary>
        public int End
        {
            get
            {
                return _End;
            }
            set
            {
                if (value < 0) throw new ArgumentException("End must be zero or greater.");
                _End = value;
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

                if (value != null)
                {
                    _Length = value.Length;

                    MD5Hash = HashHelper.MD5Hash(value);
                    SHA1Hash = HashHelper.SHA1Hash(value);
                    SHA256Hash = HashHelper.SHA256Hash(value);
                }

                _Content = value;
            }
        }

        /// <summary>
        /// Embeddings.
        /// </summary>
        public List<float> Embeddings
        {
            get
            {
                return _Embeddings;
            }
            set
            {
                if (value == null) value = new List<float>();
                else _Embeddings = value;
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
        private int _Start = 0;
        private int _End = 0;
        private int _Length = 0;
        private List<float> _Embeddings = new List<float>();
        private byte[] _Binary = null;
        private string _Content = null;

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// A semantic chunk is a portion of the data found within a semantic cell.
        /// Chunks are normally determined based on a variety of different situations including language model limitations, semantic boundaries, or logical sub-grouping within a cell.
        /// </summary>
        public SemanticChunk()
        {

        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Determine equality with another chunk.
        /// </summary>
        /// <param name="chunk">Semantic chunk.</param>
        /// <returns>True if equal.</returns>
        public bool Equals(SemanticChunk chunk)
        {
            if (chunk == null) return false;

            return SHA256Hash != null
                && chunk.SHA256Hash != null
                && SHA256Hash.SequenceEqual(chunk.SHA256Hash);
        }

        /// <summary>
        /// Determine equality with another object.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj) => Equals(obj as SemanticChunk);

        /// <summary>
        /// Retrieve hash code.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return SHA256Hash != null ? BitConverter.ToInt32(SHA256Hash, 0) : 0;
        }

        #endregion

        #region Private-Methods

        private void ResetValues()
        {
            MD5Hash = null;
            SHA1Hash = null;
            SHA256Hash = null;

            _Length = 0;

            _Content = null;
            _Binary = null;
        }

        #endregion
    }
}
