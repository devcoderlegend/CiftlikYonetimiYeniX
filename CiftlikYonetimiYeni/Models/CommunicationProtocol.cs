using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class CommunicationProtocol
{
    public int Id { get; set; }

    public string? ProtocolName { get; set; }

    public string? ProtocolDescription { get; set; }

    public string? PrimitiveDataType { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? Active { get; set; }
}
