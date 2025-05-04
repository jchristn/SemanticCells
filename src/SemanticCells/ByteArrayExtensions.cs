namespace SemanticCells
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides extension methods for byte arrays.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts a byte array to a hexadecimal string representation without dashes.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A string containing the hexadecimal representation of the byte array.</returns>
        public static string ToHexString(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            var sb = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a byte array to a Base64 string representation.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A Base64 string representation of the byte array.</returns>
        public static string ToBase64String(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            return Convert.ToBase64String(bytes);
        }
    }
}