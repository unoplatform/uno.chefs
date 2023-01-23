using System;
using System.Collections.Generic;
using System.Text;
using Uno.Toolkit.UI;
using Windows.Foundation;

namespace Chefs
{
    public class CardContentControlTest : CardContentControl
       // FixedSizeCardContentControl
    {
       // private Size? _size;

        public CardContentControlTest()
        {
            DefaultStyleKey = typeof(CardContentControl);
        }

        // protected override Size MeasureOverride(Size availableSize) => new Size(availableSize.Width, base.MeasureOverride(availableSize).Height);

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = base.MeasureOverride(availableSize);
            //return _size ??= new Size(availableSize.Width, size.Height);
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            //  _size ??= size;
            return size;
        }

    }
}
