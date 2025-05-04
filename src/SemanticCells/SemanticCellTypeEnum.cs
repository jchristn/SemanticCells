namespace SemanticCells
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Semantic cell type.
    /// </summary>
    public enum SemanticCellTypeEnum
    {
        /// <summary>
        /// Text.
        /// </summary>
        [EnumMember(Value = "Text")]
        Text,
        /// <summary>
        /// Table.
        /// </summary>
        [EnumMember(Value = "Table")]
        Table,
        /// <summary>
        /// UnorderedList.
        /// </summary>
        [EnumMember(Value = "UnorderedList")]
        UnorderedList,
        /// <summary>
        /// OrderedList.
        /// </summary>
        [EnumMember(Value = "OrderedList")]
        OrderedList,
        /// <summary>
        /// Binary.
        /// </summary>
        [EnumMember(Value = "Binary")]
        Binary,
        /// <summary>
        /// Object.
        /// </summary>
        [EnumMember(Value = "Object")]
        Object,
        /// <summary>
        /// Array.
        /// </summary>
        [EnumMember(Value = "Array")]
        Array
    }
}
