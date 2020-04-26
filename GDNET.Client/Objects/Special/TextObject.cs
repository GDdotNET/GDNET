using GDNET.Client.Attributes;
using GDNET.Client.Encryption;

namespace GDNET.Client.Objects.Special
{
    /// <summary>
    /// A text object.
    /// </summary>
    [LevelObject("914")]   
    public class TextObject : Object
    {
        private string _text;

        /// <summary>
        /// The text belonging to this object.
        /// </summary>
        [LevelObject("31")]
        public string Text
        {
            get => _text;
            set => _text = Base64.Decode(value);
        }
    }
}