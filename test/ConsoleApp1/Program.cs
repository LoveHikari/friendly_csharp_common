// See https://aka.ms/new-console-template for more information

using Hikari.Common.Hardware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;



HardwareInfo hardwareInfo1 = new HardwareInfo();
var v1 = hardwareInfo1.GetCpuList();

Hardware.Info.IHardwareInfo hardwareInfo = new Hardware.Info.HardwareInfo();

hardwareInfo.RefreshAll();
var v = hardwareInfo.CpuList;

Console.WriteLine("Hello, World!");
