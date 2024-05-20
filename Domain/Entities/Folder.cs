using System;
using System.Collections.Generic;

namespace LayoutManager.Domain.Entities;

public partial class Folder
{
    public string ItemType { get; set; } = null!;

    public long OwnerId { get; set; }

    public long FolderId { get; set; }

    public string? FolderName { get; set; }

    public int? FolderDisplayOrder { get; set; }
}
