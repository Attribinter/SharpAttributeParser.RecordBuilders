namespace SharpAttributeParser.RecordBuilders;

/// <summary>Handles wrapping of record-builder targetting recorders in non-record-builder targetting recorders.</summary>
public interface IRecorderWrapper
{
    /// <summary>Wraps the provided record-builder targetting recorder in a non-record-builder targetting recorder.</summary>
    /// <typeparam name="TRecord">The type representing the recorded arguments.</typeparam>
    /// <typeparam name="TRecordBuilder">The type through which arguments are recorded, and which builds records.</typeparam>
    /// <param name="recorder">The wrapped, record-builder targetting, recorder.</param>
    /// <returns>The wrapping recorder.</returns>
    public abstract IRecorder<TRecord> Wrap<TRecord, TRecordBuilder>(IRecorder<TRecordBuilder> recorder) where TRecordBuilder : IRecordBuilder<TRecord>;
}
