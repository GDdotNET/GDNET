using GDNET.Client.Attributes;
using GDNET.Client.Encryption;

namespace GDNET.Client.Objects.Special
{
    /// <summary>
    /// A text object.
    /// </summary>
    [LevelObject("914")]
    public abstract class TextObject : Object
    {
        private string text;

        /// <summary>
        /// The text belonging to this object.
        /// </summary>
        [LevelObject("31")]
        public string Text
        {
            get => text;
            set => text = Base64.Decode(value);
        }
    }
}