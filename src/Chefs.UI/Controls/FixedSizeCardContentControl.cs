using System;
using System.Collections.Generic;
using System.Text;
using Uno.Toolkit.UI;
using Windows.Foundation;

namespace Chefs.Controls
{
    //Using this custom local FixedCardContentControl as a workaround for now regarding this WinUI issue:
    //https://github.com/unoplatform/uno.chefs/issues/144#issuecomment-1400665013
    public partial class FixedSizeCardContentControl : CardContentControl
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