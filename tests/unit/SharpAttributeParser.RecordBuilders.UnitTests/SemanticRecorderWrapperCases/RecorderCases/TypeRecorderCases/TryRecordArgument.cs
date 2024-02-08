namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases.RecorderCases.TypeRecorderCases;

using Microsoft.CodeAnalysis;

using Moq;

using System;

using Xunit;

public sealed class TryRecordArgument
{
    private static bool Target(ISemanticTypeRecorder recorder, ITypeParameterSymbol parameter, ITypeSymbol argument) => recorder.TryRecordArgument(parameter, argument);

    private readonly RecorderContext<object, IRecordBuilder<object>> Context = RecorderContext<object, IRecordBuilder<object>>.Create();

    [Fact]
    public void NullParameter_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.TypeRecorder, null!, Mock.Of<ITypeSymbol>()));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullArguent_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Context.TypeRecorder, Mock.Of<ITypeParameterSymbol>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void AfterBuilt_InvalidOperationException()
    {
        Context.Recorder.BuildRecord();

        var exception = Record.Exception(() => Target(Context.TypeRecorder, Mock.Of<ITypeParameterSymbol>(), Mock.Of<ITypeSymbol>()));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Valid_CallsWrappedRecorder()
    {
        var parameter = Mock.Of<ITypeParameterSymbol>();
        var argument = Mock.Of<ITypeSymbol>();

        Target(Context.TypeRecorder, parameter, argument);

        Context.WrappedTypeRecorder.Verify((recorder) => recorder.TryRecordArgument(parameter, argument), Times.Once());
    }
}
