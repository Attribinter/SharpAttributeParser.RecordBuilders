namespace SharpAttributeParser.Mappers.ARecordBuilderCases;

using Moq;

using Xunit;

public sealed class CanBuild
{
    private static bool Target(RecordBuilder recordBuilder) => recordBuilder.InvokeTarget();

    [Fact]
    public void Unbuilt_True()
    {
        RecordBuilder recordBuilder = new(true);

        var result = Target(recordBuilder);

        Assert.True(result);
    }

    [Fact]
    public void Built_DontThrowOnMultipleBuilds_True()
    {
        RecordBuilder recordBuilder = new(false);

        ((IRecordBuilder<object>)recordBuilder).Build();

        var result = Target(recordBuilder);

        Assert.True(result);
    }

    [Fact]
    public void Built_ThrowOnMultipleBuilds_False()
    {
        RecordBuilder recordBuilder = new(true);

        ((IRecordBuilder<object>)recordBuilder).Build();

        var result = Target(recordBuilder);

        Assert.False(result);
    }

    private sealed class RecordBuilder : ARecordBuilder<object>
    {
        public RecordBuilder(bool throwOnMultipleBuilds) : base(throwOnMultipleBuilds) { }

        public bool InvokeTarget() => CanBuild();

        protected override object GetRecord() => Mock.Of<object>();
    }
}
