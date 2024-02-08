namespace SharpAttributeParser.RecordBuilders.RecorderWrapperCases.RecorderCases.ConstructorRecorderCases;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

public sealed class TryRecordParamsArgument
{
    private static bool Target(IConstructorRecorder recorder, IParameterSymbol parameter, object? argument, IReadOnlyList<ExpressionSyntax> elementSyntax) => recorder.TryRecordParamsArgument(parameter, argument, elementSyntax);

    private readonly RecorderContext<object, IRecordBuilder<object>> Context = RecorderContext<object, IRecordBuilder<object>>.Create();

    [Fact]
    public void NullParameter_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, null!, Mock.Of<object>(), new[] { ExpressionSyntaxFactory.Create() }));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullElementSyntax_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, Mock.Of<IParameterSymbol>(), Mock.Of<object>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void AfterBuilt_InvalidOperationException()
    {
        Context.Recorder.BuildRecord();

        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, Mock.Of<IParameterSymbol>(), Mock.Of<object>(), new[] { ExpressionSyntaxFactory.Create() }));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Valid_CallsWrappedRecorder()
    {
        var parameter = Mock.Of<IParameterSymbol>();
        var argument = Mock.Of<object>();
        var elementSyntax = new[] { ExpressionSyntaxFactory.Create() };

        Target(Context.ConstructorRecorder, parameter, argument, elementSyntax);

        Context.WrappedConstructorRecorder.Verify((recorder) => recorder.TryRecordParamsArgument(parameter, argument, elementSyntax), Times.Once());
    }
}
