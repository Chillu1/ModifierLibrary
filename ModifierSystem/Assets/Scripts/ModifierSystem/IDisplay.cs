using System.Text;

namespace ModifierSystem
{
    /// <summary>
    ///     Displays UI info
    /// </summary>
    public interface IDisplay
    {
        void DisplayText(StringBuilder builder);
    }
}