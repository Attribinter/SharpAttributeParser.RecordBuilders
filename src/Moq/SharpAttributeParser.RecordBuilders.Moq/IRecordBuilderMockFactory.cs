namespace SharpAttributeParser.Moq;

public interface IRecordBuilderMockFactory
{
    public abstract IRecordBuilderMock<TRecord> Create<TRecord>();
}
