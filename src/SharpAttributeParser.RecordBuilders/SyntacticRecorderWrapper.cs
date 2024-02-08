namespace SharpAttributeParser.RecordBuilders;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;

/// <inheritdoc cref="ISyntacticRecorderWrapper"/>
public sealed class SyntacticRecorderWrapper : ISyntacticRecorderWrapper
{
    /// <summary>Instantiates a <see cref="SyntacticRecorderWrapper"/>, handles wrapping of record-builder targetting recorders in non-record-builder targetting recorders.</summary>
    public SyntacticRecorderWrapper() { }

    ISyntacticRecorder<TRecord> ISyntacticRecorderWrapper.Wrap<TRecord, TRecordBuilder>(ISyntacticRecorder<TRecordBuilder> recorder)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        return new Recorder<TRecord, TRecordBuilder>(recorder);
    }

    private sealed class Recorder<TRecord, TRecordBuilder> : ISyntacticRecorder<TRecord> where TRecordBuilder : IRecordBuilder<TRecord>
    {
        private readonly ISyntacticRecorder<TRecordBuilder> WrappedRecorder;

        private readonly TypeRecorder Type;
        private readonly ConstructorRecorder Constructor;
        private readonly NamedRecorder Named;

        private bool HasBeenBuilt;

        public Recorder(ISyntacticRecorder<TRecordBuilder> wrappedRecorder)
        {
            WrappedRecorder = wrappedRecorder;

            Type = new(wrappedRecorder.Type);
            Constructor = new(wrappedRecorder.Constructor);
            Named = new(wrappedRecorder.Named);
        }

        ISyntacticTypeRecorder ISyntacticRecorder.Type => Type;
        ISyntacticConstructorRecorder ISyntacticRecorder.Constructor => Constructor;
        ISyntacticNamedRecorder ISyntacticRecorder.Named => Named;

        TRecord ISyntacticRecorder<TRecord>.BuildRecord()
        {
            if (HasBeenBuilt)
            {
                throw new InvalidOperationException($"Cannot build a record of the recorded arguments, as one has already been built.");
            }

            HasBeenBuilt = true;

            Type.SetHasBeenBuilt();
            Constructor.SetHasBeenBuilt();
            Named.SetHasBeenBuilt();

            return WrappedRecorder.BuildRecord().Build();
        }

        private static void ThrowModifyAfterBuild() => throw new InvalidOperationException($"Cannot record the argument, as the record of recorded arguments has already been built");

        private sealed class TypeRecorder : ISyntacticTypeRecorder
        {
            private readonly ISyntacticTypeRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public TypeRecorder(ISyntacticTypeRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool ISyntacticTypeRecorder.TryRecordArgument(ITypeParameterSymbol parameter, ExpressionSyntax syntax)
            {
                if (HasBeenBuilt)
                {
                    ThrowModifyAfterBuild();
                }

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (syntax is null)
                {
                    throw new ArgumentNullException(nameof(syntax));
                }

                return WrappedRecorder.TryRecordArgument(parameter, syntax);
            }
        }

        private sealed class ConstructorRecorder : ISyntacticConstructorRecorder
        {
            private readonly ISyntacticConstructorRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public ConstructorRecorder(ISyntacticConstructorRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool ISyntacticConstructorRecorder.TryRecordArgument(IParameterSymbol parameter, ExpressionSyntax syntax)
            {
                VerifyCanModify();

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (syntax is null)
                {
                    throw new ArgumentNullException(nameof(syntax));
                }

                return WrappedRecorder.TryRecordArgument(parameter, syntax);
            }

            bool ISyntacticConstructorRecorder.TryRecordDefaultArgument(IParameterSymbol parameter)
            {
                VerifyCanModify();

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                return WrappedRecorder.TryRecordDefaultArgument(parameter);
            }

            bool ISyntacticConstructorRecorder.TryRecordParamsArgument(IParameterSymbol parameter, IReadOnlyList<ExpressionSyntax> elementSyntax)
            {
                VerifyCanModify();

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (elementSyntax is null)
                {
                    throw new ArgumentNullException(nameof(elementSyntax));
                }

                return WrappedRecorder.TryRecordParamsArgument(parameter, elementSyntax);
            }

            private void VerifyCanModify()
            {
                if (HasBeenBuilt)
                {
                    ThrowModifyAfterBuild();
                }
            }
        }

        private sealed class NamedRecorder : ISyntacticNamedRecorder
        {
            private readonly ISyntacticNamedRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public NamedRecorder(ISyntacticNamedRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool ISyntacticNamedRecorder.TryRecordArgument(string parameterName, ExpressionSyntax syntax)
            {
                if (HasBeenBuilt)
                {
                    ThrowModifyAfterBuild();
                }

                if (parameterName is null)
                {
                    throw new ArgumentNullException(nameof(parameterName));
                }

                if (syntax is null)
                {
                    throw new ArgumentNullException(nameof(syntax));
                }

                return WrappedRecorder.TryRecordArgument(parameterName, syntax);
            }
        }
    }
}
