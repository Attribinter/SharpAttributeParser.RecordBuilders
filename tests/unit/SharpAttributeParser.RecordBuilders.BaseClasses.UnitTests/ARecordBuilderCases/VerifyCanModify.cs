namespace SharpAttributeParser.Mappers.ARecordBuilderCases;

using Moq;

using System;

using Xunit;

public sealed class VerifyCanModify
{
    private static void Target(RecordBuilder recordBuilder) => recordBuilder.InvokeTarget();

    [Fact]
    public void Unbuilt_NoException()
    {
        RecordBuilder recordBuilder = new();

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.Null(exception);
    }

    [Fact]
    public void Built_InvalidOperationException()
    {
        RecordBuilder recordBuilder = new();

        ((IRecordBuilder<object>)recordBuilder).Build();

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.IsType<InvalidOperationException>(exception);
    }

    private sealed class RecordBuilder : ARecordBuilder<object>
    {
        public RecordBuilder() : base(true) { }

        public void InvokeTarget() => VerifyCanModify();

        protected override object GetRecord() => Mock.Of<object>();
    }
}
