﻿using chess4connect.Enums;
using chess4connect.Models.Database.Entities;

namespace chess4connect.Models.Database.DTOs;

public class PlayDto
{
    public int Id { get; set; }
    public User User { get; set; }
    public User Opponent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public PlayState PlayState { get; set; }
    public Game Game { get; set; }
}
