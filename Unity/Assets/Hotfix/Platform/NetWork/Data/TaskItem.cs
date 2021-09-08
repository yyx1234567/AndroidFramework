using System.Collections.Generic;

public class Items
{
    /// <summary>
    /// 
    /// </summary>
    public string code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string virtualAidId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string lastModificationTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string lastModifierId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string creationTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string creatorId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string id { get; set; }
}

public class TaskItem
{
    /// <summary>
    /// 
    /// </summary>
    public List<Items> items { get; set; }
}
