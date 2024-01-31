namespace SharpAttributeParser.Moq;

using global::Moq;

public interface IRecordBuilderVerification
{
    public abstract void NoOtherCalls();

    public abstract void Build(Times times);
}
