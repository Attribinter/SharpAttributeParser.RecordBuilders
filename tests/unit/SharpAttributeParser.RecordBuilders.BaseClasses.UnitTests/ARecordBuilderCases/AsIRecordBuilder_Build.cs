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
        RecordBuilder recordBuilder = new(null, false, true);

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void FalseReturningCheckFullyConstructed_InvalidOperationException()
    {
        RecordBuilder recordBuilder = new(Mock.Of<object>(), false, false);

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void MultipleInvokationsOfBuild_ThrowOnMultipleBuildsEnabled_InvalidOperationExceptionOnSecondInvokation()
    {
        RecordBuilder recordBuilder = new(Mock.Of<object>(), true, true);

        ((IRecordBuilder<object>)recordBuilder).Build();

        var exception = Record.Exception(() => Target(recordBuilder));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void MultipleInvokationsOfBuild_ThrowOnMultipleBuildsDisabled_ReturnsTargetEveryTime()
    {
        var buildTarget = Mock.Of<object>();

        RecordBuilder recordBuilder = new(buildTarget, false, true);

        var firstBuildResult = ((IRecordBuilder<object>)recordBuilder).Build();
        var secondBuildResult = ((IRecordBuilder<object>)recordBuilder).Build();

        Assert.Same(buildTarget, firstBuildResult);
        Assert.Same(buildTarget, secondBuildResult);
    }

    private sealed class RecordBuilder : ARecordBuilder<object>
    {
        private readonly object? BuildTarget;
        private readonly bool CanBuildReturnValue;

        public RecordBuilder(object? buildTarget, bool throwOnMultipleBuilds, bool canBuildReturnValue) : base(throwOnMultipleBuilds)
        {
            BuildTarget = buildTarget;
            CanBuildReturnValue = canBuildReturnValue;
        }

        protected override object GetRecord() => BuildTarget!;
        protected override bool CanBuild() => CanBuildReturnValue;
    }
}
