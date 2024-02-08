﻿namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases;

using Moq;

using System;

using Xunit;

public sealed class Wrap
{
    private static ISemanticRecorder<TRecord> Target<TRecord, TRecordBuilder>(ISemanticRecorderWrapper wrapper, ISemanticRecorder<TRecordBuilder> recorder) where TRecordBuilder : IRecordBuilder<TRecord> => wrapper.Wrap<TRecord, TRecordBuilder>(recorder);

    private readonly WrapperContext Context = WrapperContext.Create();

    [Fact]
    public void NullRecorder_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target<object, IRecordBuilder<object>>(Context.Wrapper, null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Valid_NotNull()
    {
        var actial = Target<object, IRecordBuilder<object>>(Context.Wrapper, Mock.Of<ISemanticRecorder<IRecordBuilder<object>>>());

        Assert.NotNull(actial);
    }
}
