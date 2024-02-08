namespace SharpAttributeParser.RecordBuilders;

/// <summary>Responsible for building attribute records.</summary>
/// <typeparam name="TRecord">The type of the built attribute record.</typeparam>
public interface IRecordBuilder<out TRecord>
{
    /// <summary>Builds an attribute record.</summary>
    /// <returns>The built attribute record.</returns>
    public abstract TRecord Build();
}
