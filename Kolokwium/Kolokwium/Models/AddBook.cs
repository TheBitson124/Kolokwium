﻿namespace Kolokwium.Models;

public class AddBook
{
    public  string title{ get; set; }
    public List<string> genres { get; set; } = new List<string>();

}