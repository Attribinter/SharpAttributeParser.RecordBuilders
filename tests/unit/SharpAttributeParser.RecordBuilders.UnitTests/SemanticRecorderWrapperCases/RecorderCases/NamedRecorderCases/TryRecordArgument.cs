namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases.RecorderCases.NamedRecorderCases;

using Moq;

using System;

using Xunit;

public sealed class TryRecordArgument
{
    private static bool Target(ISemanticNamedRecorder recorder, string parameterName, object? argument) => recorder.TryRecordArgument(parameterName, argument);

    private readonly RecorderContext<object, IRecordBuilder<object>> Context = RecorderContext<object, IRecordBuilder<object>>.Create();

    [Fact]
    public void NullParameterName_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.NamedRecorder, null!, Mock.Of<object>()));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void AfterBuilt_InvalidOperationException()
    {
        Context.Recorder.BuildRecord();

        var exception = Record.Exception(() => Target(Context.NamedRecorder, string.Empty, Mock.Of<object>()));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Valid_CallsWrappedRecorder()
    {
        var parameterName = string.Empty;
        var argument = Mock.Of<object>();

        Target(Context.NamedRecorder, parameterName, argument);

        Context.WrappedNamedRecorder.Verify((recorder) => recorder.TryRecordArgument(parameterName, argument), Times.Once());
    }
}
