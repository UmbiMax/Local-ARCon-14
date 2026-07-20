using System.Threading.Tasks;
using Content.Shared._Starlight.TextToSpeech;
using Robust.Shared.ContentPack;
using Robust.Shared.Utility;

namespace Content.Server._Arcane.TTS;

public sealed partial class ArcaneTTSSystem
{
    [Dependency] private IResourceManager _resourceManager = default!;

    private ResPath GetCacheId(VoicePrototype voicePrototype, string cacheId)
    {
        var resPath = new ResPath($"voicecache/{voicePrototype.ID}/{cacheId}.ogg").ToRootedPath();
        _resourceManager.UserData.CreateDir(resPath.Directory);
        return resPath.ToRootedPath();
    }
    private async Task<byte[]?> GetFromCache(ResPath resPath)
    {
        if (!_resourceManager.UserData.Exists(resPath))
            return null;

        await using var reader = _resourceManager.UserData.OpenRead(resPath);
        return reader.CopyToArray();
    }

    private async Task SaveVoiceCache(ResPath resPath, byte[] data)
    {
        await using var writer = _resourceManager.UserData.OpenWrite(resPath);
        await writer.WriteAsync(data);
    }
}
