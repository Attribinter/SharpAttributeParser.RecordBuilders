namespace SharpAttributeParser;

using System;

/// <summary>An abstract <see cref="IRecordBuilder{TRecord}"/>, responsible for building attribute records.</summary>
/// <typeparam name="TRecord">The type of the built attribute record.</typeparam>
public abstract class ARecordBuilder<TRecord> : IRecordBuilder<TRecord>
{
    private bool HasBeenBuilt;

    private readonly bool ThrowOnMultipleBuilds;

    private bool CannotBuildDueToAlreadyBuilt => HasBeenBuilt && ThrowOnMultipleBuilds;

    /// <summary>Instantiates a <see cref="ARecordBuilder{TRecord}"/>, reponsible for building attribute records.</summary>
    /// <param name="throwOnMultipleBuilds">Indicates whether an <see cref="InvalidOperationException"/> should be thrown if the attribute record is built more than once.</param>
    protected ARecordBuilder(bool throwOnMultipleBuilds)
    {
        ThrowOnMultipleBuilds = throwOnMultipleBuilds;
    }

    TRecord IRecordBuilder<TRecord>.Build()
    {
        if (CannotBuildDueToAlreadyBuilt)
        {
            throw new InvalidOperationException($"Cannot build the attribute record, as it has already been built.");
        }

        if (CanBuild() is false)
        {
            var reason = CannotBuildReason() ?? throw new InvalidOperationException("The reason for not being able to build the attribute record was unexpectedly null.");

            throw new InvalidOperationException(reason);
        }

        HasBeenBuilt = true;

        return GetRecord() ?? throw new InvalidOperationException($"The attribute record was unexpectedly null.");
    }

    /// <summary>Retrieves the attribute record under construction.</summary>
    /// <returns>The attribute record under construction.</returns>
    protected abstract TRecord GetRecord();

    /// <summary>Checks whether the attribute record can be built in the current state.</summary>
    /// <returns>A <see cref="bool"/> indicating whether the attribute record can be built.</returns>
    protected virtual bool CanBuild() => CannotBuildDueToAlreadyBuilt is false;

    /// <summary>Retrieves a <see cref="string"/> describing the reason the attribute record cannot be built.</summary>
    /// <returns>A <see cref="string"/> describing the reason the attribute record cannot be built.</returns>
    protected virtual string CannotBuildReason() => "Cannot build the attribute record, due to the current state of the record.";

    /// <summary>Determines whether the attribute record may be further modified.</summary>
    /// <returns>A <see cref="bool"/> indicating whether the attribute record may be further modified.</returns>
    protected bool CanModify() => HasBeenBuilt is false;

    /// <summary>Verifies that the attribute record may be further modified, and throws an <see cref="InvalidOperationException"/> otherwise.</summary>
    protected void VerifyCanModify()
    {
        if (CanModify() is false)
        {
            throw new InvalidOperationException($"Cannot modify the attribute record, as it has already been built.");
        }
    }
}
