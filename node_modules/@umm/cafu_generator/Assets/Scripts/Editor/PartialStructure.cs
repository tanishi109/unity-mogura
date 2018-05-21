namespace CAFU.Generator
{
    public interface IPartialStructure
    {
        string Name { get; }

        string Render(Parameter parameter);
    }

    public abstract class PartialStructureBase : StructureBase, IPartialStructure
    {
        public abstract string Name { get; }

        public abstract string Render(Parameter parameter);
    }
}