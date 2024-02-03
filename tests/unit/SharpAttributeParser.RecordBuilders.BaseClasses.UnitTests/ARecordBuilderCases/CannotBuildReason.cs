namespace SharpAttributeParser.ARecordBuilderCases;

using Moq;

using Xunit;

public sealed class CannotBuildReason
{
    private static string Target(RecordBuilder recordBuilder) => recordBuilder.InvokeTarget();

    [Fact]
    public void NotNull()
    {
        RecordBuilder recordBuilder = new();

        var actual = Target(recordBuilder);

        Assert.NotNull(actual);
    }

    private sealed class RecordBuilder : ARecordBuilder<object>
    {
        public RecordBuilder() : base(true) { }

        public string InvokeTarget() => CannotBuildReason();

        protected override object GetRecord() => Mock.Of<object>();
    }
}
