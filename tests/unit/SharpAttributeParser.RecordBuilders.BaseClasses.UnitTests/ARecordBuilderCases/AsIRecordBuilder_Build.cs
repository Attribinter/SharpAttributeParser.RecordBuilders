namespace SharpAttributeParser.Mappers.ARecordBuilderCases;

using Moq;

using System;

using Xunit;

public sealed class AsIRecordBuilder_Build
{
    private static TRecord Target<TRecord>(IRecordBuilder<TRecord> recordBuilder) => recordBuilder.Build();

    [Fact]
    public void NullReturningGetTarget_InvalidOperationException()
    {
        RecordBuilder recordBuilder = new(null, false, true, string.Empty);

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void FalseReturningCheckFullyConstructed_InvalidOperationException()
    {
        RecordBuilder recordBuilder = new(Mock.Of<object>(), false, false, string.Empty);

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void MultipleInvokationsOfBuild_ThrowOnMultipleBuildsEnabled_InvalidOperationExceptionOnSecondInvokation()
    {
        RecordBuilder recordBuilder = new(Mock.Of<object>(), true, true, string.Empty);

        ((IRecordBuilder<object>)recordBuilder).Build();

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void MultipleInvokationsOfBuild_ThrowOnMultipleBuildsDisabled_ReturnsTargetEveryTime()
    {
        var buildTarget = Mock.Of<object>();

        RecordBuilder recordBuilder = new(buildTarget, false, true, string.Empty);

        var firstBuildResult = ((IRecordBuilder<object>)recordBuilder).Build();
        var secondBuildResult = ((IRecordBuilder<object>)recordBuilder).Build();

        Assert.Same(buildTarget, firstBuildResult);
        Assert.Same(buildTarget, secondBuildResult);
    }

    [Fact]
    public void FalseCanBuild_NullCannotBuildReason_InvalidOperationException()
    {
        RecordBuilder recordBuilder = new(Mock.Of<object>(), false, false, null);

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.IsType<InvalidOperationException>(exception);
    }

    private sealed class RecordBuilder : ARecordBuilder<object>
    {
        private readonly object? BuildTarget;
        private readonly bool CanBuildReturnValue;
        private readonly string? CannotBuildReasonReturnValue;

        public RecordBuilder(object? buildTarget, bool throwOnMultipleBuilds, bool canBuildReturnValue, string? cannotBuildReasonReturnValue) : base(throwOnMultipleBuilds)
        {
            BuildTarget = buildTarget;
            CanBuildReturnValue = canBuildReturnValue;
            CannotBuildReasonReturnValue = cannotBuildReasonReturnValue;
        }

        protected override object GetRecord() => BuildTarget!;
        protected override bool CanBuild() => CanBuildReturnValue;
        protected override string CannotBuildReason() => CannotBuildReasonReturnValue!;
    }
}
