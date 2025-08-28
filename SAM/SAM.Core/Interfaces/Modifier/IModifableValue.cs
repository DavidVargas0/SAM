namespace SAM 
 // namespace  SAM.Core
{
    public interface IModifiableValue : IJSAMObject
    {
        double Value { get; }

        IModifier Modifier { get; }
    }
}