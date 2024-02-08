namespace SharpAttributeParser.RecordBuilders.RecorderWrapperCases.RecorderCases.ConstructorRecorderCases;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using System;

using Xunit;

public sealed class TryRecordArgument
{
    private static bool Target(IConstructorRecorder recorder, IParameterSymbol parameter, object? argument, ExpressionSyntax syntax) => recorder.TryRecordArgument(parameter, argument, syntax);

    private readonly RecorderContext<object, IRecordBuilder<object>> Context = RecorderContext<object, IRecordBuilder<object>>.Create();

    [Fact]
    public void NullParameter_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, null!, Mock.Of<object>(), ExpressionSyntaxFactory.Create()));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullSyntax_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, Mock.Of<IParameterSymbol>(), Mock.Of<object>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void AfterBuilt_InvalidOperationException()
    {
        Context.Recorder.BuildRecord();

        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, Mock.Of<IParameterSymbol>(), Mock.Of<object>(), ExpressionSyntaxFactory.Create()));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Valid_CallsWrappedRecorder()
    {
        var parameter = Mock.Of<IParameterSymbol>();
        var argument = Mock.Of<object>();
        var syntax = ExpressionSyntaxFactory.Create();

        Target(Context.ConstructorRecorder, parameter, argument, syntax);

        Context.WrappedConstructorRecorder.Verify((recorder) => recorder.TryRecordArgument(parameter, argument, syntax), Times.Once());
    }
}
