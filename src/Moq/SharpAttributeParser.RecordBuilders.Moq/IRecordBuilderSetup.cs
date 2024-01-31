namespace SharpAttributeParser.Moq;

using SharpAttributeParser.Moq.RecordBuilderSetups;

public interface IRecordBuilderSetup<TRecord>
{
    public abstract IRecordBuilderSetupBuild<TRecord> Build { get; }
}
