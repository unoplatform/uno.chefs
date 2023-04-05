using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;

namespace Chefs.Controls
{
    //Using this custom local FixedSizeChip as a workaround for now regarding this WinUI issue:
    //https://github.com/unoplatform/uno.chefs/issues/144#issuecomment-1400665013
    public partial class FixedSizeChip : Chip
    {
        public FixedSizeChip()
        {
            DefaultStyleKey = typeof(Chip);
        }

        protected override Size MeasureOverride(Size availableSize) => new Size(availableSize.Width, base.MeasureOverride(availableSize).Height);

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            return size;
        }

    }
}