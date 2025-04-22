using System;
using System.Collections.Generic;
using Hikari.Common.Hardware.Components;

namespace Hikari.Common.Hardware.Components;

/// <summary>
/// WMI class: Win32_Processor
/// </summary>
public class CpuInfo
{
    /// <summary>
    /// 简短描述（单行字符串）
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// 处理器的当前速度，以 MHz 为单位。
    /// </summary>
    public uint CurrentClockSpeed { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 2 级处理器缓存的大小。 二级缓存是一个外部存储区，其访问时间比主 RAM 存储器快。
    /// </summary>
    public uint L2CacheSize { get; set; }

    /// <summary>
    /// 3 级处理器缓存的大小。 三级缓存是一个外部存储区，其访问时间比主 RAM 存储器快。
    /// </summary>
    public uint L3CacheSize { get; set; }

    /// <summary>
    /// 处理器制造商的名称。
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 处理器的最大速度，以 MHz 为单位
    /// </summary>
    public uint MaxClockSpeed { get; set; }

    /// <summary>
    /// 识别对象的标签.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 当前处理器实例的内核数。 核心是集成电路上的物理处理器。 例如，在双核处理器中，此属性的值为 2。
    /// </summary>
    public uint NumberOfCores { get; set; }

    /// <summary>
    /// 当前处理器实例的逻辑处理器数。 对于支持超线程的处理器，此值仅包括启用了超线程的处理器。
    /// </summary>
    public uint NumberOfLogicalProcessors { get; set; }

    /// <summary>
    /// 描述处理器特性的处理器信息。
    /// 对于 x86 类 CPU，字段格式取决于 CPUID 指令的处理器支持。
    /// 如果支持该指令，则该属性包含 2（两个）DWORD 格式的值。
    /// 第一个是 08h-0Bh 的偏移量，这是 CPUID 指令在输入 EAX 设置为 1 时返回的 EAX 值。
    /// 第二个是0Ch-0Fh的偏移量，也就是指令返回的EDX值。
    /// 只有属性的前两个字节是有效的，并且包含 CPU 复位时 DX 寄存器的内容——所有其他字节都设置为 0（零），并且内容为 DWORD 格式。
    /// </summary>
    public string ProcessorId { get; set; } = string.Empty;

    /// <summary>
    /// 如果为 True，则处理器支持用于虚拟化的地址转换扩展。
    /// </summary>
    public bool SecondLevelAddressTranslationExtensions { get; set; }

    /// <summary>
    /// 电路上使用的芯片插座类型
    /// </summary>
    public string SocketDesignation { get; set; } = string.Empty;

    /// <summary>
    /// 如果为 True，则固件已启用虚拟化扩展。
    /// </summary>
    public Boolean VirtualizationFirmwareEnabled { get; set; }

    /// <summary>
    /// 如果为 True，则处理器支持 Intel 或 AMD Virtual Machine Monitor 扩展。
    /// </summary>
    public Boolean VMMonitorModeExtensions { get; set; }

    /// <summary>
    /// % Processor Time 是处理器执行非空闲线程所花费的时间百分比。
    /// 计算方法是测量处理器执行空闲线程所花费的时间百分比，然后从 100% 中减去该值。
    ///（每个处理器都有一个空闲线程，在没有其他线程准备运行时消耗周期）。
    /// 这个计数器是处理器活动的主要指标，显示采样间隔期间观察到的平均繁忙时间百分比。
    /// 需要注意的是，处理器是否空闲的计费计算是以系统时钟的内部采样间隔（10ms）进行的。
    /// 在当今的快速处理器上，% Processor Time 因此可能会低估处理器利用率，因为处理器可能会在系统时钟采样间隔之间花费大量时间为线程服务。
    /// 基于工作负载的计时器应用程序是更可能测量不准确的应用程序示例之一，因为计时器在采样后立即发出信号。
    /// </summary>
    public UInt64 PercentProcessorTime { get; set; }

    public List<CpuCoreInfo> CpuCoreList { get; set; } = new List<CpuCoreInfo>();
}
