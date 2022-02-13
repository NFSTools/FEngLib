using FEngLib.Tags;

namespace FEngLib.Objects;

public abstract class ObjectTag : Tag
{
    protected ObjectTag(IObject<ObjectData> frontendObject)
    {
        FrontendObject = frontendObject;
    }

    protected IObject<ObjectData> FrontendObject { get; }
}