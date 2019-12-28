namespace Viking.Rich
{
    /// <summary>
    /// Code for VikingRich; Rich text prefix and suffix.
    /// </summary>
    public class RichCode
    {
        /// <summary>
        /// Prefix for rich text. ie. <i>
        /// </summary>
        public string prefix;

        /// <summary>
        /// Suffix for rich text. ie. </i>
        /// </summary>
        public string suffix;

        /// <summary>
        /// Create a new code for editing rich text.
        /// </summary>
        /// <param name="prefix">Prefix for rich text. ie. <i></param>
        /// <param name="suffix">Suffix for rich text. ie. </i></param>
        public RichCode(string prefix, string suffix)
        {
            this.prefix = prefix;
            this.suffix = suffix;
        }
    }
}
