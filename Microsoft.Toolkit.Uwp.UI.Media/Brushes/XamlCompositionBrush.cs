﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Brushes.Base;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// A <see langword="delegate"/> that represents a custom effect animation that can be applied to a <see cref="XamlCompositionBrush"/> instance
    /// </summary>
    /// <param name="value">The animation target value</param>
    /// <param name="ms">The animation duration, in milliseconds</param>
    /// <returns>A <see cref="Task"/> that completes when the target animation completes</returns>
    public delegate Task XamlEffectAnimation(float value, int ms);

    /// <summary>
    /// A simple <see langword="class"/> that can be used to quickly create XAML brushes from arbitrary <see cref="PipelineBuilder"/> pipelines
    /// </summary>
    public sealed class XamlCompositionBrush : XamlCompositionEffectBrushBase
    {
        /// <summary>
        /// Gets the <see cref="PipelineBuilder"/> pipeline for the current instance
        /// </summary>
        public PipelineBuilder Pipeline { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XamlCompositionBrush"/> class.
        /// </summary>
        /// <param name="pipeline">The <see cref="PipelineBuilder"/> instance to create the effect</param>
        public XamlCompositionBrush(PipelineBuilder pipeline) => this.Pipeline = pipeline;

        /// <summary>
        /// Binds an <see cref="EffectAnimation"/> to the composition brush in the current instance
        /// </summary>
        /// <param name="animation">The input animation</param>
        /// <param name="bound">The resulting animation</param>
        /// <returns>The current <see cref="XamlCompositionBrush"/> instance</returns>
        [Pure]
        public XamlCompositionBrush Bind(EffectAnimation animation, out XamlEffectAnimation bound)
        {
            bound = (value, ms) => animation(this.CompositionBrush, value, ms);

            return this;
        }

        /// <inheritdoc cref="XamlCompositionEffectBrushBase"/>
        protected override PipelineBuilder OnBrushRequested() => this.Pipeline;

        /// <summary>
        /// Clones the current instance by rebuilding the source <see cref="Windows.UI.Xaml.Media.Brush"/>. Use this method to reuse the same effects pipeline on a different <see cref="Windows.UI.Core.CoreDispatcher"/>
        /// </summary>
        /// <returns>A <see cref="XamlCompositionBrush"/> instance using the current effects pipeline</returns>
        [Pure]
        public XamlCompositionBrush Clone()
        {
            if (this.Dispatcher.HasThreadAccess)
            {
                throw new InvalidOperationException("The current thread already has access to the brush dispatcher, so a clone operation is not necessary. " +
                                                    "You can just assign this brush to an arbitrary number of controls and it will still work correctly. " +
                                                    "This method is only meant to be used to create a new instance of this brush using the same pipeline, " +
                                                    "on threads that can't access the current instance, for example in secondary app windows.");
            }

            return new XamlCompositionBrush(this.Pipeline);
        }
    }
}