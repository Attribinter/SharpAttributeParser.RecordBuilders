namespace SharpAttributeParser.Moq.RecordBuilderSetups;

using global::Moq.Language.Flow;

using System;

public interface IRecordBuilderSetupBuild<TRecord>
{
    public abstract IReturnsResult<IRecordBuilder<TRecord>> Returns(TRecord result);
    public abstract IThrowsResult Throws<TException>() where TException : Exception, new();
    public abstract IThrowsResult Throws(Exception exception);
}
