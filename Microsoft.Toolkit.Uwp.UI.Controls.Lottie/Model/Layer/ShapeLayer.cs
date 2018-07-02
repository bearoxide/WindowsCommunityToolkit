// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer
{
    internal class ShapeLayer : BaseLayer
    {
        private readonly ContentGroup _contentGroup;

        internal ShapeLayer(LottieDrawable lottieDrawable, Layer layerModel)
            : base(lottieDrawable, layerModel)
        {
            // Naming this __container allows it to be ignored in KeyPath matching.
            ShapeGroup shapeGroup = new ShapeGroup("__container", layerModel.Shapes);
            _contentGroup = new ContentGroup(lottieDrawable, this, shapeGroup);
            _contentGroup.SetContents(new List<IContent>(), new List<IContent>());
        }

        public override void DrawLayer(BitmapCanvas canvas, Matrix3X3 parentMatrix, byte parentAlpha)
        {
            _contentGroup.Draw(canvas, parentMatrix, parentAlpha);
        }

        public override void GetBounds(out Rect outBounds, Matrix3X3 parentMatrix)
        {
            base.GetBounds(out outBounds, parentMatrix);
            _contentGroup.GetBounds(out outBounds, BoundsMatrix);
        }

        internal override void ResolveChildKeyPath(KeyPath keyPath, int depth, List<KeyPath> accumulator, KeyPath currentPartialKeyPath)
        {
            _contentGroup.ResolveKeyPath(keyPath, depth, accumulator, currentPartialKeyPath);
        }
    }
}