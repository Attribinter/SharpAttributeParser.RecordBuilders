namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases.RecorderCases.ConstructorRecorderCases;

using Microsoft.CodeAnalysis;

using Moq;

using System;

using Xunit;

public sealed class TryRecordArgument
{
    private static bool Target(ISemanticConstructorRecorder recorder, IParameterSymbol parameter, object? argument) => recorder.TryRecordArgument(parameter, argument);

    private readonly RecorderContext<object, IRecordBuilder<object>> Context = RecorderContext<object, IRecordBuilder<object>>.Create();

    [Fact]
    public void NullParameter_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, null!, Mock.Of<object>()));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void AfterBuilt_InvalidOperationException()
    {
        Context.Recorder.BuildRecord();

        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, Mock.Of<IParameterSymbol>(), Mock.Of<object>()));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Valid_CallsWrappedRecorder()
    {
        var parameter = Mock.Of<IParameterSymbol>();
        var argument = Mock.Of<object>();

        Target(Context.ConstructorRecorder, parameter, argument);

        Context.WrappedConstructorRecorder.Verify((recorder) => recorder.TryRecordArgument(parameter, argument), Times.Once());
    }
}
