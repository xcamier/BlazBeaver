namespace BlazBeaver.Data;

    public class TreeItemData
    {
        public string Guid { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string DisplayText { get; set; } = string.Empty;
        public bool IsFolder { get; set; }
        public string Icon { get; set; } = string.Empty;
        public bool IsExpanded { get; set; }

        public HashSet<TreeItemData> TreeItems { get; set; } = new HashSet<TreeItemData>();
    }
