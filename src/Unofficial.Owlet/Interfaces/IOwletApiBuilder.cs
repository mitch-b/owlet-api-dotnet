namespace Unofficial.Owlet.Interfaces
{
    public interface IOwletApiBuilder
    {
        /// <summary>
        /// Allow configuring Owlet API settings before runtime begins.
        /// </summary>
        /// <param name="settings">Pass a new instance of <see cref="Unofficial.Owlet.Models.OwletApiSettings"/>.</param>
        /// <returns></returns>
        IOwletApiBuilder WithSettings(IOwletApiSettings settings);
    }
}
