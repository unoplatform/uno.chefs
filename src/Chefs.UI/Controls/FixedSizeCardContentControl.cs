using System;
using System.Collections.Generic;
using System.Text;
using Uno.Toolkit.UI;
using Windows.Foundation;

namespace Chefs.Controls
{
    public class FixedSizeCardContentControl : CardContentControl
    {
        public FixedSizeCardContentControl()
        {
            DefaultStyleKey = typeof(CardContentControl);
        }

        protected override Size MeasureOverride(Size availableSize) => new Size(availableSize.Width, base.MeasureOverride(availableSize).Height);

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            return size;
        }

    }
}
