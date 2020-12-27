using System;
using System.Collections.Generic;
using System.Text;

namespace Wavepager
{
    public partial class WebpageAccessor
    {
        partial void AccessImpl(string url);
        public void Access(string url)
        {
            AccessImpl(url);
        }
    }
}
