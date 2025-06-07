﻿using System.ComponentModel.DataAnnotations;

public class EventEntity {
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string Information { get; set; } = null!;


}
