namespace SharpAttributeParser.RecordBuilders.SyntacticRecorderWrapperCases.RecorderCases.ConstructorRecorderCases;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

public sealed class TryRecordParamsArgument
{
    private static bool Target(ISyntacticConstructorRecorder recorder, IParameterSymbol parameter, IReadOnlyList<ExpressionSyntax> elementSyntax) => recorder.TryRecordParamsArgument(parameter, elementSyntax);

    private readonly RecorderContext<object, IRecordBuilder<object>> Context = RecorderContext<object, IRecordBuilder<object>>.Create();

    [Fact]
    public void NullParameter_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, null!, new[] { ExpressionSyntaxFactory.Create() }));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullElementSyntax_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, Mock.Of<IParameterSymbol>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void AfterBuilt_InvalidOperationException()
    {
        Context.Recorder.BuildRecord();

        var exception = Record.Exception(() => Target(Context.ConstructorRecorder, Mock.Of<IParameterSymbol>(), new[] { ExpressionSyntaxFactory.Create() }));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Valid_CallsWrappedRecorder()
    {
        var parameter = Mock.Of<IParameterSymbol>();
        var elementSyntax = new[] { ExpressionSyntaxFactory.Create() };

        Target(Context.ConstructorRecorder, parameter, elementSyntax);

        Context.WrappedConstructorRecorder.Verify((recorder) => recorder.TryRecordParamsArgument(parameter, elementSyntax), Times.Once());
    }
}
