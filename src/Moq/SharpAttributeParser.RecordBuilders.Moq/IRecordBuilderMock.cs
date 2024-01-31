namespace SharpAttributeParser.Moq;

using global::Moq;

public interface IRecordBuilderMock<TRecord>
{
    public abstract Mock<IRecordBuilder<TRecord>> Mock { get; }
    public abstract IRecordBuilder<TRecord> Object { get; }
    public abstract IRecordBuilderSetup<TRecord> Setup { get; }
    public abstract IRecordBuilderVerification Verify { get; }
}
