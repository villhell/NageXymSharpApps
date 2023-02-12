using System.Collections.Generic;

namespace NageXymSharpApps.Shared.CatSdk
{
    public interface IStruct : ISerializer
    {
        public Dictionary<string, string> TypeHints { get; }
    }
}