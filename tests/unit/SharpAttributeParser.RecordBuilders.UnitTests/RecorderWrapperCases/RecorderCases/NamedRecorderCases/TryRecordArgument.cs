namespace SharpAttributeParser.RecordBuilders.RecorderWrapperCases.RecorderCases.NamedRecorderCases;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using System;

using Xunit;

public sealed class TryRecordArgument
{
    private static bool Target(INamedRecorder recorder, string parameterName, object? argument, ExpressionSyntax syntax) => recorder.TryRecordArgument(parameterName, argument, syntax);

    private readonly RecorderContext<object, IRecordBuilder<object>> Context = RecorderContext<object, IRecordBuilder<object>>.Create();

    [Fact]
    public void NullParameterName_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.NamedRecorder, null!, Mock.Of<object>(), ExpressionSyntaxFactory.Create()));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullSyntax_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.NamedRecorder, string.Empty, Mock.Of<object>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void AfterBuilt_InvalidOperationException()
    {
        Context.Recorder.BuildRecord();

        var exception = Record.Exception(() => Target(Context.NamedRecorder, string.Empty, Mock.Of<object>(), ExpressionSyntaxFactory.Create()));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Valid_CallsWrappedRecorder()
    {
        var parameterName = string.Empty;
        var argument = Mock.Of<object>();
        var syntax = ExpressionSyntaxFactory.Create();

        Target(Context.NamedRecorder, parameterName, argument, syntax);

        Context.WrappedNamedRecorder.Verify((recorder) => recorder.TryRecordArgument(parameterName, argument, syntax), Times.Once());
    }
}
