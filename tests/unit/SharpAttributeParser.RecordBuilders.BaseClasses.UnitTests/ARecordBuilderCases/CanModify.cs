﻿namespace SharpAttributeParser.Mappers.ARecordBuilderCases;

using Moq;

using Xunit;

public sealed class CanModify
{
    private static bool Target(RecordBuilder recordBuilder) => recordBuilder.InvokeTarget();

    [Fact]
    public void Unbuilt_True()
    {
        RecordBuilder recordBuilder = new();

        var result = Target(recordBuilder);

        Assert.True(result);
    }

    [Fact]
    public void Built_False()
    {
        RecordBuilder recordBuilder = new();

        ((IRecordBuilder<object>)recordBuilder).Build();

        var result = Target(recordBuilder);

        Assert.False(result);
    }

    private sealed class RecordBuilder : ARecordBuilder<object>
    {
        public RecordBuilder() : base(true) { }

        public bool InvokeTarget() => CanModify();

        protected override object GetRecord() => Mock.Of<object>();
    }
}
