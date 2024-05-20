using System;
using System.Collections.Generic;

namespace LayoutManager.Domain.Entities;

public partial class Item
{
    public long FolderId { get; set; }

    public long ItemId { get; set; }

    public string? ItemName { get; set; }

    public string? ItemContent { get; set; }
}
